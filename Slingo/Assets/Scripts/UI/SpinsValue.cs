using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpinsValue : MonoBehaviour
{
    public GameObject spinsBetPanel;
    public GameObject settingsPanel;
    public GameObject PrisButton;

    private spin spin;
    private SettingsMenu settingsMenu;
    private List<float> spinValues = new List<float>() {0.5f, 1f, 2f, 5f, 10f, 25 };
    private int currentIndex = 2;
    public TextMeshProUGUI valueText;




    private void Awake()
    {
        spin = GetComponent<spin>();
        settingsMenu = FindAnyObjectByType<SettingsMenu>();
        UpdateValueText();

    }

    public void ViewSpinsBets()
    {
        spinsBetPanel.SetActive(!spinsBetPanel.activeSelf);
        settingsPanel.SetActive(false);
       
    }

    public void IncreaseIndex()
    {
        currentIndex++;
        if(currentIndex >= spinValues.Count) 
        { 
            currentIndex = 0; //Loop back
        }
        UpdateValueText();
    }

    public void DecreaseIndex()
    {
        currentIndex--;
        if(currentIndex < 0)
        {
            currentIndex = 0;
        }
        UpdateValueText();
    }

    public void UpdateValueText()
    {
       if(valueText != null && currentIndex >= 0 && currentIndex < spinValues.Count) 
       { 
            valueText.text = spinValues[currentIndex].ToString("0.0") + " kr";
       }
    }


    public void SpinBetChosen()
    {
       if(currentIndex >= 0 && currentIndex < spinValues.Count)
       {
            float selectedBet = spinValues[currentIndex];
            spin.spinBets = selectedBet;
            PlayerData.Instance.bet = selectedBet;
            valueText.text = UIManager.Instance.DisplayMoney(selectedBet);
        }
    }

    public void StartGame()
    {
        SpinBetChosen();
        spinsBetPanel.SetActive(false);
        settingsMenu.multiplier = PlayerData.Instance.bet;
        GetComponentInParent<GridCheck>().UpdateRewards(null, PlayerData.Instance.bet);
        PrisButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Vælg pris";
        spin.StartSpin();
    }
}
