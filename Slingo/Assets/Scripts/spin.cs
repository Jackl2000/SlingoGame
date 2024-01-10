using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class spin : MonoBehaviour
{
    public List<TMP_Text> slotTextList;
    public GridGeneration gridGenScript;
    public List<int> spinNumbers;
    public TextMeshProUGUI spinLeftText;
    public Button spinButton;
    
    int rnd;
    int min = 1;
    int max = 15;
    int wCount = 0;
    int wildPicked = 0;

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
                    Debug.Log("spin: " + spin + "Grid: " + gridNumber);
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
                
            gridButton.gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;
            Debug.Log("Wildpick clicked");
            
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
