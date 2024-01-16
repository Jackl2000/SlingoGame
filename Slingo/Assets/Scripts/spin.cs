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
    public CollectReward collectReward;

    public TextMeshProUGUI price;
    public TextMeshProUGUI balanceText;

    [Space(10)]
    [Header("Spin settings")]
    public TextMeshProUGUI spinLeftText;
    public Button spinButton;
    [Space(5)]
    public float spinPrice;
    [SerializeField]private float spinWaitTime;

    [Space(10)]
    public List<GameObject> slotsList;
    public List<int> spinNumbers;

    #region private variables
    [HideInInspector] public int wCount = 0;
    [HideInInspector] public int spinLeft = 8;
    int rnd;
    int min = 1;
    int max = 15;
    int wildPick;
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
                playerData.balance -= PriceCaculator();
            }
        }


    }

    private float PriceCaculator()
    {
        possibleRewardAmplifiere = gridCheck.CheckForMaxReward();

        float starMultipliere = 0.015f + (gridCheck.starsCount * (0.05f + (gridCheck.slingoCount / 10)));

        float slingoReward = gridCheck.slingoCount * starMultipliere;

        if (gridCheck.rewards.ContainsKey(gridCheck.slingoCount + 1))
        {
            slingoReward = gridCheck.rewards[gridCheck.slingoCount + 1] / Mathf.Clamp(5 /gridCheck.slingoCount, 2, 5 / gridCheck.slingoCount) + starMultipliere;
        }

        float maxSlingoAmplifiere = possibleRewardAmplifiere / 2;
        float price = slingoReward * Mathf.Clamp(maxSlingoAmplifiere, 1, maxSlingoAmplifiere);

        spinLeftText.text = UIManager.Instance.DisplayMoney(Mathf.Clamp(price, 0.015f, price));
        return Mathf.Clamp(price, 0.015f, price);
    }

    private void TestCalculation()
    {
        for (int i = 0; i < 11; i++)
        {
            for (float j = 10; j < 25; j++)
            {
                float multipliere = 0.015f + (j * (0.05f + (i / 10)));
                float slingoRewards = i * multipliere;
                if (gridCheck.rewards.ContainsKey(i + 1))
                {
                    slingoRewards = gridCheck.rewards[i + 1] / Mathf.Clamp(5 / i, 2, 5 / i) + multipliere;
                }
                float price = Mathf.Clamp(slingoRewards * multipliere, 0.015f, slingoRewards * multipliere);
                Debug.Log("SlingoCount: " + i + " Starscount: " + j + " Multipliere: " + multipliere + " final value: " + UIManager.Instance.DisplayMoney(price));
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
        balanceText.text = UIManager.Instance.DisplayMoney(playerData.balance);

        if (spinLeft == 0)
        {
            StopCoroutine(spinCoroutine);
            //spinLeft remains zero causing loop to be entered constantly, unless its set to -1
            spinLeft = -1;
            //PriceCaculator();
            Debug.Log("Spinleft is:" + spinLeft +
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
