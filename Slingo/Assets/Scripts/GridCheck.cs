using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GridCheck : MonoBehaviour
{
    private GridGeneration grid;

    [HideInInspector]public List<float> rewards = new List<float>();
    private Dictionary<string, bool> gridSlingoList = new Dictionary<string, bool>();
    private TextMeshProUGUI[] slingoText;
 
    [SerializeField] private GameObject slingoPanel;
    [HideInInspector]public int slingoCount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<GridGeneration>();
        gridSlingoList.Add("h1", false);
        gridSlingoList.Add("h2", false);
        gridSlingoList.Add("h3", false);
        gridSlingoList.Add("h4", false);
        gridSlingoList.Add("h5", false);
        gridSlingoList.Add("v1", false);
        gridSlingoList.Add("v2", false);
        gridSlingoList.Add("v3", false);
        gridSlingoList.Add("v4", false);
        gridSlingoList.Add("v5", false);
        gridSlingoList.Add("dl", false);
        gridSlingoList.Add("dr", false);
        AddingRewards(1);

        slingoText = slingoPanel.GetComponentsInChildren<TextMeshProUGUI>();
    }

    private void AddingRewards(float multiplyere)
    {
        rewards.Add(0);
        rewards.Add(0 * multiplyere);
        rewards.Add(0.1f * multiplyere);
        rewards.Add(0.5f * multiplyere);
        rewards.Add(1 * multiplyere);
        rewards.Add(3 * multiplyere);
        rewards.Add(5 * multiplyere);
        rewards.Add(20 * multiplyere);
        rewards.Add(50 * multiplyere);
        rewards.Add(150 * multiplyere);
        rewards.Add(300 * multiplyere);
        rewards.Add(500 * multiplyere);
        rewards.Add(1000 * multiplyere);
    }

    public void ResetGrid()
    {
        //Adding to balance
        float reward = rewards[slingoCount];
        float balance = grid.currentBalance + reward;
        grid.currentBalance = balance;

        foreach(string item in gridSlingoList.Keys.ToList())
        {
            gridSlingoList[item] = false;
        }
        slingoCount = 0;

        foreach(TextMeshProUGUI item in slingoText)
        {
            if (item.color != Color.white)
            {
                item.color = Color.white;
            }
            else break;
        }
    }

    public void CheckGrid(int h, int v, bool diagonal)
    {
        int horIndex = 0;
        foreach (GridNumbers number in grid.numberPositions.Values)
        {
            if (number.h == h)
            {
                horIndex++;
                if (!number.hasBeenHit)
                {
                    break;
                }
            }
            if (horIndex == 5 && !gridSlingoList["h" + h])
            {
                gridSlingoList["h" + h] = true;
                slingoCount++;
                SlingoUI();
                break;
            }
        }

        int vertIndex = 0;
        foreach (GridNumbers number in grid.numberPositions.Values)
        {
            if (number.v == v)
            {
                vertIndex++;
                if (!number.hasBeenHit)
                {
                    break;
                }
            }
            if (vertIndex == 5 && !gridSlingoList["v" + v])
            {
                gridSlingoList["v" + v] = true;
                slingoCount++;
                SlingoUI();
                break;
            }
        }


        if (diagonal)
        {
            if(!gridSlingoList["dl"])
            {
                int leftIndex = 0;
                foreach (GridNumbers number in grid.numberPositions.Values)
                {
                    if (number.h == number.v)
                    {
                        leftIndex++;
                        if (!number.hasBeenHit)
                        {
                            break;
                        }
                        if (leftIndex == 5)
                        {
                            gridSlingoList["dl"] = true;
                            slingoCount++;
                            SlingoUI();
                            break;
                        }
                    }
                }
            }

            if (!gridSlingoList["dr"])
            {
                int rightIndex = 0;
                foreach (GridNumbers number in grid.numberPositions.Values)
                {
                    if ((number.h == 5 && number.v == 1) || (number.h == 4 && number.v == 2) || (number.h == 3 && number.v == 3) || (number.h == 2 && number.v == 4) || (number.h == 1 && number.v == 5))
                    {
                        rightIndex++;
                        if (!number.hasBeenHit)
                        {
                            break;
                        }
                        if (rightIndex == 5 && !gridSlingoList["dr"])
                        {
                            gridSlingoList["dr"] = true;
                            slingoCount++;
                            SlingoUI();
                            break;
                        }
                    }
                }
            }
        }
    }

    private void SlingoUI()
    {
        foreach (TextMeshProUGUI item in slingoText)
        {
            if(item.gameObject.name == "Slingo" + slingoCount.ToString())
            {
                item.color = Color.green;
                break;
            }
        }
    }
}
