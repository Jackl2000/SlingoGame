using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameThemeManager : MonoBehaviour
{
    #region Reference to images in prefab
    public spin spin;
    public Image BackgroundImage;
    #endregion

    public Image placeholder;

    private List<GameTheme> gameThemes;
    // Start is called before the first frame update
    void Start()
    {
        gameThemes = new List<GameTheme>();
        //LoadGameThemes();
    }

    private void LoadGameThemes()
    {
        GameTheme defaultTheme = new GameTheme(BackgroundImage.sprite, placeholder.sprite, placeholder.sprite, spin.BackgroundImages[0], placeholder.sprite, spin.BackgroundImages[1], spin.BackgroundImages[2]);
        
        gameThemes.Add(defaultTheme);
    }

    private void SetGameTheme()
    {

    }
}

public class GameTheme
{
    public Sprite BackGroundImage { get; private set; }
    public Sprite StarImage { get; private set; }
    public Sprite SlingoStarImage { get; private set; }
    public Sprite SlingoBackgroundImage { get; private set; }
    public Sprite WildImage { get; private set; }
    public Sprite WildBackgroundImage { get; private set; }
    public Sprite BestChoiceBackgroundImage { get; private set; }

    public GameTheme(Sprite Background, Sprite Star, Sprite Slingo, Sprite SlingoBackground, Sprite Wild, Sprite WildBackground, Sprite BestChoiceBackground)
    {
        BackGroundImage = Background;
        StarImage = Star;
        SlingoStarImage = Slingo;
        SlingoBackgroundImage = SlingoBackground;
        WildImage = Wild;
        WildBackgroundImage = WildBackground;
        BestChoiceBackgroundImage = BestChoiceBackground;
    }
}
