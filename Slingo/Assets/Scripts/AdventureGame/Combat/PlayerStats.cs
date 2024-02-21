using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new PlayerStats();
            }
            return instance;
        }
    }

    public int Damage { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Luck { get; set; }
    public int Level { get; set; } = 1;

    public bool CritAttack { get; set; } = false;
}
