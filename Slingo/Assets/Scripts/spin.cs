using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


    [SerializeField] private Sprite[] BackgroundImages;

    [Space(10)]
    [Header("Spin settings")]
    public float spinSpeed = 5f;
    /// <summary>
    /// wait time before next spin, when auto spin
    /// </summary>
    [SerializeField] private float spinWaitTime;
    [SerializeField] private int wildChance;
    public bool isSpinning = false;

    [Space(5)]
    public TextMeshProUGUI spinLeftText;
    public Button spinButton;
    [Space(10)]
    public List<GameObject> slotsList = new List<GameObject>();
    private List<TextMeshProUGUI> slotTextList = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> textToGoEmpty = new List<TextMeshProUGUI>();
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
    public int spinLeft = 10;
    public int spinBuyLimit = 8;
    [HideInInspector] public float stakes = 0;
    int rnd;
    int min = 1;
    int max = 15;
    int wildPicked = 0;
    private List<Image> starImgs = new List<Image>();

    private List<Animator> spinAnimations = new List<Animator>();
    private Animator spinButtonAnimation;
    private AI AI;
    private TextMeshProUGUI bestChoiceText;
    private Calculations calculations;

    PanelEffects blinkEffect;
    #endregion

    [SerializeField] Test subjectToObserve;

    private void OnStuffHappens()
    {
        this.GetComponent<Image>().color = Color.blue;
        Debug.Log("Stuff is happening");
    }


    private void Awake()
    {


        collectReward = this.gameObject.GetComponentInParent<CollectReward>();
        AI = GetComponentInParent<AI>();
        if(spinButton != null) spinButtonAnimation = spinButton.GetComponent<Animator>();
        foreach (GameObject spinSlot in slotsList)
        {
            slotTextList.Add(spinSlot.GetComponentInChildren<TextMeshProUGUI>());
            spinAnimations.Add(spinSlot.gameObject.GetComponent<Animator>());
        }
        blinkEffect = GetComponent<PanelEffects>();
        calculations = GetComponent<Calculations>();
    }


    private void Update()
    {
        NumberPingPong();
        //spinLeft = 10;
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
        if(isSpinning || gridButton.GetComponent<TextMeshProUGUI>().text == "")
        {
            return;
        }

        GameObject wildNumberPicked = gridButton.GetComponentInChildren<Animator>().gameObject;
        int numberPressed = Convert.ToInt32(gridButton.GetComponent<TextMeshProUGUI>().text);

        if (wildPicks == 0)
        {
            if(gridGeneration.numberPositions[numberPressed].hasBeenHit && wildNumberPicked)
            {
                Animator animator = wildNumberPicked.GetComponentInChildren<Animator>();
                Image starImg = animator.transform.GetChild(0).GetComponent<Image>();
                if(starImg.color.a != 0)
                {
                    starImg.GetComponentInParent<TextMeshProUGUI>().text = "";
                    animator.SetBool("Duppe", true);
                    StartCoroutine(Fade(starImg));
                }
            }
        }
        else
        {
            
            
            if (wildPicked < wildPicks)
            {
                if (!gridGeneration.numberPositions[numberPressed].hasBeenHit)
                {
                    gridGeneration.numberPositions[numberPressed].Hit();
                    gridGeneration.numberPositions[numberPressed].gameObject.GetComponent<TextMeshProUGUI>().text = "";

                    GameObject wild = wilds.Dequeue();
                    wild.GetComponentInChildren<Image>().color = Color.green;
                    wild.GetComponentInChildren<Outline>().GetComponent<Animator>().SetBool("Wild", false);

                    wildPicked++;
                }
            }
            if (wildPicked == wildPicks)
            {
                blinkEffect.blinkeffectStart = false;
                WildTransparency(true, wildNumberPicked);
                bestChoiceText.color = Color.white;
                if (spinLeft <= 0)
                {
                    spinButton.GetComponent<Image>().color = Color.black;
                    spinButton.GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
                    spinButton.GetComponentInChildren<TextMeshProUGUI>().text = "Price pr. spin " + UIManager.Instance.DisplayMoney(calculations.PriceCaculator());
                }
            }
            else
            {
                blinkEffect.blinkeffectStart = false;
                WildTransparency(false, wildNumberPicked);
                bestChoiceText.color = Color.white;
                GridNumbers bestChoice = AI.BestChoice();
                if (bestChoice == null) return;
                bestChoice.gameObject.GetComponentInChildren<Image>().sprite = BackgroundImages[2];
                bestChoiceText = gridGeneration.numberPositions[bestChoice.number].gameObject.GetComponentInChildren<TextMeshProUGUI>();
                blinkEffect.FlashingEffect(gridGeneration.numberPositions[bestChoice.number].gameObject.GetComponentInChildren<TextMeshProUGUI>());
            }
        }
    }

    public void StartSpin()
    {
        if (isSpinning || gridCheck.starsCount == 25 || spinBuyLimit == 0) return;
        isSpinning = true;
        Stakes();
        ColorReset();
        StartCoroutine(Fade());


        foreach (GridNumbers gridnumber in gridGeneration.numberPositions.Values) 
        {
            gridnumber.gameObject.GetComponentInChildren<Image>().enabled = false;
        }

        foreach (TextMeshProUGUI textNumber in textToGoEmpty)
        {
            textNumber.text = "";
        }

        foreach (GameObject slot in slotsList)
        {
            if (slot.GetComponentInChildren<Image>().enabled)
            {
                slot.GetComponentInChildren<Image>().color = Color.white;
                slot.GetComponentInChildren<Image>().enabled = false;
            }
        }
        
        if (spinLeft == 10)
        {
            spinButton.GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(false);
            spinButton.GetComponent<Image>().color = Color.white;
            playerData.balance -= spinBets;
        }
        

        if (spinBuyLimit >= 0 && spinLeft < 0)
        {
            playerData.balance -= UIManager.Instance.GetMoneyValue(spinButton.GetComponentInChildren<TextMeshProUGUI>().text.Substring(14));

            spinBuyLimit--;
            Debug.Log("Buy limit: " + spinBuyLimit);
            spinLeftText.text = spinBuyLimit.ToString();
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

            slot.GetComponentInChildren<Outline>().GetComponent<Animator>().SetBool("Wild", true);
            slot.GetComponentInChildren<TextMeshProUGUI>().text = ""; //Clears number behind the star when getting wild

            //blinkEffect = FindObjectsByType<PanelEffects>(FindObjectsSortMode.None);

            //for (int i = 0; i < blinkEffect.Length; i++)
            //{
            //    blinkEffect[i].FlashingEffect();
            //}
            wildPicks++;
        }
        else
        {
            TextMeshProUGUI text = slot.GetComponentInChildren<TextMeshProUGUI>();
            text.text = rnd.ToString();
            StartCoroutine(CheckMatchingNumb(text, Convert.ToInt32(text.text)));
        }
    }

    private IEnumerator CheckMatchingNumb(TextMeshProUGUI text, int number)
    {

        if (gridGeneration.numberPositions.ContainsKey(number) && !gridGeneration.numberPositions[number].hasBeenHit)
        {
            text.color = Color.green;
            yield return new WaitForSeconds(0.5f);
            gridGeneration.numberPositions[number].Hit();

            Transform goTrans = gridGeneration.numberPositions[number].gameObject.transform.GetChild(0).GetChild(0);
            Image starImg = goTrans.GetComponentInChildren<Image>();
            starImg.color = new Color(starImg.color.r, starImg.color.g, starImg.color.b, 0.4f);
            starImgs.Add(starImg);
            textToGoEmpty.Add(gridGeneration.numberPositions[number].gameObject.GetComponent<TextMeshProUGUI>());
        }
    }

    public void Stakes()
    {
        if (spinLeft == 10)
        {
            stakes += spinBets;
        }
        if (spinLeft < 0)
        {
            stakes += calculations.PriceCaculator();
        }
        spentText.text = "Stakes: " + stakes.ToString("F2") + " kr";

    }

    private void SpinsLeft()
    {
        if (spinLeft <= 0)
        {
            spinButton.GetComponent<Image>().color = Color.black;
            spinButton.GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
            spinButton.GetComponentInChildren<TextMeshProUGUI>().text = "Price pr. spin " + UIManager.Instance.DisplayMoney(calculations.PriceCaculator());
            spinCountHeader.text = "Buy limit";
            spinLeftText.text = spinBuyLimit.ToString();
        }
        if (wildPicked == 0)
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

        if(spinLeft >= 0)
        {
            spinButtonAnimation.SetBool("Spin", true);
            yield return new WaitForSeconds(0.1f);
            spinButtonAnimation.SetBool("Spin", false);
        }
        

        yield return new WaitForSeconds(spinWaitTime - 0.1f);
        min = 1;
        max = 16;
        foreach (Animator item in spinAnimations)
        {
            yield return new WaitForSeconds(0.5f);
            item.SetBool("Spinning", false);
            Spin(item.gameObject);
            min += 15;
            max += 15;
        }
        
        if (wildPicks > 0)
        {
            yield return new WaitForSeconds(0.5f);
            WildTransparency(false);
            GridNumbers bestChoice = AI.BestChoice();
            if(bestChoice != null)
            {
                bestChoice.gameObject.GetComponentInChildren<Image>().sprite = BackgroundImages[2];
                bestChoiceText = gridGeneration.numberPositions[bestChoice.number].gameObject.GetComponentInChildren<TextMeshProUGUI>();
                blinkEffect.FlashingEffect(bestChoice.gameObject.GetComponent<TextMeshProUGUI>());
            }
        }
        
        yield return new WaitForSeconds(0.4f);
        SpinsLeft();
    }

    private void WildTransparency(bool stop, GameObject wildNumberPicked = null)
    {
        if (!stop)
        {
            foreach (GridNumbers number in gridGeneration.numberPositions.Values)
            {
                if (!number.hasBeenHit)
                {
                    Animator animatorObject = number.gameObject.GetComponentInChildren<Animator>();
                    animatorObject.GetComponent<Image>().sprite = BackgroundImages[1];
                    animatorObject.GetComponent<Image>().enabled = true;
                }
            }
            if (wildNumberPicked != null)
            {
                wildNumberPicked.GetComponent<Image>().enabled = false;
            }
        }
        else
        {
            foreach (GridNumbers number in gridGeneration.numberPositions.Values)
            {
                if (!number.hasBeenHit)
                {
                    Animator animatorObject = number.gameObject.GetComponentInChildren<Animator>();
                    animatorObject.GetComponent<Image>().enabled = false;
                }
                
            }
            if(wildNumberPicked != null)
            {
                wildNumberPicked.GetComponent<Image>().enabled = false;
            }
        }
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
        foreach (GridNumbers gridNumbers in gridGeneration.numberPositions.Values)
        {
            gridNumbers.gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
        }

        if(bestChoiceText != null)
        {
            blinkEffect.blinkeffectStart = false;
            bestChoiceText.color = Color.white;
        }
    }

    IEnumerator Fade(Image starImg = null)
    {
        if(starImg == null)
        {
            foreach (Image star in starImgs)
            {
                while (star.color.a < 1)
                {
                    star.color = new Color(star.color.r, star.color.g, star.color.b, star.color.a + 0.05f);
                    yield return null;
                }
                yield return new WaitForSeconds(0.05f);
            }
            starImgs.Clear();
        }
        else
        {
            while (starImg.color.a < 1)
            {
                starImg.color = new Color(starImg.color.r, starImg.color.g, starImg.color.b, starImg.color.a + 0.05f);
                yield return null;
            }
            starImgs.Remove(starImg);
        }

    }
}
