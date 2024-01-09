using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class spin : MonoBehaviour
{
    public List<TMP_Text> slotTextList;
    public GridGeneration gridGenScript;
    public List<int> spinNumbers;

    [HideInInspector]
    public int slotNumber;
    
    int rnd;
    int min = 1;
    int max = 15;



    public void Spin()
    {
        if (spinNumbers.Count >= 5)
        {
            spinNumbers.Clear();
        }

        foreach (var slotText in slotTextList)
        {
            rnd = Random.Range(min, max);
            min += 15;
            max += 15;

            slotNumber = rnd;
            slotText.text = rnd.ToString();
            spinNumbers.Add(rnd);

            
        }

        foreach (int spin in spinNumbers)
        {
            foreach (int gridNumber in gridGenScript.numberPositions.Keys)
            {
                if (spin == gridNumber)
                {
                    gridGenScript.numberPositions[gridNumber].Hit();
                    Debug.Log("spin: " + spin + "Grid: " + gridNumber);
                }
            }
        }

        min = 1;
        max = 15;
    }

}
