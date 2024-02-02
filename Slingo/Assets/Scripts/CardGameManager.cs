using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardGameManager : MonoBehaviour
{

    CardGameManager gameManager;

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
        List<int> rolledNumb = new List<int>();

        foreach (TextMeshProUGUI cardText in cardTexts)
        {
            for (int i = 0; i < 9;)
            {
                rndCard = UnityEngine.Random.Range(0, 10);
                if (!rolledNumb.Contains(rndCard))
                {
                    rolledNumb.Add(rndCard);
                    cardText.text = rndCard.ToString();
                    i++;
                }
            }
        }
        rolledNumb.Clear();
    }

    public void FlipCard()
    {
        if (true)
        {
            this.GetComponent<Image>().sprite = GoodHit;
        }
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

