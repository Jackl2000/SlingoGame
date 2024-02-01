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
    public float invokeTime = 5;

    private GridCheck gridCheck;
    private PlayerData playerData;

    private void Awake()
    {
        gridCheck = GetComponent<GridCheck>();
        playerData = GetComponent<PlayerData>();        
    }

    private void Start()
    {
        
    }

    public void Collect()
    {
        if (!gridCheck.slingoAnimationFinished || spinScript.isSpinning || (spinScript.wildPicks != 0 && gridCheck.slingoCount != 12))
        {
            return;
        }
        //Reset values in spin
        spinScript.spinCountHeader.text = "SPINS";
        spinScript.spinLeft = 10;
        spinsCounter.text = spinScript.spinLeft.ToString();
        spinScript.spinBuyLimit = 8;
        spinScript.stakes = 0;
        spinScript.spentText.text = "Stakes: " + UIManager.Instance.DisplayMoney(0);
        spinScript.textToGoEmpty.Clear();
        spinScript.wildPicks = 0;
        spinScript.spinButton.GetComponent<Image>().color = Color.black;
        spinScript.spinButton.GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
        spinScript.spinButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Game";
        spinScript.ColorReset();

        foreach (var spinSlot in spinScript.slotsList)
        {
            spinSlot.GetComponentInChildren<Image>().color = Color.white;
            spinSlot.GetComponentInChildren<Image>().enabled = false;
            spinSlot.GetComponentInChildren<TextMeshProUGUI>().text = "?";
        }

        playerData.balance += gridCheck.rewards[gridCheck.slingoCount];
        gridCheck.resetButton.GetComponentInChildren<TextMeshProUGUI>().text = "Reset";
        collectBorderMessage.SetActive(false);
        ResetTime();
    }

    public void ResetTime()
    {
        invokeTime = 0;
    }

    private void Update()
    {

    }
}
