using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AI : MonoBehaviour
{
    private GridGeneration gridGeneration;
    private Dictionary<int, int> bestChoiceList;
    [HideInInspector] public int currentNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        gridGeneration = GetComponent<GridGeneration>();
    }

    public GridNumbers BestChoice(int superCounts, List<int> arrowList)
    {
        if(superCounts == 0 && arrowList.Count == 0)
        {
            return null;
        }
        else if(arrowList.Count == 0)
        {
            bestChoiceList = BestChoiceList(0);
        }
        else
        {
            bestChoiceList = BestChoiceList(arrowList[0]);
        }
        if (bestChoiceList.Count == 0) return null;
        if (bestChoiceList.Values.Max() == 0) return null;
        
        int maxValue = 0;
        
        foreach (int item in bestChoiceList.Keys)
        {
            if (bestChoiceList[item] > maxValue)
            {
                maxValue = bestChoiceList[item];
                currentNumber = item;
            }
        }
        if(currentNumber == 0)
        {
            Debug.Log("Should never be played " + bestChoiceList.Values.Max());
        }
        gridGeneration.numberPositions[currentNumber].gameObject.transform.parent.transform.parent.GetComponent<NumberManager>().PlayHighlighterDot();

        return gridGeneration.numberPositions[currentNumber];
    }

    private Dictionary<int, int> BestChoiceList(int hIndex)
    {
        Dictionary<int, int> bestChoiceValuesList = new Dictionary<int, int>();
        foreach(GridNumbers number in gridGeneration.numberPositions.Values)
        {
            if(hIndex != 0 && number.h != hIndex)
            {
                continue;
            }
            int finalScore = 0;
            if(number.hasBeenHit)
            {
                bestChoiceValuesList.Add(number.number, 0);
                continue;
            }
            else if(number.h == 3 && number.v == 3)
            {
                finalScore += 5;
            }
            else if(number.diagonal)
            {
                finalScore += 3;
            }
            else
            {
                finalScore += 1;
            }
            finalScore += SlingoBonus(number);
            bestChoiceValuesList.Add(number.number, finalScore);
        }
        if(bestChoiceValuesList.Count == 0)
        {

        }
        return bestChoiceValuesList;
    }

    private int SlingoBonus(GridNumbers number)
    {
        int hIndex = 0;
        int vIndex = 0;
        int dl = 0;
        int dr = 0;
        foreach(GridNumbers gridNumbers in gridGeneration.numberPositions.Values)
        {
            if(gridNumbers.hasBeenHit)
            {
                if (gridNumbers.h == number.h)
                {
                    hIndex++;
                }
                if (gridNumbers.v == number.v)
                {
                    vIndex++;
                }
                if (number.diagonal)
                {
                    if (number.h == number.v && gridNumbers.h == gridNumbers.v)
                    {
                        dl++;
                    }
                    else if ((number.h == 1 && number.v == 5) || (number.h == 2 && number.v == 4) || (number.h == 4 && number.v == 2) || (number.h == 5 && number.v == 1) || (number.h == 3 && number.v == 3))
                    {
                        if ((gridNumbers.h == 1 && gridNumbers.v == 5) || (gridNumbers.h == 2 && gridNumbers.v == 4) || (gridNumbers.h == 4 && gridNumbers.v == 2) || (gridNumbers.h == 5 && gridNumbers.v == 1) || (gridNumbers.h == 3 && gridNumbers.v == 3))
                        {
                            dr++;
                        }
                    }
                }
            }
        }
        if(hIndex == 4)
        {
            hIndex *= 5;
        }
        else if(hIndex == 3)
        {
            hIndex *= 2;
        }
        if(vIndex == 4)
        {
            vIndex *= 5;
            vIndex++;
        }
        else if (vIndex == 3)
        {
            vIndex *= 2;
            vIndex++;
        }
        if (dl == 4)
        {
            dl *= 5;
        }
        else if (dl == 3)
        {
            dl *= 2;
        }
        if (dr == 4)
        {
            dr *= 5;
        }
        else if (dr == 3)
        {
            dr *= 2;
        }
        return hIndex + vIndex + dl + dr;
    }
}
