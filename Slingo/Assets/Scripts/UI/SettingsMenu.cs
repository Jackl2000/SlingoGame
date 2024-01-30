using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenuPanel;
    public TextMeshProUGUI soundText;
    public GameObject spinsBetPanel;
    public Button RewardButton;
    public GameObject PossibleRewards;
    public TextMeshProUGUI JackpotText;
    public TextMeshProUGUI SuperJackpotText;
    public float multiplier = 1; 

    private GridCheck gridCheck;

    public void Start()
    {
        RewardButton.onClick.AddListener(ShowPanelRewards);

        gridCheck = FindAnyObjectByType <GridCheck>();

    }


    public void ViewSettingsPanel()
    {
        if (PossibleRewards == true)
        {
            PossibleRewards.SetActive(false);
        }


        settingsMenuPanel.SetActive(!settingsMenuPanel.activeSelf);
        spinsBetPanel.SetActive(false);

    }

    public void Sound()
    {
        if(soundText.text == "Sounds: On")
        {
            soundText.text = "Sounds: Off";
        }
        else
        {
            soundText.text = "Sounds: On";
        }
    }

    public void ShowPanelRewards()
    {

        PossibleRewards.SetActive(true);
        if (gridCheck != null)
        {
            //Standard price is 1
            gridCheck.UpdateRewards(multiplier);

            if(gridCheck.rewards.Count > 9) 
            {
                float jackpotReward = gridCheck.rewards[11];
                float superJackpotReward = gridCheck.rewards[12];

                JackpotText.text = $"{jackpotReward}";
                SuperJackpotText.text = $"{superJackpotReward}";

            }


        }

    }

    public void Info()
    {
        Debug.Log("View info");
    }

    public void History()
    {
        Debug.Log("View history");
    }
}
