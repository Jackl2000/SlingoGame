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
    public spin spinScript;

    private CardGameManager gameManager;
    private Animator animator;

    [Space(5)]
    public TextMeshProUGUI gevintsText;
    public TextMeshProUGUI indsatsText;
    public TextMeshProUGUI balanceText;

    [Space(5)]
    [Header("Reward Setting")]
    public float multiplier = 0.5f;
    private int rndCard;
    private float gevints;
    private float præmie;
    private GameObject LostGameObject;

    [Space(10)]
    public List<GameObject> cardGameObjects = new List<GameObject>();
    private List<TextMeshProUGUI> cardTexts = new List<TextMeshProUGUI>();
    private List<Image> cardImages = new List<Image>();

    [Space(5)]
    [Header("Card sprite")]
    public Sprite defualtCard;
    public Sprite goodHit;
    public Sprite badHit;

    private string gevintsString;

    private void Awake()
    {
        gameManager = this.GetComponent<CardGameManager>();
        animator = this.GetComponent<Animator>();
        foreach (GameObject goCard in cardGameObjects)
        {
            cardTexts.Add(goCard.GetComponentInChildren<TextMeshProUGUI>());
            cardImages.Add(goCard.GetComponent<Image>());
        }
    }

    private void Start()
    {
        gevintsString = "Bonus gevints: ";

        balanceText.text = "Balance: " + playerData.balance.ToString();
        indsatsText.text = "Indsats: " + spinScript.stakes.ToString();

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
        playerData.balance += gevints;
        balanceText.text = "Balance: " + playerData.balance.ToString();
        gevints = 0;
        gevintsText.text = "Bonus gevints: " + gevints.ToString();

        foreach (Image cardImage in cardImages)
        {
            cardImage.sprite = defualtCard;
            cardImage.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            cardImage.color = Color.white;
            cardImage.gameObject.GetComponent<Button>().enabled = true;

        }
        if (LostGameObject != null)
        {
            LostGameObject.SetActive(false);
        }
        animator.SetBool("isBadFlip", false);
        
        ShuffleCards();
    }


    public void FlipCard(GameObject cardObj)
    {
        TextMeshProUGUI cardNumberText = cardObj.GetComponentInChildren<TextMeshProUGUI>();

        Debug.Log("Spin bet: " + spinScript.spinBets + " multiplier: " + multiplier);

        switch (Convert.ToInt32(cardNumberText.text))
        {
            case 1:
                //foreach (GameObject card in cardGameObjects)
                //{
                //    card.GetComponent<Button>().enabled = false;
                //}
                cardObj.GetComponent<Image>().color = Color.black;
                LostGameObject = cardObj.gameObject.transform.GetChild(1).gameObject;
                LostGameObject.SetActive(true);
                animator.SetBool("isBadFlip", true);

                break;
            case 2:
                præmie = 2 + (2 + spinScript.spinBets * multiplier);

                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x" + præmie; 

                gevints += præmie; 
                gevintsText.text = gevintsString + gevints + "kr";
                Debug.Log("reward added to 1: " + (præmie - 2));
                break;
            case 3:
                præmie = 5 + (5 + spinScript.spinBets * multiplier);

                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x" + præmie;
                
                gevints += float.Parse(cardNumberText.text.Substring(1));
                gevintsText.text = gevintsString + gevints + "kr";
                Debug.Log("reward added to 2: " + (præmie - 5));
                break;
            case 4:
                præmie = 5 + (5 + spinScript.spinBets * multiplier);

                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x" + præmie;

                gevints += float.Parse(cardNumberText.text.Substring(1));
                gevintsText.text = gevintsString + gevints + "kr";
                Debug.Log("reward added to 3: " + (præmie - 5));
                break;
            case 5:
                præmie = 10 + (10 + spinScript.spinBets * multiplier);

                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x" + præmie;
                
                gevints += float.Parse(cardNumberText.text.Substring(1));
                gevintsText.text = gevintsString + gevints + "kr";
                Debug.Log("reward added to 4: " + (præmie - 10));
                break;
            case 6:
                præmie = 10 + (10 + spinScript.spinBets * multiplier);

                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x" + præmie;
                
                gevints += float.Parse(cardNumberText.text.Substring(1));
                gevintsText.text = gevintsString + gevints + "kr";
                Debug.Log("reward added to 5: " + (præmie - 10));
                break;
            case 7:
                præmie = 30 + (30 + spinScript.spinBets * multiplier);

                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x" + præmie;
                
                gevints += float.Parse(cardNumberText.text.Substring(1));
                gevintsText.text = gevintsString + gevints + "kr";
                Debug.Log("reward added to 6: " + (præmie - 30));
                break;
            case 8:
                præmie = 50 + (50 + spinScript.spinBets * multiplier);

                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x" + præmie;
                
                gevints += float.Parse(cardNumberText.text.Substring(1));
                gevintsText.text = gevintsString + gevints + "kr";
                Debug.Log("reward added to 7: " + (præmie - 50));
                break;
            case 9:
                præmie = 100 + (100 + spinScript.spinBets * multiplier);

                cardObj.GetComponent<Image>().sprite = goodHit;
                cardNumberText.enabled = true;
                cardNumberText.text = "x" + præmie;
                
                gevints += float.Parse(cardNumberText.text.Substring(1));
                gevintsText.text = gevintsString + gevints + "kr";
                Debug.Log("reward added to 8: " + (præmie - 100));
                break;
        }

        
    }


 
}

