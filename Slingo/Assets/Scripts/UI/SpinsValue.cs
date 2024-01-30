using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinsValue : MonoBehaviour
{
    public GameObject spinsBetPanel;
    public GameObject settingsPanel;

    private spin spin;
    private GridCheck gridCheck;
    private SettingsMenu settingsMenu;

    private void Awake()
    {
        spin = GetComponent<spin>();
        gridCheck = GetComponentInParent<GridCheck>();
        settingsMenu = FindAnyObjectByType<SettingsMenu>();
    }

    public void ViewSpinsBets()
    {
        spinsBetPanel.SetActive(!spinsBetPanel.activeSelf);
        settingsPanel.SetActive(false);
    }

    public void SetSpinBets(float bet)
    {
        if(spin.spinLeft == 10)
        {
            spin.spinBets = bet;
            spinsBetPanel.SetActive(false);
            gridCheck.UpdateRewards(bet);

            settingsMenu.multiplier = bet;
        }
    }
}
