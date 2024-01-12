using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectReward : MonoBehaviour
{
    [Header("References")]
    public GridCheck gridCheck;
    public PlayerData playerData;
    public spin spin;
    public GameObject jackpotGO;

    public void Collect()
    {
        if (gridCheck.slingoCount >= 3)
        {
            playerData.balance += gridCheck.rewards[gridCheck.slingoCount];
            spin.spinLeft = 10;
            spin.spinPrice = 0;
            gridCheck.resetText.text = "Retry";
            foreach (var slotText in spin.slotTextList)
            {
                slotText.text = "?";
            }
        }
    }

}
