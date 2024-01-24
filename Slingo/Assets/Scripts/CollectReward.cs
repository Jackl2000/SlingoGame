using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CollectReward : MonoBehaviour
{
    [Header("References")]
    public spin spinScript;
    [SerializeField] private TextMeshProUGUI spinsCounter;

    [Header("Collect message settings")]
    public GameObject collectBorderMessage;
    public TextMeshProUGUI collectMessage;
    public float timeInterval = 10f;
    public float invokeTime = 5;

    private GridCheck gridCheck;
    private PlayerData playerData;

    private void Awake()
    {
        gridCheck = GetComponent<GridCheck>();
        playerData = GetComponent<PlayerData>();        
    }

    public void Collect()
    {
        //Reset values in spin
        spinScript.spinCountHeader.text = "SPINS";
        spinsCounter.text = "8";
        spinScript.spinLeft = 8;
        spinScript.stakes = 0;
        spinScript.spentText.text = "0";

        spinScript.wildPicks = 0;
        foreach (var spinSlot in spinScript.slotsList)
        {
            spinSlot.GetComponentInChildren<Image>().color = Color.white;
            spinSlot.GetComponentInChildren<Image>().enabled = false;
            spinSlot.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            spinSlot.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }

        if (gridCheck.slingoCount >= 3)
        {
            playerData.balance += gridCheck.rewards[gridCheck.slingoCount];

            
            gridCheck.resetButton.GetComponentInChildren<TextMeshProUGUI>().text = "Reset";

            foreach (GameObject go in spinScript.slotsList)
            {
                go.GetComponentInChildren<Image>(true).enabled = false;
            }
            foreach (var slotText in spinScript.slotsList)
            {
                slotText.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            }
            collectBorderMessage.SetActive(false);
            ResetTime();
        }
    }

    private void CollectRewardPopMsg()
    {
        if (gridCheck.slingoCount >= 3 && gridCheck.slingoCount <= 9)
        {
            collectMessage.text = "Collect your reward on " + gridCheck.rewards[gridCheck.slingoCount] + "kr or continue";
            collectBorderMessage.SetActive(true);
        }
    }

    public void ResetTime()
    {
        invokeTime = 0;
    }

    private void Update()
    {
        if (gridCheck.slingoCount >= 3 && spinScript.wildPicks <= 0 && spinScript.spinLeft <= 0)
        {

            invokeTime += Time.deltaTime;

            if (gridCheck.slingoIsHit && invokeTime > 3)
            {
                gridCheck.slingoIsHit = false;
                CollectRewardPopMsg();
            }
            else if (invokeTime >= 13f)
            {
                CollectRewardPopMsg();
            }
        }
    }
}
