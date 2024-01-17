using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class spin : MonoBehaviour
{
    [Header("References")]
    public GridGeneration gridGeneration;
    public GridCheck gridCheck;
    public PlayerData playerData;
    public Button retryButton;
    [HideInInspector] private CollectReward collectReward;

    [Space(5)]
    public TextMeshProUGUI balanceText;

    [Space(10)]
    [Header("Spin settings")]
    public TextMeshProUGUI spinLeftText;
    public Button spinButton;
    /// <summary>
    /// wait time before next spin, when auto spin
    /// </summary>
    [SerializeField] private float spinWaitTime; 
    [SerializeField] private int wildChance;

    [Space(10)]
    public List<GameObject> slotsList;
    public List<int> spinNumbers;

    #region others variables
    [HideInInspector] public float spinBets = 1;
    /// <summary>
    /// How many wildpicks you have
    /// </summary>
    [HideInInspector] public int wildPicks = 0;
    /// <summary>
    /// spins left before you pay
    /// </summary>
    [HideInInspector] public int spinLeft = 8;
    int rnd;
    int min = 1;
    int max = 15;
    int wildPicked = 0;
    private int possibleRewardAmplifiere;

    PanelEffects[] blinkEffect;
    private IEnumerator spinCoroutine;
    #endregion

    private void Awake()
    {
        collectReward = this.gameObject.GetComponentInParent<CollectReward>();
    }

    private void Start()
    {
        //TestCalculation();
        spinCoroutine = AutoSpin();
    }

    public void Spin()
    {
        foreach(GameObject go in slotsList)
        {
            go.GetComponentInChildren<Image>(true).enabled = false;
        }

        min = 1;
        max = 16;

        if (spinNumbers.Count >= 5)
        {
            spinNumbers.Clear();
        }

        foreach (var spinSlot in slotsList)
        {
            rnd = UnityEngine.Random.Range(min, max);
            min += 15;
            max += 15;

            int wildPick = UnityEngine.Random.Range(0, wildChance + 1);

            if (wildPick == 5)
            {
                spinNumbers.Add(0);
                spinSlot.GetComponentInChildren<Image>().enabled = true;
                spinSlot.GetComponentInChildren<TextMeshProUGUI>().text = "";

                blinkEffect = FindObjectsByType<PanelEffects>(FindObjectsSortMode.None);

                for (int i = 0; i < blinkEffect.Length; i++)
                {
                    blinkEffect[i].FlashingEffect();
                }
                StopCoroutine(spinCoroutine);
                wildPicks++;
            }
            else
            {
                spinSlot.GetComponentInChildren<TextMeshProUGUI>().text = rnd.ToString();
                spinNumbers.Add(rnd);
            }
        }

        CheckMatchingNumb();

        if (spinLeft <= 0)
        {
            playerData.balance -= PriceCaculator();
        }

        if (spinLeft >= 1)
        {
            spinLeft--;
            spinLeftText.text = spinLeft.ToString();
            if(spinLeft <= 0)
            {
                PriceCaculator();
            }
        }
    }

    private float PriceCaculator()
    {
        if(gridCheck.slingoCount == 12)
        {
            return 0;
        }
        possibleRewardAmplifiere = gridCheck.CheckForMaxReward();

        float starMultipliere = 0.015f + (gridCheck.starsCount * 0.05f);
        float slingoReward = 0.015f * (gridCheck.starsCount * 5);
        if (gridCheck.slingoCount == 0)
        {
            slingoReward = 0.015f + starMultipliere;
        }
        if (gridCheck.rewards.ContainsKey(gridCheck.slingoCount + 1))
        {
            slingoReward = (gridCheck.rewards[gridCheck.slingoCount + 1] / spinBets) / Mathf.Clamp((1 + gridCheck.slingoCount) / gridCheck.slingoCount, 2, (1 + gridCheck.slingoCount) / gridCheck.slingoCount) * (starMultipliere + 0.5f);
        }

        float maxSlingoAmplifiere = Mathf.Clamp(possibleRewardAmplifiere - 0.5f, 0.5f, 1.8f);
        float price = slingoReward * Mathf.Clamp(maxSlingoAmplifiere, 1, maxSlingoAmplifiere);
        price *= spinBets;
        spinLeftText.text = UIManager.Instance.DisplayMoney(price);
        return price;
    }

    private void TestCalculation()
    {
        float bet = 1; 
        for (float m = 0; m < 3; m++)
        {
            for (float i = 0; i < 11; i++)
            {
                for (float j = 10; j < 25; j++)
                {
                    float multipliere = 0.015f + (j * 0.05f);
                    float slingoRewards = 0.015f * (j * 5);
                    if (i == 0)
                    {
                        slingoRewards = 0.015f + multipliere;
                    }
                    if (gridCheck.rewards.ContainsKey(Convert.ToInt32(i) + 1))
                    {
                        slingoRewards = (gridCheck.rewards[Convert.ToInt32(i) + 1] / bet) / Mathf.Clamp((1 + i) / i, 2, (1 + i) / i) * (multipliere + 0.5f);
                    }
                    float maxSlingoAmplifiere = Mathf.Clamp(m - 0.5f, 0.5f, 1.8f);
                    float price = slingoRewards * Mathf.Clamp(maxSlingoAmplifiere, 1, maxSlingoAmplifiere);
                    price *= bet;
                    Debug.Log("SlingoCount: " + i + " Starscount: " + j + " Multipliere: " + multipliere + " Amplifiere: " + maxSlingoAmplifiere + " final value: " + UIManager.Instance.DisplayMoney(price));
                }
            }
        }
    }

    void CheckMatchingNumb()
    {
        foreach (int spin in spinNumbers)
        {
            foreach (int gridNumber in gridGeneration.numberPositions.Keys)
            {
                if (spin == gridNumber)
                {
                    gridGeneration.numberPositions[gridNumber].Hit(false);
                    break;
                }
            }
        }
    }

    public void WildPick(Button gridButton)
    {
        if (wildPicked < wildPicks)
        {
            wildPicked++;
            foreach (int gridNumber in gridGeneration.numberPositions.Keys)
            {
                if (gridButton.gameObject.GetComponent<TextMeshProUGUI>().text != "")
                {
                    if (gridNumber == Convert.ToInt32(gridButton.gameObject.GetComponent<TextMeshProUGUI>().text))
                    {
                        gridGeneration.numberPositions[gridNumber].Hit(true);
                    }
                }
            }
            if (spinLeft <= 0)
            {
                PriceCaculator();
            }
        }
    }

    IEnumerator AutoSpin()
    {
        for (int spinCount = 0; spinCount <= spinLeft;)
        {
            Spin();

            //Debug.Log("Spin running"    +       "\n"     +   "Spin left:" + spinLeft);
            yield return new WaitForSeconds(spinWaitTime);
        }

    }

    public void StartSpin( )
    {
        Debug.Log("Spinsleft: " + spinLeft);
        if (spinLeft == 8)
        {
            playerData.balance -= spinBets;
        }

        if (spinLeft < 0)
        {
            Spin();
            //reset time for collect reward pop message
            collectReward.ResetTime();
        }
        else
        {
            StartCoroutine(spinCoroutine);
            retryButton.enabled = false;
        }
    }

    private void Update()
    {
        if (spinLeft == 0)
        {
            StopCoroutine(spinCoroutine);
            //spinLeft remains zero causing loop to be entered constantly, unless its set to -1
            spinLeft = -1;
            retryButton.enabled = true;

            Debug.Log("Spinleft:" + spinLeft +
                "\n" + "Spinning stopped");
        }

        balanceText.text = UIManager.Instance.DisplayMoney(playerData.balance);

        #region Enables to pick any number on plate if user got wildpicks
        if (wildPicked == wildPicks)
        {
            wildPicks = 0;
            wildPicked = 0;
            spinButton.enabled = true;
        }
        else
        {
            spinButton.enabled = false;
        }
        #endregion

    }
}
