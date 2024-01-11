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
    public GridGeneration gridGenScript;

    [Space(5)]
    [Header("Spin settings")]
    public TextMeshProUGUI spinLeftText;
    public Button spinButton;

    [Space(10)]
    public List<TMP_Text> slotTextList;
    public List<int> spinNumbers;


    #region private variables
    [HideInInspector]
    public int wCount = 0;
    int rnd;
    int min = 1;
    int max = 15;
    int wildPicked = 0;

    PanelEffects[] blinkEffect;
    #endregion

    public void Spin()
    {
        min = 1;
        max = 16;

        if (spinNumbers.Count >= 5)
        {
            spinNumbers.Clear();
        }

        foreach (var slotText in slotTextList)
        {
            rnd = UnityEngine.Random.Range(min, max);
            min += 15;
            max += 15;

            int wildPick = UnityEngine.Random.Range(0, 10);

            if (wildPick == 0)
            {
                spinNumbers.Add(0);
                slotText.text = "W";
                
                blinkEffect = FindObjectsByType<PanelEffects>(FindObjectsSortMode.None);

                for (int i = 0; i < blinkEffect.Length; i++)
                {
                    blinkEffect[i].FlashingEffect();
                }

                wCount++;
            }
            else
            {
                slotText.text = rnd.ToString();
                spinNumbers.Add(rnd);
            }

        }

        CheckMatchingNumb();

        int spinLeft = Convert.ToInt32(spinLeftText.text);
        spinLeft--;
        spinLeftText.text = spinLeft.ToString();
    }

    void CheckMatchingNumb()
    {
        foreach (int spin in spinNumbers)
        {
            foreach (int gridNumber in gridGenScript.numberPositions.Keys)
            {
                if (spin == gridNumber)
                {
                    gridGenScript.numberPositions[gridNumber].Hit();
                }
            }
        }
        
    }

    public void WildPick(Button gridButton)
    {
        if (wildPicked < wCount)
        {
            wildPicked++;
            foreach (int gridNumber in gridGenScript.numberPositions.Keys)
            {
                if(gridNumber == Convert.ToInt32(gridButton.gameObject.GetComponent<TextMeshProUGUI>().text))
                {
                    gridGenScript.numberPositions[gridNumber].Hit();
                }
            }
        }
    }

    private void Update()
    {
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
    }


}
