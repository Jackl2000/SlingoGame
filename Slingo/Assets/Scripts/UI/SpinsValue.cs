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
    private List<GameObject> spinValues = new List<GameObject>();

    private void Awake()
    {
        spin = GetComponent<spin>();
        settingsMenu = FindAnyObjectByType<SettingsMenu>();
    }

    public void ViewSpinsBets()
    {
        spinsBetPanel.SetActive(!spinsBetPanel.activeSelf);
        settingsPanel.SetActive(false);
        foreach (Outline outline in PrisButton.GetComponentsInChildren<Outline>(true).ToArray())
        {
            spinValues.Add(outline.gameObject);
        }
    }


    public void ViewSpinBetOptions()
    {
        if(spin.spinLeft == spin.startSpins)
        {
            foreach(GameObject go in spinValues)
            {
                go.SetActive(true);
            }
        }
    }

    public void SpinBetChosen(float bet)
    {
        spin.spinBets = bet;
        PlayerData.Instance.bet = bet;
        foreach (GameObject go in spinValues)
        {
            go.SetActive(false);
        }
        PrisButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = UIManager.Instance.DisplayMoney(bet);
    }

    public void StartGame()
    {
        spinsBetPanel.SetActive(false);
        settingsMenu.multiplier = PlayerData.Instance.bet;
        GetComponentInParent<GridCheck>().UpdateRewards(null, PlayerData.Instance.bet);
        PrisButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Vælg pris";
        spin.StartSpin();
    }
}
