using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class spin : MonoBehaviour
{
    [Header("References")]
    public GridGeneration gridGeneration;
    public GridCheck gridCheck;
    public PlayerData playerData;
    [HideInInspector] private CollectReward collectReward;

    [Space(5)]
    public TextMeshProUGUI balanceText;

    [Space(10)]
    [Header("Spin settings")]
    public TextMeshProUGUI spinLeftText;
    public Button spinButton;
    [SerializeField] private float spinWaitTime;

    [Space(10)]
    public List<GameObject> slotsList;
    public List<int> spinNumbers;

    #region others variables
    [HideInInspector] public float spinBets = 1;
    [HideInInspector] public int wCount = 0;
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

        foreach (var slotText in slotsList)
        {
            rnd = UnityEngine.Random.Range(min, max);
            min += 15;
            max += 15;

            int wildPick = UnityEngine.Random.Range(0, 18);

            if (wildPick == 0)
            {
                spinNumbers.Add(0);
                slotText.GetComponentInChildren<Image>().enabled = true;
                slotText.GetComponentInChildren<TextMeshProUGUI>().text = "";

                blinkEffect = FindObjectsByType<PanelEffects>(FindObjectsSortMode.None);

                for (int i = 0; i < blinkEffect.Length; i++)
                {
                    blinkEffect[i].FlashingEffect();
                }
                StopCoroutine(spinCoroutine);
                wCount++;
            }
            else
            {
                slotText.GetComponentInChildren<TextMeshProUGUI>().text = rnd.ToString();
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
        float slingoReward = 0.015f * gridCheck.starsCount;
        if (gridCheck.slingoCount == 0)
        {
            slingoReward = 0.015f;
        }
        if (gridCheck.rewards.ContainsKey(gridCheck.slingoCount + 1))
        {
            slingoReward = (gridCheck.rewards[gridCheck.slingoCount + 1] / spinBets) / Mathf.Clamp((10 + (gridCheck.slingoCount * 2)) / gridCheck.slingoCount, 2, (10 + (gridCheck.slingoCount * 2)) / gridCheck.slingoCount) * (starMultipliere + 0.5f);
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
                    float slingoRewards = 0.015f * j;
                    if (i == 0)
                    {
                        slingoRewards = 0.015f;
                    }
                    if (gridCheck.rewards.ContainsKey(Convert.ToInt32(i) + 1))
                    {
                        slingoRewards = gridCheck.rewards[Convert.ToInt32(i) + 1] / Mathf.Clamp((10 + (i * 2)) / i, 2, (10 + (i * 2)) / i) * (multipliere + 0.5f);
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
        if (wildPicked < wCount)
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
            Debug.Log("Spin running" + "\n" +
                        "Spin left:" + spinLeft);
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
        }
    }

    private void Update()
    {
        if (spinLeft == 0)
        {
            StopCoroutine(spinCoroutine);
            //spinLeft remains zero causing loop to be entered constantly, unless its set to -1
            spinLeft = -1;
            //PriceCaculator();

            Debug.Log("Spinleft:" + spinLeft +
                "\n" + "Spinning stopped");
        }

        balanceText.text = UIManager.Instance.DisplayMoney(playerData.balance);

        #region Enables to pick any number on plate if user got wildpicks
        if (wildPicked == wCount)
        {
            wCount = 0;
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
