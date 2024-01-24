using Codice.Client.Common.GameUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private GridCheck gridCheck;
    private GridGeneration gridGeneration;
    private Dictionary<int, int> bestChoiceList;
    // Start is called before the first frame update
    void Start()
    {
        gridCheck = GetComponent<GridCheck>();
        gridGeneration = GetComponent<GridGeneration>();
    }

    public GridNumbers BestChoice()
    {
        int maxSlingoCount = gridCheck.CheckForMaxReward();
        bestChoiceList = BestChoiceList();

        int maxValue = 0;
        int currentNumber = 0;

        if(maxSlingoCount == 0)
        {
            foreach(int item in bestChoiceList.Keys)
            {
                if (bestChoiceList[item] > maxValue)
                {
                    maxValue = bestChoiceList[item];
                    currentNumber = item;
                }
            }
        }
        return gridGeneration.numberPositions[currentNumber];
    }

    private Dictionary<int, int> BestChoiceList()
    {
        Dictionary<int, int> bestChoiceValuesList = new Dictionary<int, int>();
        foreach(GridNumbers number in gridGeneration.numberPositions.Values)
        {
            if(number.hasBeenHit)
            {
                Debug.Log(number.h + ":" + number.v + " has been hit");
                bestChoiceValuesList.Add(number.number, 0);
            }
            else if(number.h == 3 && number.v == 3)
            {
                bestChoiceValuesList.Add(number.number, 5);
            }
            else if(number.diagonal)
            {
                bestChoiceValuesList.Add(number.number, 3);
            }
            else
            {
                bestChoiceValuesList.Add(number.number, 1);
            }
        }
        return bestChoiceValuesList;
        
    } 
}
