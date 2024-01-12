using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectReward : MonoBehaviour
{
    [Header("References")]
    public GridCheck gridCheck;
    public PlayerData playerData;
    public spin spin;
    public GameObject jackpotGO;

    public void Collect()
    {
        playerData.balance--;
        if (gridCheck.slingoCount >= 3)
        {
            playerData.balance += gridCheck.rewards[gridCheck.slingoCount];
            spin.spinLeft = 10;
            spin.spinPrice = 0;
            gridCheck.resetText.text = "Retry";
            foreach (GameObject go in spin.slotsList)
            {
                go.GetComponentInChildren<Image>(true).enabled = false;
            }
            foreach (var slotText in spin.slotsList)
            {
                slotText.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            }
        }
    }

}
