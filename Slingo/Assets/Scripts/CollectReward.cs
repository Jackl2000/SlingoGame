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


    private void Awake()
    {
        gridCheck = GetComponent<GridCheck>();
    } 

    public void Collect()
    {
        if (!gridCheck.slingoAnimationFinished || (spinScript.isSpinning && gridCheck.starsCount != 25) || (spinScript.wildPicks != 0 && gridCheck.slingoCount != 12) || spinScript.spinLeft == spinScript.startSpins)
        {
            return;
        }
        //Reset values in spin
        spinsCounter.text = spinScript.spinLeft.ToString();
        spinScript.textToGoEmpty.Clear();
        spinScript.spinCountHeader.text = "SPINS";
        spinScript.spinLeft = spinScript.startSpins;
        spinScript.GetComponent<PanelEffects>().spinLeftText = null;
        spinScript.indsatsChoosen = false;
        spinScript.spinLeftText.color = Color.white;
        spinScript.spinLeftText.text = spinScript.spinLeft.ToString();
        spinScript.spentText.text = "Satsning: " + UIManager.Instance.DisplayMoney(0);
        spinScript.stakes = 0;
        spinScript.spinBuyLimit = 5;
        spinScript.wildPicks = 0;

        spinScript.spinButton.GetComponent<Image>().color = Color.black;
        spinScript.spinButton.GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
        spinScript.spinButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        spinScript.spinButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Spil";

        spinScript.resetButton.color = Color.gray;
        spinScript.resetButton.GetComponentInParent<Button>().enabled = false;
        spinScript.resetButton.GetComponentInChildren<TextMeshProUGUI>().text = "Nyt Spil";
        
        spinScript.MessageAnimator.SetBool("MinimizePlate", false);
        spinScript.ColorReset();

        foreach (var spinSlot in spinScript.slotsList)
        {
            spinSlot.GetComponentInChildren<Image>().color = Color.white;
            spinSlot.GetComponentInChildren<Image>().enabled = false;
            spinSlot.GetComponentInChildren<TextMeshProUGUI>().text = "?";
        }

        if(gridCheck.slingoCount >= 3)
        {
            PlayerData.Instance.balance += gridCheck.rewards[gridCheck.slingoCount];
            balanceBorder.GetComponent<Animator>().SetBool("BalanceIncreased", true);
            

        }
        
        collectBorderMessage.SetActive(false);
        ResetTime();
        GetComponent<GridGeneration>().ReGenerateGrid();
       
    }

    public void ResetTime()
    {
        invokeTime = 0;
    }

    public void stopAni()
    {
        balanceBorder.GetComponent<Animator>().SetBool("BalanceIncreased", false);
    }
}
