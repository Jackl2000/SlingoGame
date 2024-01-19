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
    public TextMeshProUGUI spinCountHeader;
    [HideInInspector] private CollectReward collectReward;

    [Space(5)]
    public TextMeshProUGUI balanceText;

    [Space(10)]
    [Header("Spin settings")]
    public TextMeshProUGUI spinLeftText;
    public Button spinButton;
    public float spinSpeed = 5f;
    /// <summary>
    /// wait time before next spin, when auto spin
    /// </summary>
    [SerializeField] private float spinWaitTime; 
    [SerializeField] private int wildChance;

    [Space(10)]
    public List<GameObject> slotsList;
    List<TextMeshProUGUI> slotTextList = new List<TextMeshProUGUI>();
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
    private Animator spinAnimation;
    [SerializeField] private bool isSpinning = false;

    PanelEffects[] blinkEffect;
    private IEnumerator spinCoroutine;
    #endregion

    private void Awake()
    {
        collectReward = this.gameObject.GetComponentInParent<CollectReward>();
        spinAnimation = GetComponentInChildren<Animator>();
        foreach (GameObject spinSlot in slotsList)
        {
            slotTextList.Add(spinSlot.GetComponentInChildren<TextMeshProUGUI>());
        }
    }

    private void Start()
    {
        //TestCalculation();
        spinCoroutine = AutoSpin(false);
    }

    public void Spin()
    {
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
    }

    /// <summary>
    /// Interpolates between a and b
    /// </summary>
    void NumberSpinning()
    {
        min = 1;
        max = 16;

        if (spinAnimation.GetBool("Spinning"))
        {
            foreach (var text in slotTextList)
            {
                min += 15;
                max += 15;
                int numbers = (int)Mathf.Lerp(min, max, Mathf.PingPong(Time.time * spinSpeed, 1));
                text.text = numbers.ToString();
            }
        }
        
    }


    private void SpinsLeft()
    {
        if (spinLeft <= 0)
        {
            playerData.balance -= PriceCaculator();
            isSpinning = false;
        }

        if (spinLeft >= 1)
        {
            spinLeft--;
            spinLeftText.text = spinLeft.ToString();
            if (spinLeft <= 0)
            {
                spinCountHeader.text = "COST";
                PriceCaculator();
                isSpinning = false;
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

    public void CheckMatchingNumb()
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
        }
        if(wildPicked == wildPicks)
        {
            if (spinLeft <= 0)
            {
                spinCountHeader.text = "COST";
                PriceCaculator();
                isSpinning = false;
            }
            if (spinLeft > 0)
            {
                StartCoroutine(spinCoroutine);
                retryButton.enabled = false;
            }
        }

    }

    IEnumerator AutoSpin(bool once)
    {
        if(!once)
        {
            for (int spinCount = 0; spinCount <= spinLeft;)
            {
                if (spinLeft != 8)
                {
                    yield return new WaitForSeconds(1);
                }
                foreach (GameObject slot in slotsList)
                {
                    if(slot.GetComponentInChildren<Image>().enabled)
                    {
                        slot.GetComponentInChildren<Image>().enabled = false;
                        slot.GetComponentInChildren<TextMeshProUGUI>().text = "?";
                    }   
                }
                spinAnimation.SetBool("Spinning", true);
                yield return new WaitForSeconds(spinWaitTime);
                spinAnimation.SetBool("Spinning", false);
                Spin();
                CheckMatchingNumb();
                SpinsLeft();
            }
        }
        else
        {
            foreach (GameObject slot in slotsList)
            {
                if (slot.GetComponentInChildren<Image>().enabled)
                {
                    slot.GetComponentInChildren<Image>().enabled = false;
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = "?";
                }
            }
            
            spinAnimation.SetBool("Spinning", true);
            yield return new WaitForSeconds(spinWaitTime);
            spinAnimation.SetBool("Spinning", false);
            Spin();
            CheckMatchingNumb();
            SpinsLeft();
        }
    }

    public void StartSpin( )
    {
        if (isSpinning) return;

        if (spinLeft == 8)
        {
            playerData.balance -= spinBets;
        }
        isSpinning = true;
        if (spinLeft < 0)
        {
            StartCoroutine(AutoSpin(true));
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
        NumberSpinning();
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
