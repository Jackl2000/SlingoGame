using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject SettingsMenuPanel;
    public TextMeshProUGUI soundText;

    public void ViewSettingsPanel()
    {
        SettingsMenuPanel.SetActive(!SettingsMenuPanel.gameObject.activeSelf);
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
