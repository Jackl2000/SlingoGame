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
    public float invokeTime = 5;

    public void Collect()
    {
        if (gridCheck.slingoCount >= 3)
        {
            playerData.balance += gridCheck.rewards[gridCheck.slingoCount];
            spin.spinLeft = 8;
            gridCheck.retryButtonImg.color = Color.white;

            foreach (GameObject go in spin.slotsList)
            {
                go.GetComponentInChildren<Image>(true).enabled = false;
            }
            foreach (var slotText in spin.slotsList)
            {
                slotText.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            }
            ResetTime();
        }
    }

    void CollectRewardPopMsg()
    {
        if (gridCheck.slingoCount >= 3 && gridCheck.slingoCount <= 9)
        {
            collectMessage.text = "Collect your reward on " + gridCheck.rewards[gridCheck.slingoCount] + "kr or continue";
            collectBorderMessage.SetActive(true); ;
        }
    }

    public void ResetTime()
    {
        invokeTime = 0;
    }

    private void Update()
    {
        if (gridCheck.slingoCount >= 3 && spin.wCount <= 0)
        {

            invokeTime += Time.deltaTime;

            if (gridCheck.slingoIsHit)
            {
                gridCheck.slingoIsHit = false;
                CollectRewardPopMsg();
            }
            else if (invokeTime >= 7.5f)
            {
                CollectRewardPopMsg();
            }
        }
    }


}
