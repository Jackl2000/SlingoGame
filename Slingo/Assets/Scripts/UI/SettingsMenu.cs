using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private SceneSwap sceneSwap;

    private GridCheck gridCheck;

    public void Start()
    {
        if (RewardButton != null)
        {
            RewardButton.onClick.AddListener(ShowPanelRewards);
        }

        gridCheck = FindAnyObjectByType <GridCheck>();
    }


    public void ViewSettingsPanel()
    {
        if (PossibleRewards == true)
        {
            PossibleRewards.SetActive(false);
        }

        settingsMenuPanel.SetActive(!settingsMenuPanel.activeSelf);
        if(spinsBetPanel != null)
        {
            spinsBetPanel.SetActive(false);
        }

        if(GameManager.Instance.BonusGameEnable)
        {
            int childCount = settingsMenuPanel.transform.childCount;
            settingsMenuPanel.transform.GetChild(childCount - 1).gameObject.SetActive(true);
            settingsMenuPanel.transform.GetChild(childCount - 2).gameObject.SetActive(true);
        }
    }

    public void Sound()
    {
        if(soundText.text == "Lyd: Til")
        {
            soundText.text = "Lyd: Fra";
        }
        else
        {
            soundText.text = "Lyd: Til";
        }
    }

    public void ShowPanelRewards()
    {

        PossibleRewards.SetActive(true);
        if (gridCheck != null)
        {
            //Standard price is 1
            //gridCheck.UpdateRewards(multiplier);

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
        Debug.Log("Se info");
    }

    public void History()
    {
        Debug.Log("Se spilhistorik");
    }

    public void ReturnToMainScene()
    {
        SceneSwap.Instance.SceneSwitch("scene_tai");
    }
}
