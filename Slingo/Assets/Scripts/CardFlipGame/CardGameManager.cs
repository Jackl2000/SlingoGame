using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardGameManager : MonoBehaviour
{
    [Header("References")]
    private PlayerData playerData;
    public spin spinScript;
    public GameObject GameFinishedPanel;
    private CardGameManager gameManager;
    private Animator animator;

    [Space(5)]
    public TextMeshProUGUI gevintsText;
    public TextMeshProUGUI indsatsText;
    public TextMeshProUGUI balanceText;

    [Space(5)]
    [Header("Reward Setting")]
    //public float multiplier = 0.5f;
    private int rndCard;
    private float gevints;
    private GameObject LostGameObject;
    public TextMeshProUGUI gevinstButtonText;

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
    private bool gameDone = false;

    private void Awake()
    {
        gameManager = this.GetComponent<CardGameManager>();
        animator = this.GetComponent<Animator>();
        foreach (GameObject goCard in cardGameObjects)
        {
            cardTexts.Add(goCard.GetComponentInChildren<TextMeshProUGUI>());
            cardImages.Add(goCard.GetComponent<Image>());
        }
        ShuffleCards();
        //if (GameObject.Find("PlayerData").gameObject.GetComponent<PlayerData>() == null)
        //{
        //    playerData = new PlayerData();
        //}
        //else
        //{
        //    playerData = GameObject.Find("PlayerData").gameObject.GetComponent<PlayerData>();
        //}
    }

    private void Start()
    {
        gevintsString = "Bonus gevints: ";

        balanceText.text = "Balance: " + PlayerData.Instance.balance.ToString();

        
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
        PlayerData.Instance.balance += gevints;
        balanceText.text = "Balance: " + UIManager.Instance.DisplayMoney(PlayerData.Instance.balance);
        indsatsText.text = "Indsats: " + UIManager.Instance.DisplayMoney(PlayerData.Instance.bet);
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
        if (gameDone) return;
        TextMeshProUGUI cardNumberText = cardObj.GetComponentInChildren<TextMeshProUGUI>();

        float multiplier = PlayerData.Instance.bet;
        float præmie = 0;
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
                gameDone = true;
                break;
            case 2:
                præmie = 0.5f * multiplier;
                break;
            case 3:
                præmie = multiplier;
                break;
            case 4:
                præmie = 1.5f * multiplier;
                break;
            case 5:
                præmie = 2 * multiplier;
                break;
            case 6:
                præmie = 5 * multiplier;
                break;
            case 7:
                præmie = 8 * multiplier;
                break;
            case 8:
                præmie = 12 * multiplier;
                break;
            case 9:
                præmie = 20 * multiplier;
                break;
        }

        if(præmie != 0)
        {
            cardObj.GetComponent<Image>().sprite = goodHit;
            cardNumberText.enabled = true;
            cardNumberText.text = præmie.ToString();

            gevints += præmie;
            gevintsText.text = gevintsString + gevints + "kr";
            Debug.Log("reward added to 1: " + (præmie - 2));
        }

        if(præmie == 0 || gevints == 50)
        {
            GameFinishedPanel.SetActive(true);
            gameDone = true;
        }

        gevinstButtonText.text = "Tag gevints: " + gevints + "kr";
        
    }

    public void ExitWithReward()
    {
        PlayerData.Instance.balance += gevints;
        balanceText.text = "Balance: " + PlayerData.Instance.balance.ToString();
        gevints = 0;
        gevintsText.text = "Bonus gevints: " + gevints.ToString();
        SceneSwap.Instance.SceneSwitch("Scene_tai");
    }
 
}

