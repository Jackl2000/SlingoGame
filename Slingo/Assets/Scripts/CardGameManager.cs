using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardGameManager : MonoBehaviour
{
    [Header("References")]
    public PlayerData playerData;
    private CardGameManager gameManager;
    private Animator animator;

    [Space(5)]
    public GameObject LostPrefab;

    [Space(5)]
    public TextMeshProUGUI rewardText;

    [Space(5)]
    [Header("Card sprite")]
    public Sprite defualtCard;
    public Sprite goodHit;
    public Sprite badHit;
    
    public List<GameObject> cardGo = new List<GameObject>();
    private List<TextMeshProUGUI> cardTexts = new List<TextMeshProUGUI>();
    private List<Image> cardImages = new List<Image>();

    private int rndCard;
    private int gevints;
    private GameObject LostGameObject;

    private void Awake()
    {
        gameManager = this.GetComponent<CardGameManager>();
        animator = this.GetComponent<Animator>();
        foreach (GameObject goCard in cardGo)
        {
            cardTexts.Add(goCard.GetComponentInChildren<TextMeshProUGUI>());
            cardImages.Add(goCard.GetComponent<Image>());
        }
    }

    private void Start()
    {
        ShuffleCards();
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

    public void NewGame()
    {
        foreach (Image cardImage in cardImages)
        {
            cardImage.sprite = defualtCard;
            cardImage.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            cardImage.color = Color.white;
            cardImage.gameObject.GetComponent<Button>().enabled = true;

        }
        LostGameObject.SetActive(false);
        animator.SetBool("isBadFlip", false);
        
        gevints = 0;
        rewardText.text = "Gevints: " + gevints.ToString();

        ShuffleCards();
    }

    public void FlipCard(GameObject cardObj)
    {
        TextMeshProUGUI cardNumberText = cardObj.GetComponentInChildren<TextMeshProUGUI>();
        

        switch (Convert.ToInt32(cardNumberText.text))
        {
            case 1:
                foreach (GameObject card in cardGo)
                {
                    card.GetComponent<Button>().enabled = false;
                }
                cardObj.GetComponent<Image>().color = Color.black;
                LostGameObject = cardObj.gameObject.transform.GetChild(1).gameObject;
                LostGameObject.SetActive(true);
                animator.SetBool("isBadFlip", true);

                break;
            case 2:
                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x2";
                
                gevints += Convert.ToInt32(cardNumberText.text.Substring(1));
                rewardText.text = "Gevints: " + gevints + "kr";

                break;
            case 3:
                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x5";
                
                gevints += Convert.ToInt32(cardNumberText.text.Substring(1));
                rewardText.text = "Gevints: " + gevints + "kr";

                break;
            case 4:
                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x5";

                gevints += Convert.ToInt32(cardNumberText.text.Substring(1));
                rewardText.text = "Gevints: " + gevints + "kr";

                break;
            case 5:
                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x10";
                
                gevints += Convert.ToInt32(cardNumberText.text.Substring(1));
                rewardText.text = "Gevints: " + gevints + "kr";

                break;
            case 6:
                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x20";
                
                gevints += Convert.ToInt32(cardNumberText.text.Substring(1));
                rewardText.text = "Gevints: " + gevints + "kr";

                break;
            case 7:
                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x30";
                
                gevints += Convert.ToInt32(cardNumberText.text.Substring(1));
                rewardText.text = "Gevints: " + gevints + "kr";

                break;
            case 8:
                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x50";
                
                gevints += Convert.ToInt32(cardNumberText.text.Substring(1));
                rewardText.text = "Gevints: " + gevints + "kr";

                break;
            case 9:
                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x100";
                
                gevints += Convert.ToInt32(cardNumberText.text.Substring(1));
                rewardText.text = "Gevints: " + gevints + "kr";

                break;
        }
    }


 
}

