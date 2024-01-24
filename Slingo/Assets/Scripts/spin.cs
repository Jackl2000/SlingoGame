using Codice.Client.BaseCommands;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class spin : MonoBehaviour
{
    [Header("References")]
    public GridGeneration gridGeneration;
    public GridCheck gridCheck;
    public PlayerData playerData;
    public TextMeshProUGUI spinCountHeader;
    private CollectReward collectReward;

    [Space(5)]
    public TextMeshProUGUI balanceText;
    public TextMeshProUGUI spentText;

    [Space(10)]
    [Header("Spin settings")]
    public float spinSpeed = 5f;
    /// <summary>
    /// wait time before next spin, when auto spin
    /// </summary>
    [SerializeField] private float spinWaitTime;
    [SerializeField] private int wildChance;
    [SerializeField] private bool isSpinning = false;

    [Space(5)]
    public TextMeshProUGUI spinLeftText;
    public Button spinButton;
    [Space(10)]
    public List<GameObject> slotsList = new List<GameObject>();
    private List<TextMeshProUGUI> slotTextList = new List<TextMeshProUGUI>();
    public Queue<GameObject> wilds = new Queue<GameObject>();


    #region others variables
    [HideInInspector] public float spinBets = 1;
    /// <summary>
    /// How many wildpicks you have
    /// </summary>
    [HideInInspector] public int wildPicks = 0;
    /// <summary>
    /// spins left before you pay
    /// </summary>
    [HideInInspector] public int spinLeft = 8;
    [HideInInspector] public float stakes = 0;
    int rnd;
    int min = 1;
    int max = 15;
    int wildPicked = 0;
    private int possibleRewardAmplifiere;
    //private Animator spinAnimation;
    private List<Animator> spinAnimations = new List<Animator>();
    private Animator spinButtonAnimation;
    

    PanelEffects[] blinkEffect;
    #endregion

    private void Awake()
    {
        collectReward = this.gameObject.GetComponentInParent<CollectReward>();
        //spinAnimation = GetComponentInChildren<Animator>();
        if(spinButton != null) spinButtonAnimation = spinButton.GetComponent<Animator>();
        foreach (GameObject spinSlot in slotsList)
        {
            slotTextList.Add(spinSlot.GetComponentInChildren<TextMeshProUGUI>());
            spinAnimations.Add(spinSlot.gameObject.GetComponent<Animator>());
        }
    }


    private void Update()
    {
        NumberPingPong();

        if (spinLeft == 0)
        {
            //spinLeft remains zero causing loop to be entered constantly, unless its set to -1
            spinLeft = -1;
            spinButton.enabled = true;

            Debug.Log("Spinleft:" + spinLeft +
                "\n" + "Spinning stopped");
        }

        balanceText.text = UIManager.Instance.DisplayMoney(playerData.balance);

        #region Enables to pick any number on plate if user got wildpicks
        if (wildPicked == wildPicks)
        {
            wildPicks = 0;
            wildPicked = 0;
            spinButton.enabled = true;
        }
        else
        {
            spinButton.enabled = false;
        }

        #endregion

    }

    public void WildPick(Button gridButton)
    {
        if (wildPicked < wildPicks)
        {
            foreach (int gridNumber in gridGeneration.numberPositions.Keys)
            {
                if (gridButton.gameObject.GetComponent<TextMeshProUGUI>().text != "")
                {
                    if (gridNumber == Convert.ToInt32(gridButton.gameObject.GetComponent<TextMeshProUGUI>().text))
                    {
                        gridGeneration.numberPositions[gridNumber].Hit(true);

                        GameObject wild = wilds.Dequeue();
                        wild.GetComponentInChildren<Image>().color = Color.green;
                        wildPicked++;
                        break;
                    }
                }
            }
        }
        if(wildPicked == wildPicks)
        {
            if (spinLeft <= 0)
            {
                spinCountHeader.text = "COST";
                PriceCaculator();
                isSpinning = false;
            }
        }
        spinButton.enabled = true;

    }
    public void StartSpin( )
    {
        if (isSpinning) return;

        ColorReset();

        foreach (GameObject slot in slotsList)
        {
            if (slot.GetComponentInChildren<Image>().enabled)
            {
                slot.GetComponentInChildren<Image>().color = Color.white;
                slot.GetComponentInChildren<Image>().enabled = false;
            }
        }
        
        if (spinLeft == 8)
        {
            spinButton.GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(false);
            spinButton.GetComponent<Image>().color = Color.white;
            playerData.balance -= spinBets;
        }
        isSpinning = true;

        if (spinLeft < 0)
        {
            playerData.balance -= UIManager.Instance.GetMoneyValue(spinLeftText.text);
            StartCoroutine(Spinner());

            //reset time for collect reward pop message
            collectReward.ResetTime();
        }
        else
        {
            spinLeft--;
            spinLeftText.text = spinLeft.ToString();
            StartCoroutine(Spinner());
        }

    }
    public void Spin(GameObject slot)
    {
        rnd = UnityEngine.Random.Range(min, max);

        int wildPick = UnityEngine.Random.Range(0, wildChance + 1);

        if (wildPick == 5)
        {
            wilds.Enqueue(slot);
            slot.GetComponentInChildren<Image>().enabled = true;
            slot.GetComponentInChildren<TextMeshProUGUI>().text = "";

            blinkEffect = FindObjectsByType<PanelEffects>(FindObjectsSortMode.None);

            for (int i = 0; i < blinkEffect.Length; i++)
            {
                blinkEffect[i].FlashingEffect();
            }
            wildPicks++;
            spinButton.enabled = false;
        }
        else
        {
            TextMeshProUGUI text = slot.GetComponentInChildren<TextMeshProUGUI>();
            text.text = rnd.ToString();

            StartCoroutine(CheckMatchingNumb(slot.GetComponentInChildren<TextMeshProUGUI>()));
        }

        
    }


    private IEnumerator CheckMatchingNumb(TextMeshProUGUI text)
    {
        if (gridGeneration.numberPositions.ContainsKey(Convert.ToInt32(text.text)) && !gridGeneration.numberPositions[Convert.ToInt32(text.text)].hasBeenHit)
        {
            text.color = Color.green;
            yield return new WaitForSeconds(0.5f);
            gridGeneration.numberPositions[Convert.ToInt32(text.text)].Hit(false);

            Transform goTrans;

            goTrans = gridGeneration.numberPositions[Convert.ToInt32(text.text)].gameObject.transform.GetChild(0).GetChild(0);
            Image starImg = goTrans.GetComponentInChildren<Image>();
            starImg.color = new Color(starImg.color.r, starImg.color.g, starImg.color.b, 0.4f);

        }
    }

    public void Stakes()
    {
        if (spinLeft == 8)
        {
            stakes += spinBets;
        }
        if (spinLeft < 0 && !isSpinning)
        {
            stakes += PriceCaculator();
        }
        spentText.text = "Stakes: " + stakes.ToString("F2") + " kr";

    }

    private void SpinsLeft()
    {
        if (spinLeft <= 0)
        {
            PriceCaculator();
            spinCountHeader.text = "COST";
        }
        if(wildPicked == 0)
        {
            isSpinning = false;
        }
        
    }

    IEnumerator Spinner()
    {
        if (wilds.Count > 0) wilds.Clear();
        foreach (Animator item in spinAnimations)
        {
            item.SetBool("Spinning", true);
        }
        spinButtonAnimation.SetBool("Spin", true);
        yield return new WaitForSeconds(0.1f);
        spinButtonAnimation.SetBool("Spin", false);
        yield return new WaitForSeconds(spinWaitTime - 0.1f);
        min = 1;
        max = 16;
        foreach (Animator item in spinAnimations)
        {
            yield return new WaitForSeconds(0.6f);
            item.SetBool("Spinning", false);
            Spin(item.gameObject);
            min += 15;
            max += 15;
        }
        yield return new WaitForSeconds(0.4f);
        SpinsLeft();
    }

    /// <summary>
    /// Interpolates between a and b
    /// </summary>
    void NumberPingPong()
    {
        int minNumber = 1;
        int maxNumber = 16;

        foreach (var text in slotTextList)
        {
            if (text.gameObject.GetComponentInParent<Animator>().GetBool("Spinning"))
            {
                int numbers = (int)Mathf.Lerp(minNumber, maxNumber, Mathf.PingPong(Time.time * spinSpeed, 1));
                text.text = numbers.ToString();
            }
            minNumber += 15;
            maxNumber += 15;
        }
    }

    public void ColorReset()
    {
        foreach (var slotText in slotTextList)
        {
            slotText.color = Color.white;
        }
    }

    private float PriceCaculator()
    {
        if (gridCheck.slingoCount == 12)
        {
            return 0;
        }
        possibleRewardAmplifiere = gridCheck.CheckForMaxReward();

        float starMultipliere = 0.015f + (gridCheck.starsCount * 0.05f);
        float slingoReward = 0.015f * (gridCheck.starsCount * 5);
        if (gridCheck.slingoCount == 0)
        {
            slingoReward = 0.015f + starMultipliere;
        }
        if (gridCheck.rewards.ContainsKey(gridCheck.slingoCount + 1))
        {
            slingoReward = (gridCheck.rewards[gridCheck.slingoCount + 1] / spinBets) / Mathf.Clamp((1 + gridCheck.slingoCount) / gridCheck.slingoCount, 3, (1 + gridCheck.slingoCount) / gridCheck.slingoCount) * (starMultipliere + 0.5f);
        }

        float maxSlingoAmplifiere = Mathf.Clamp(possibleRewardAmplifiere - 0.5f, 0.5f, 1.8f);
        float price = slingoReward * Mathf.Clamp(maxSlingoAmplifiere, 1, maxSlingoAmplifiere);
        price *= spinBets;
        spinLeftText.text = UIManager.Instance.DisplayMoney(price);
        return price;
    }

    private void TestCalculation()
    {
        float bet = 1;
        for (float m = 0; m < 3; m++)
        {
            for (float i = 0; i < 11; i++)
            {
                for (float j = 10; j < 25; j++)
                {
                    float multipliere = 0.015f + (j * 0.05f);
                    float slingoRewards = 0.015f * (j * 5);
                    if (i == 0)
                    {
                        slingoRewards = 0.015f + multipliere;
                    }
                    if (gridCheck.rewards.ContainsKey(Convert.ToInt32(i) + 1))
                    {
                        slingoRewards = (gridCheck.rewards[Convert.ToInt32(i) + 1] / bet) / Mathf.Clamp((1 + i) / i, 3, (1 + i) / i) * (multipliere + 0.5f);
                    }
                    float maxSlingoAmplifiere = Mathf.Clamp(m - 0.5f, 0.5f, 1.8f);
                    float price = slingoRewards * Mathf.Clamp(maxSlingoAmplifiere, 1, maxSlingoAmplifiere);
                    price *= bet;
                    Debug.Log("SlingoCount: " + i + " Starscount: " + j + " Multipliere: " + multipliere + " Amplifiere: " + maxSlingoAmplifiere + " final value: " + UIManager.Instance.DisplayMoney(price));
                }
            }
        }
    }
}
