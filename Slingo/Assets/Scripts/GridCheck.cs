using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class GridCheck : MonoBehaviour
{
    public TextMeshProUGUI resetText;

    private GridGeneration grid;
    private Dictionary<string, bool> gridSlingoList = new Dictionary<string, bool>();
    private Image[] slingoBorders;
    [HideInInspector] public int slingoCount = 0;
    [HideInInspector] public Dictionary<int, float> rewards = new Dictionary<int, float>();

    [SerializeField] private GameObject slingoPanel;
    public GameObject jackpotMessage;    
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

        slingoBorders = slingoPanel.GetComponentsInChildren<Image>().Skip(1).ToArray();
    }



    private void AddingRewards(float multiplyere)
    {
        rewards.Add(3, 10 * multiplyere);
        rewards.Add(4, 20 * multiplyere);
        rewards.Add(5, 40 * multiplyere);
        rewards.Add(6, 100 * multiplyere);
        rewards.Add(7, 250 * multiplyere);
        rewards.Add(8, 750 * multiplyere);
        rewards.Add(9, 2250 * multiplyere);
        rewards.Add(10, 5000 * multiplyere);
        rewards.Add(12, 10000 * multiplyere);
    }

    public void ResetGrid()
    {
        //Adding to balance
        if(rewards.ContainsKey(slingoCount))
        {
            float reward = rewards[slingoCount];
            float balance = GetComponentInChildren<spin>().playerData.balance + reward;
            GetComponentInChildren<spin>().playerData.balance = balance;
        }

        foreach(string item in gridSlingoList.Keys.ToList())
        {
            gridSlingoList[item] = false;
        }
        slingoCount = 0;

        foreach(Image item in slingoBorders)
        {
            if (item.color != Color.black)
            {
                item.color = Color.black;
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
                CheckForReward();
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
                CheckForReward();
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
                            CheckForReward();
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
                            CheckForReward();
                            break;
                        }
                    }
                }
            }
        }
    }

    private void CheckForReward()
    {
        if(rewards.ContainsKey(slingoCount))
        {
            foreach (Image item in slingoBorders)
            {
                if (item.color != Color.green)
                {
                    item.color = Color.green;
                    break;
                }
            }
        }
        if (slingoCount >= 3)
        {
            resetText.text = "Collect";
        }
        if (slingoCount == 12)
        {
            jackpotMessage.SetActive(true);
        }
    }
}

