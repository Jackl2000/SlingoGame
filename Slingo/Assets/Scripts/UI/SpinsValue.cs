using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinsValue : MonoBehaviour
{
    public GameObject SpinsBetPanel;
    private spin spin;
    private GridCheck gridCheck;

    private void Start()
    {
        spin = GetComponent<spin>();
        gridCheck = GetComponentInParent<GridCheck>();
    }

    public void ViewSpinsBets()
    {
        SpinsBetPanel.SetActive(!SpinsBetPanel.gameObject.activeSelf);
    }

    public void SetSpinBets(float bet)
    {
        if(spin.spinLeft == 8)
        {
            spin.spinBets = bet;
            SpinsBetPanel.SetActive(false);
            gridCheck.UpdateRewards(bet);
        }
    }
}
