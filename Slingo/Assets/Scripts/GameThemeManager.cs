using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameThemeManager : MonoBehaviour
{
    #region Reference for the assets to replace
    public Image Background;
    public spin spin;

    #endregion

    public GameObject themesPanel;

    public List<GameTheme> themes;

    private GameTheme currentTheme;

    private void Awake()
    {
        currentTheme = themes[0];
    }
    public void ViewThemes()
    {
        if (!Application.isEditor) return;
        themesPanel.SetActive(!themesPanel.activeSelf);
    }

    public void SetGameTheme(int index)
    {
        if (currentTheme.Theme == themes[index].Theme) return;
        Background.sprite = themes[index].BackGroundImage;
        List<GameObject> green = LoadStars().greenStars;
        foreach(GameObject go in green)
        {
            go.GetComponent<Image>().sprite = themes[index].StarImage;
        }
        List<GameObject> yellow = LoadStars().yellowStars;
        foreach (GameObject go in yellow)
        {
            go.GetComponent<Image>().sprite = themes[index].SlingoStarImage;
        }
        spin.wildsImages[0] = themes[index].SuperWildImage;
        spin.wildsImages[1] = themes[index].LineWildImage;
        spin.BackgroundImages[0] = themes[index].BestChoiceBackgroundLineWildImage;
        spin.BackgroundImages[1] = themes[index].GridBackgroundSuperWildImage;
        spin.BackgroundImages[2] = themes[index].BestChoiceBackgroundSuperWildImage;
        spin.BackgroundImages[3] = themes[index].GridBackgroundLineWildImage;

        currentTheme = themes[index];
        themesPanel.SetActive(false);
    }

    private (List<GameObject> yellowStars, List<GameObject> greenStars) LoadStars()
    {
        
        List<GameObject> yellow = new List<GameObject>();
        List<GameObject> green = new List<GameObject>();

        GameObject[] stars = GameObject.FindGameObjectsWithTag("GridImage");
        foreach(GameObject star in stars)
        {
            yellow.Add(star.transform.GetChild(1).gameObject);
            green.Add(star.transform.GetChild(0).gameObject);
        }
        return (yellow, green);

    }

}

[Serializable]
public class GameTheme
{
    public string Theme;
    public Sprite BackGroundImage;
    public Sprite StarImage;
    public Sprite SlingoStarImage;
    public Sprite SuperWildImage;
    public Sprite LineWildImage;
    public Sprite GridBackgroundSuperWildImage;
    public Sprite GridBackgroundLineWildImage;
    public Sprite BestChoiceBackgroundSuperWildImage;
    public Sprite BestChoiceBackgroundLineWildImage;

    public GameTheme(Sprite Background, Sprite Star, Sprite Slingo, Sprite SuperWild, Sprite LineWild, Sprite GridBackgroundsSuperWild, Sprite GridBackgroundLineWild, Sprite BestChoiceBackgroundSuperWild, Sprite BestChoiceBackgroundLineWild)
    {
        BackGroundImage = Background;
        StarImage = Star;
        SlingoStarImage = Slingo;
        SuperWildImage = SuperWild;
        LineWildImage = LineWild;
        GridBackgroundSuperWildImage = GridBackgroundsSuperWild;
        GridBackgroundLineWildImage = GridBackgroundLineWild;
        BestChoiceBackgroundSuperWildImage = BestChoiceBackgroundSuperWild;
        BestChoiceBackgroundLineWildImage = BestChoiceBackgroundLineWild;
    }
}
