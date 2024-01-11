using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }
    }

    public string DisplayMoney(float money)
    {
        return money.ToString("n2") + " KR";
    }
}
