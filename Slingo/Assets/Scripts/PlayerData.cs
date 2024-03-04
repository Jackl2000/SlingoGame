using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerData
{
    public float balance = 999;
    public float bet = 1;
    public float CombatBonusReward = 0;
    public float CombatBonusIncrementReward = 0;
    public float moneyBagWidth = 60f;
    private static PlayerData instance;
    public static PlayerData Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new PlayerData();
            }
            return instance;
        }
    }
}
