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

    PanelEffects[] blinkEffect;
    #endregion

    private IEnumerator spinCoroutine;

    float PriceCaculator()
    {
        if (spinLeft <= 0)
        {
            switch (gridCheck.slingoCount)
            {
                case 0:
                    spinPrice = 0.05f;
                    break;
                case 1:
                    spinPrice = 0.05f;
                    break;
                case 2:
                    spinPrice = .08f;
                    break; 
                case 3:
                    spinPrice = .15f;
                    break;
                case 4:
                    spinPrice = .25f;
                    break;
                case 5:
                    spinPrice = 1.25f;
                    break;
                case 6:
                    spinPrice = 2.4f;
                    break;
                case 7:
                    spinPrice = 11;
                    break;
                case 8:
                    spinPrice = 38;
                    break;
                case 9:
                    spinPrice = 112;
                    break;
                case 10:
                    spinPrice = 160;
                    break;
                case 11:
                    spinPrice = 215;
                    break;
            }

            spinLeftText.text = spinPrice.ToString();
        }
        else
        {
            spinPrice = 0;
        }
        return spinPrice;
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
        }
    }

    public void Spin()
    {
        foreach(GameObject go in slotsList)
        {
            go.GetComponentInChildren<Image>(true).enabled = false;
        }

        min = 1;
        max = 16;
        
        playerData.balance -= PriceCaculator();
        
        if (spinNumbers.Count >= 5)
        {
            spinNumbers.Clear();
        }

        foreach (var slotText in slotsList)
        {
            rnd = UnityEngine.Random.Range(min, max);
            min += 15;
            max += 15;

            wildPick = UnityEngine.Random.Range(0, 75);

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
                wCount++;
                StopCoroutine(spinCoroutine);
            }
            else
            {
                slotText.GetComponentInChildren<TextMeshProUGUI>().text = rnd.ToString();
                spinNumbers.Add(rnd);
            }
        }

        CheckMatchingNumb();

        if (spinLeft >= 1)
        {
            spinLeft = Convert.ToInt32(spinLeftText.text);
            spinLeft--;
            spinLeftText.text = spinLeft.ToString();
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
        }
        else
        {
            StartCoroutine(spinCoroutine);
        }

    }

    private void Start()
    {
        spinCoroutine = AutoSpin();
    }

    private void Update()
    {
        if (spinLeft == 0)
        {
            StopCoroutine(spinCoroutine);
            spinLeft = -1;
            PriceCaculator();
            Debug.Log("Spinleft is:" + spinLeft +
                "\n" + "Spinning stopped");
        }

        if (spinLeft == 0)
        {
            spinLeftText.text = PriceCaculator().ToString() + " kr";
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
