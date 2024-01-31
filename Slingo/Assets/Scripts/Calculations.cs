using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Calculations : MonoBehaviour
{
    private spin spin;
    private GridCheck gridCheck;
    private int possibleRewardAmplifiere;

    private void Start()
    {
        spin = GetComponent<spin>();
        gridCheck = GetComponentInParent<GridCheck>();
        //TestCalculation();
    }
    public float PriceCaculator()
    {
        if (gridCheck.slingoCount == 12)
        {
            return 0;
        }
        possibleRewardAmplifiere = gridCheck.CheckForMaxReward();

        float starMultipliere = 0.015f + (gridCheck.starsCount * 0.015f);
        float slingoReward = starMultipliere * 1.2f;
        if (gridCheck.slingoCount == 0)
        {
            slingoReward = starMultipliere;
        }
        if (gridCheck.rewards.ContainsKey(gridCheck.slingoCount + 1))
        {
            slingoReward = (gridCheck.rewards[Convert.ToInt32(gridCheck.slingoCount) + 1] / spin.spinBets) * Mathf.Clamp(gridCheck.starsCount / gridCheck.slingoCount, 0.6f, 0.95f + starMultipliere) * starMultipliere;
        }

        float maxSlingoAmplifiere = Mathf.Clamp(possibleRewardAmplifiere - 0.5f, 0.5f, 1.8f);
        float price = slingoReward * Mathf.Clamp(maxSlingoAmplifiere, 1, maxSlingoAmplifiere);
        price *= spin.spinBets;
        //spin.spinLeftText.text = UIManager.Instance.DisplayMoney(price);
        return price;
    }

    public void TestCalculation()
    {
        float bet = 1;
        for (float m = 0; m < 3; m++)
        {
            for (float i = 0; i < 11; i++)
            {
                for (float j = 10; j < 25; j++)
                {
                    float multipliere = 0.015f + (j * 0.015f);
                    float slingoRewards = multipliere * 1.2f;
                    if (i == 0)
                    {
                        slingoRewards = multipliere;
                    }
                    if (gridCheck.rewards.ContainsKey(Convert.ToInt32(i) + 1))
                    {
                        slingoRewards = (gridCheck.rewards[Convert.ToInt32(i) + 1] / bet) * Mathf.Clamp(j / i, 0.6f, 0.95f + multipliere) * multipliere;
                    }
                    float maxSlingoAmplifiere = Mathf.Clamp(m - 0.5f, 0.5f, 1.8f);
                    float price = slingoRewards * Mathf.Clamp(maxSlingoAmplifiere, 1, maxSlingoAmplifiere);     
                    price *= bet;
                    Debug.Log("SlingoCount: " + i + " Starscount: " + j + " Multipliere: " + multipliere + " Amplifiere: " + maxSlingoAmplifiere + " final value: " + UIManager.Instance.DisplayMoney(price));
                }
            }
        }
    }
}
