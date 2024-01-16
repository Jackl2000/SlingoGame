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
    public TextMeshProUGUI collectText;
    public Image retryButtonImg;

    [HideInInspector] public int slingoCount = 0;
    [HideInInspector] public int starsCount = 0;
    [HideInInspector] public Dictionary<int, float> rewards = new Dictionary<int, float>();

    [Space(5)]
    [SerializeField] private GameObject slingoPanel;
    [SerializeField] private Sprite[] slingoBorderImages;
    [SerializeField] private Sprite[] starImages;
    [Space(5)]
    public GameObject jackpotMessage;
    [SerializeField] private Sprite[] jackpotSlingoBorderImages;

    private GridGeneration grid;
    private Dictionary<string, bool> gridSlingoList = new Dictionary<string, bool>();
    [HideInInspector]public bool slingoIsHit = false;
    private Image[] slingoBorders;
    private int rewardCount;

    // Start is called before the first frame update
    void Awake()
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

        slingoBorders = slingoPanel.GetComponentsInChildren<Image>().SkipLast(1).ToArray();
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
        rewards.Add(11, 5000 * multiplyere);
        rewards.Add(12, 10000 * multiplyere);
    }

    public void ResetGrid()
    {
        foreach(string item in gridSlingoList.Keys.ToList())
        {
            gridSlingoList[item] = false;
        }
        slingoCount = 0;
        starsCount = 0;

        foreach(Image item in slingoBorders)
        {
            if (item.sprite != slingoBorderImages[0] && item != slingoBorders[slingoBorders.Length - 1])
            {
                item.sprite = slingoBorderImages[0];
            }
            else if(item == slingoBorders[slingoBorders.Length - 1])
            {
                item.sprite = jackpotSlingoBorderImages[0];
            }
            else break;
        }
    }

    public void CheckGrid(int h, int v, bool diagonal, bool check)
    {
        rewardCount = 0;

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

                if (horIndex == 5 && !gridSlingoList["h" + h])
                {
                    if(check)
                    {
                        rewardCount++;
                        break;
                    }
                    gridSlingoList["h" + h] = true;
                    slingoIsHit = true;
                    slingoCount++;
                    
                    CheckForReward();
                    break;
                }
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

                if (vertIndex == 5 && !gridSlingoList["v" + v])
                {
                    if (check)
                    {
                        rewardCount++;
                        break;
                    }
                    gridSlingoList["v" + v] = true;
                    slingoIsHit = true;
                    slingoCount++;
                    CheckForReward();
                    break;
                }
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
                            if (check)
                            {
                                rewardCount++;
                                break;
                            }
                            gridSlingoList["dl"] = true;
                            slingoIsHit = true;
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
                            if (check)
                            {
                                rewardCount++;
                                break;
                            }
                            gridSlingoList["dr"] = true;
                            slingoIsHit = true;
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
        if (rewards.ContainsKey(slingoCount))
        {
            if(slingoCount == 12)
            {
                slingoBorders[slingoBorders.Length - 1].sprite = jackpotSlingoBorderImages[1];
            }

            foreach (Image item in slingoBorders)
            {
                if (item.sprite != slingoBorderImages[1] && item != slingoBorders[slingoBorders.Length - 1])
                {
                    item.sprite = slingoBorderImages[1];
                    break;
                }
            }
        }
        if (slingoCount >= 3)
        {
            //retry button image to black
            retryButtonImg.color = Color.black;
            collectText.text = "Collect " + rewards[slingoCount].ToString() + "kr";
            collectText.gameObject.SetActive(true);
        }
        if (slingoCount == 12)
        {
            jackpotMessage.SetActive(true);
        }
    }

    public int CheckForMaxReward()
    {
        int maxReward = 0;
        foreach (GridNumbers item in grid.numberPositions.Values)
        {
            if(!item.hasBeenHit)
            {
                item.hasBeenHit = true;
                CheckGrid(item.h, item.v, item.diagonal, true);
                item.hasBeenHit = false;
                if (rewardCount > maxReward)
                {
                    maxReward = rewardCount;
                }
            }
        }
        return maxReward;
    }
}

