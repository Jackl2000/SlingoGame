using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CollectReward : MonoBehaviour
{
    [Header("References")]
    public GridCheck gridCheck;
    public PlayerData playerData;
    public spin spin;

    [Header("Collect message settings")]
    public GameObject collectBorderMessage;
    public TextMeshProUGUI collectMessage;
    public float timeInterval = 10f;

    public void Collect()
    {
        playerData.balance--;
        if (gridCheck.slingoCount >= 3)
        {
            playerData.balance += gridCheck.rewards[gridCheck.slingoCount];
            spin.spinLeft = 8;
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

    void StartMessageCoroutine()
    {
        if (gridCheck.slingoCount >= 3 && gridCheck.slingoCount <= 9)
        {
            collectMessage.text = "Collect your reward on " + gridCheck.rewards[gridCheck.slingoCount] + "kr or continue";
            collectBorderMessage.SetActive(true); ;
        }
    }

    private void Start()
    {
        InvokeRepeating("StartMessageCoroutine", 1, timeInterval);
    }




}
