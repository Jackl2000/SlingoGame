using System;
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


        if (gridCheck.slingoCount >= 10 && GameManager.Instance.BonusGameEnable)
        {
            spinScript.collectMessageText.transform.parent.parent.gameObject.SetActive(true); //CollectMessage
            spinScript.collectMessageText.text = "BONUS SPIL OPN�ET !" + "\n" + "Spil videre for at vinde ekstra";
            spinScript.collectMessageText.transform.parent.GetChild(0).gameObject.SetActive(false);
            spinScript.collectMessageText.transform.parent.GetChild(1).gameObject.SetActive(true);
            spinScript.collectMessageText.transform.parent.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            //Reset values in spin
            spinScript.spinLeftText.text = spinScript.spinLeft.ToString();
            spinScript.textToGoEmpty.Clear();
            spinScript.spinCountHeader.text = "SPINS";
            spinScript.spinLeft = spinScript.startSpins;
            spinScript.GetComponent<PanelEffects>().spinLeftText = null;
            spinScript.indsatsChoosen = false;
            spinScript.spinLeftText.color = Color.white;
            spinScript.spinLeftText.text = spinScript.spinLeft.ToString();
            PlayerData.Instance.totalIndsats = 0;
            spinScript.spentText.text = "Indsat: " + UIManager.Instance.DisplayMoney(0);
            spinScript.stakes = 0;
            spinScript.spinBuyLimit = 5;
            spinScript.wildPicks = 0;
            spinScript.isSpinning = false;

            spinScript.spinButton.GetComponent<Image>().color = Color.black;
            spinScript.spinButton.GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
            spinScript.spinButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            spinScript.spinButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Spil";

            spinScript.resetButtonText.GetComponentInParent<Button>().enabled = false;
            spinScript.resetButtonText.text = "Nyt Spil";

            spinScript.MessageAnimator.SetBool("MinimizePlate", false);
            spinScript.collectMessageText.text = "SPIL SLUT";
            spinScript.ColorReset();

            spinScript.CollectMessage.SetActive(false);

            foreach (var spinSlot in spinScript.slotsList)
            {
                spinSlot.transform.GetChild(1).GetComponent<Image>().color = Color.white;
                spinSlot.transform.GetChild(1).GetComponent<Image>().enabled = false;
                spinSlot.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            }

            if (gridCheck.slingoCount >= 3)
            {
                PlayerData.Instance.balance += gridCheck.rewards[gridCheck.slingoCount];
                balanceBorder.GetComponent<Animator>().SetBool("BalanceIncreased", true);
            }

            GetComponent<GridGeneration>().ReGenerateGrid();
        }
    }

    public void stopAni()
    {
        balanceBorder.GetComponent<Animator>().SetBool("BalanceIncreased", false);
    }
}
