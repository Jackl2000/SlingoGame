using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardGameManager : MonoBehaviour
{
    CardGameManager gameManager;

    public Sprite defualtCard;
    public Sprite goodHit;
    public Sprite badHit;
    
    public List<GameObject> cardGOs = new List<GameObject>();
    private List<TextMeshProUGUI> cardTexts = new List<TextMeshProUGUI>();

    private int rndCard;


    private void Awake()
    {
        gameManager = this.GetComponent<CardGameManager>();

        foreach (GameObject goCard in cardGOs)
        {
            cardTexts.Add(goCard.GetComponentInChildren<TextMeshProUGUI>());
        }

    }

    public void ShuffleCards()
    {
        List<int> usedNumb = new List<int>();

        foreach (TextMeshProUGUI cardText in cardTexts)
        {
            rndCard = UnityEngine.Random.Range(1, 10);
            if (!usedNumb.Contains(rndCard))
            {
                usedNumb.Add(rndCard);
                cardText.text = rndCard.ToString();
            }
            else
            {
                while (usedNumb.Contains(rndCard))
                {
                    rndCard = UnityEngine.Random.Range(1, 10);
                    if (!usedNumb.Contains(rndCard))
                    {
                        usedNumb.Add(rndCard);
                        cardText.text = rndCard.ToString();
                        break;
                    }
                }
            }

        }
        usedNumb.Clear();
    }

    public void FlipCard(GameObject cardObj)
    {
        TextMeshProUGUI cardNumberText = cardObj.GetComponentInChildren<TextMeshProUGUI>();

        switch (Convert.ToInt32(cardNumberText.text))
        {
            case 1:
                cardObj.GetComponent<Image>().color = Color.black;
                cardNumberText.enabled = true;
                cardNumberText.text = "(X o X)";
                break;
            case 2:
                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x2";
                break;
            case 3:
                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x5";
                break;
            case 4:
                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x5";
                break;
            case 5:
                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x10";
                break;
            case 6:
                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x20";
                break;
            case 7:
                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x30";
                break;
            case 8:
                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x50";
                break;
            case 9:
                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x100";
                break;
        }
    }

}

