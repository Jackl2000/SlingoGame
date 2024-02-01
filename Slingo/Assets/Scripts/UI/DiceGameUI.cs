using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceGameUI : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public Button higherButton;
    public Button lowerButton;
    public TextMeshProUGUI feedBackText;
    public Button throwButton;
    public GameObject textPanel;
    public Button playAgainButton;
    public GameObject youRolledPanel;

    public DiceThrower diceThrower;
    private int playerRollResult;
    private int opponentRollResult;

    // Start is called before the first frame update
    void Start()
    {
        higherButton.onClick.AddListener(() => GuessOutcome(true));
        lowerButton.onClick.AddListener(() => GuessOutcome(false));
        throwButton.onClick.AddListener(Reset);
        higherButton.gameObject.SetActive(false);
        lowerButton.gameObject.SetActive(false);
        feedBackText.gameObject.SetActive(false);
        playAgainButton.onClick.AddListener(HidePanel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HidePanel()
    {
        textPanel.gameObject.SetActive(false);
        throwButton.gameObject.SetActive(true);
    }

    public void ShowPlayerResult(int result)
    {
        playerRollResult = result;
        higherButton.gameObject.SetActive(true);
        lowerButton.gameObject.SetActive(true);
        youRolledPanel.gameObject.SetActive(true);
        feedBackText.gameObject .SetActive(true);
        feedBackText.text = $"Du slog {result}";
    }

    public void ShowOpponentResult(int result)
    {
        opponentRollResult = result;
    }

    private async void GuessOutcome(bool guess)
    {
        // Compare player's roll against opponent
        higherButton.gameObject.SetActive(false);
        lowerButton.gameObject.SetActive(false);
        youRolledPanel.gameObject.SetActive(false);
        feedBackText.text = "";
        int opponentRoll = await RollOpponentDice();
        bool isCorrect = (guess && opponentRoll > playerRollResult) ||
                         (!guess && opponentRoll < playerRollResult) ||
                         (guess && opponentRoll == playerRollResult) ||
                         (!guess && opponentRoll == playerRollResult);

        // Display feedback including the player's roll
        string feedback = isCorrect
            ? $"Korrekt! Du slog {playerRollResult}, din modstander slog {opponentRoll}"
            : $"Forkert! Du slog {playerRollResult}, din modstander slog {opponentRoll}";

        if(playerRollResult == opponentRollResult)
        {
            feedback = $"I Slog det samme, slå om";
        }

        textPanel.gameObject.SetActive(true);

        resultText.text = feedback;
       
    }

    public void Reset()
    {
        //Hide buttons and text
        higherButton.gameObject.SetActive(false);
        lowerButton.gameObject.SetActive(false);
        feedBackText.gameObject.SetActive(false);

        //Clear player's previous roll result
        playerRollResult = 0;

        //Trigger new dice roll
        diceThrower.PlayerRollDice();
        //Hides button while throw is ongoing
        throwButton.gameObject.SetActive(false);
    }

    public async Task<int> RollOpponentDice()
    {

        await diceThrower.oppponentRollDice();
        opponentRollResult = diceThrower.currentValue;

        return opponentRollResult;


    }


}
