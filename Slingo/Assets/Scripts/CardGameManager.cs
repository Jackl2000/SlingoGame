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
    public Sprite GoodHit;
    public Sprite BadHit;
    
    public List<GameObject> cardGOs = new List<GameObject>();
    List<TextMeshProUGUI> cardTexts = new List<TextMeshProUGUI>();

    private void Awake()
    {
        gameManager = this.GetComponent<CardGameManager>();

        foreach (GameObject goCard in cardGOs)
        {
            cardTexts.Add(goCard.GetComponentInChildren<TextMeshProUGUI>());
        }

    }

    private void Start()
    {
        ShuffleCards();
    }

    int rndCard;

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

    public void FlipCard(GameObject CardToFlip)
    {
        CardToFlip.GetComponent<Image>().sprite = GoodHit;
        //switch (rndCard)
        //{
        //    case 1:
        //        this.GetComponent<Image>().sprite = GoodHit;
        //        break;
        //    case 2:
        //        break;
        //    case 3:
        //        break;
        //    case 4:
        //        break;
        //    case 5:

        //        break;
        //    case 6:
        //        break;
        //    case 7:
        //        break;
        //    case 8:
        //        break;
        //    case 9:
        //        break;

        //}
    }

}

