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
    public GameObject balanceBorder;

 
    private GridCheck gridCheck;
    private PlayerData playerData;


    private void Awake()
    {
        gridCheck = GetComponent<GridCheck>();
        playerData = GetComponent<PlayerData>();

    }

    public void Collect()
    {
        if (!gridCheck.slingoAnimationFinished || (spinScript.isSpinning && gridCheck.starsCount != 25) || (spinScript.wildPicks != 0 && gridCheck.slingoCount != 12) || spinScript.spinLeft == 10)
        {
            return;
        }
        //Reset values in spin
        spinScript.spinCountHeader.text = "SPINS";
        spinScript.spinLeft = 10;
        spinScript.GetComponent<PanelEffects>().spinLeftText = null;
        spinScript.spinLeftText.color = Color.white;
        spinsCounter.text = spinScript.spinLeft.ToString();
        spinScript.spinBuyLimit = 5;
        spinScript.stakes = 0;
        spinScript.spentText.text = "Satsning: " + UIManager.Instance.DisplayMoney(0);
        spinScript.textToGoEmpty.Clear();
        spinScript.wildPicks = 0;
        spinScript.spinButton.GetComponent<Image>().color = Color.black;
        spinScript.spinButton.GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
        spinScript.spinButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        spinScript.spinButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Spil";
        spinScript.resetButton.color = Color.gray;
        spinScript.resetButton.GetComponentInParent<Button>().enabled = false;
        spinScript.ColorReset();

        foreach (var spinSlot in spinScript.slotsList)
        {
            spinSlot.GetComponentInChildren<Image>().color = Color.white;
            spinSlot.GetComponentInChildren<Image>().enabled = false;
            spinSlot.GetComponentInChildren<TextMeshProUGUI>().text = "?";
        }

        if(gridCheck.slingoCount >= 3)
        {
            playerData.balance += gridCheck.rewards[gridCheck.slingoCount];
            balanceBorder.GetComponent<Animator>().SetBool("BalanceIncreased", true);
            

        }
        
        gridCheck.resetButton.GetComponentInChildren<TextMeshProUGUI>().text = "Nyt Spil";
        collectBorderMessage.SetActive(false);
        ResetTime();
        GetComponent<GridGeneration>().ReGenerateGrid();
       
    }

    public void ResetTime()
    {
        invokeTime = 0;
    }

    private void Update()
    {

    }

    public void stopAni()
    {
        balanceBorder.GetComponent<Animator>().SetBool("BalanceIncreased", false);
    }
}
