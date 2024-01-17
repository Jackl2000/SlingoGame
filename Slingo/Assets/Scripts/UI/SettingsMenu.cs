using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject SettingsMenuPanel;
    public TextMeshProUGUI soundText;
    public GameObject spinsBetPanel;

    public void ViewSettingsPanel()
    {
        SettingsMenuPanel.SetActive(!SettingsMenuPanel.activeSelf);
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

    public void Info()
    {
        Debug.Log("View info");
    }

    public void History()
    {
        Debug.Log("View history");
    }
}
