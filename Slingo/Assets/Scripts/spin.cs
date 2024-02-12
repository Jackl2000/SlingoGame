
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public Sprite[] BackgroundImages;

    [Space(10)]
    [Header("Spin settings")]
    public float spinSpeed = 5f;
    /// <summary>
    /// wait time before next spin, when auto spin
    /// </summary>
    [SerializeField] private float spinWaitTime;
    [SerializeField] private int wildChance;
    [SerializeField] private int wildArrowChance;
    public int spinLeft = 10;
    public int spinBuyLimit = 5;
    public bool isSpinning = false;

    [Space(5)]
    public TextMeshProUGUI spinLeftText;
    public Button spinButton;
    public TextMeshProUGUI resetButton;
    [Space(10)]
    public List<GameObject> slotsList = new List<GameObject>();
    public Sprite[] wildsImages;
    private List<TextMeshProUGUI> slotTextList = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> textToGoEmpty = new List<TextMeshProUGUI>();
    public Queue<GameObject> wilds = new Queue<GameObject>();
    public Queue<GameObject> wildsArrow = new Queue<GameObject>();
    private List<int> slotWildArrow = new List<int>();
    [SerializeField] private Button continueButton;
    [SerializeField] private Button stopSpinningButton;

    #region others variables
    [HideInInspector] public float spinBets = 1;
    /// <summary>
    /// How many wildpicks you have
    /// </summary>
    [HideInInspector] public int wildPicks = 0;
    /// <summary>
    /// spins left before you pay
    /// </summary>

    [HideInInspector] public float stakes = 0;
    int rnd;
    int min = 1;
    int max = 15;
    int wildPicked = 0;
    [HideInInspector] public float timePassedForMsg;
    private List<Image> starImgs = new List<Image>();

    private List<Animator> spinAnimations = new List<Animator>();
    private Animator spinButtonAnimation;
    private AI AI;
    private TextMeshProUGUI bestChoiceText;
    private Calculations calculations;

    PanelEffects blinkEffect;

    [Header("MessageObjects")]
    [SerializeField] private GameObject keepSpinningPanel;
    [SerializeField] private GameObject CostMessage;
    private bool isMessageActive = false;
    private bool warning = true;

    #endregion


    private void Awake()
    {
        resetButton.color = Color.gray;
        resetButton.GetComponentInParent<Button>().enabled = false;
        collectReward = this.gameObject.GetComponentInParent<CollectReward>();
        AI = GetComponentInParent<AI>();
        if (spinButton != null) spinButtonAnimation = spinButton.GetComponent<Animator>();
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

        if (gridCheck.slingoAnimationFinished && gridCheck.slingoCount >= 12)
        {
            timePassedForMsg += Time.deltaTime;
        }

        if (spinLeft == 0)
        {

            //spinLeft remains zero causing loop to be entered constantly, unless its set to -1
            spinLeft = -1;
            //spinButton.enabled = true;

            Debug.Log("Spinleft:" + spinLeft +
                "\n" + "Spinning stopped");
        }

        balanceText.text = UIManager.Instance.DisplayMoney(playerData.balance);


        #region Enables to pick any number on plate if user got wildpicks
        if (wildPicked == wildPicks)
        {
            //if (gridCheck.slingoAnimationFinished && spinLeft <= 0 && spinBuyLimit == 8 && !isSpinning && isUnderstood)
            //{
            //    StartCoroutine(MessageHandler(1.5f));
            //    isUnderstood = false;
            //}

            wildPicks = 0;
            wildPicked = 0;
            //spinButton.enabled = true;
        }
        else
        {
            //spinButton.enabled = false;
        }

        #endregion
    }

    public void WildPick(Button gridButton)
    {
        if (gridButton.GetComponentInChildren<TextMeshProUGUI>().text == "")
        {
            return;
        }

        GameObject wildNumberPicked = gridButton.GetComponentInChildren<Animator>().gameObject;
        int numberPressed = Convert.ToInt32(gridButton.GetComponentInChildren<TextMeshProUGUI>().text);

        StarDupping(wildNumberPicked, numberPressed);

        if (wildPicks == 0)
        {
            return;
        }

        if (slotWildArrow.Count > 0) WildArrow(wildNumberPicked, numberPressed);
        else if (wilds.Count > 0) SuperWild(wildNumberPicked, numberPressed);
    }

    private void StarDupping(GameObject wildNumberPicked, int numberPressed)
    {
        if (gridGeneration.numberPositions[numberPressed].hasBeenHit && wildNumberPicked && starImgs.Contains(wildNumberPicked.GetComponentInParent<Animator>().transform.GetChild(0).GetComponent<Image>()))
        {
            Animator animator = wildNumberPicked.GetComponentInChildren<Animator>();
            Image starImg = animator.transform.GetChild(0).GetComponent<Image>();
            if (starImg.color.a != 0)
            {
                animator.GetComponentInChildren<TextMeshProUGUI>().text = "";
                animator.SetBool("Duppe", true);
                StartCoroutine(Fade(starImg));
                return;
            }
        }
    }

    private void SuperWild(GameObject wildNumberPicked, int numberPressed)
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
        WildReset(wildNumberPicked, 0);
    }

    private void WildArrow(GameObject wildNumberPicked, int numberPressed)
    {
        if (wildPicked < wildPicks)
        {
            foreach (int slot in slotWildArrow)
            {
                if (gridGeneration.numberPositions[numberPressed].h == slot && !gridGeneration.numberPositions[numberPressed].hasBeenHit)
                {
                    gridGeneration.numberPositions[numberPressed].Hit();
                    gridGeneration.numberPositions[numberPressed].gameObject.GetComponent<TextMeshProUGUI>().text = "";
                    wildPicked++;
                    GameObject wild = wildsArrow.Dequeue();
                    wild.GetComponentInChildren<Outline>().GetComponent<Animator>().SetBool("Wild", false);
                }
            }
        }
        WildReset(wildNumberPicked, gridGeneration.numberPositions[numberPressed].h);
    }

    private void WildReset(GameObject wildNumberPicked, int index)
    {
        blinkEffect.FlashingEffect(false, bestChoiceText);
        WildTransparency(true, wildNumberPicked, index);

        if (wildPicked == wildPicks && gridCheck.slingoAnimationFinished)
        {
            SpinButtonReset();
        }
        else if (wildPicked != wildPicks)
        {
            if (wildPicked == wildPicks) return;

            GridNumbers bestChoice = AI.BestChoice(wilds.Count, slotWildArrow);
            if (bestChoice == null) return;

            if (slotWildArrow.Count > 0) bestChoice.gameObject.GetComponentInParent<Image>().sprite = BackgroundImages[0];
            else bestChoice.gameObject.GetComponentInParent<Image>().sprite = BackgroundImages[2];
            bestChoiceText = gridGeneration.numberPositions[bestChoice.number].gameObject.GetComponentInChildren<TextMeshProUGUI>();
            blinkEffect.FlashingEffect(true, gridGeneration.numberPositions[bestChoice.number].gameObject.GetComponentInChildren<TextMeshProUGUI>());
        }
    }

    public void HideMessage(Toggle toggle) //This is called with toogle in KeepSpinningPanel
    {
        if (toggle.isOn) warning = false;
        else warning = true;
    }

    public void StartSpin()
    {
        if (isSpinning || gridCheck.starsCount == 25 || spinBuyLimit == 0) return;

        float costPrSpin = calculations.PriceCaculator();

        #region --Not in use-- CostWarning message to pop on spin click button 
        //if (spinLeftText.text == "0" && !isMessageActive)
        //{
        //    isMessageActive = true;
        //    StartCoroutine(MessageHandler(CostMessage, 0, "Du har opbrugt all dine spins :( Ekstra spins vil koste pr. spin"));
        //    spinCountHeader.text = "Extra spins";
        //    return;
        //}
        #endregion

        if (spinLeft < 0 && costPrSpin > 0 && !isMessageActive && warning)

        {
            isMessageActive = true;
            StartCoroutine(MessageHandler(keepSpinningPanel, 0, $"Vil du forsætte med at spinne, dit næste spin koster {UIManager.Instance.DisplayMoney(calculations.PriceCaculator())}"));
            return;
        }

        isSpinning = true;
        Stakes();
        ColorReset();
        StartCoroutine(Fade());

        keepSpinningPanel.SetActive(false);

        foreach (GridNumbers gridnumber in gridGeneration.numberPositions.Values)
        {
            //gridnumber.gameObject.GetComponentInChildren<Image>().enabled = false;
            gridnumber.gameObject.transform.parent.GetComponent<Image>().enabled = false;
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
            isMessageActive = false;
            spinButton.GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(false);
            spinButton.GetComponent<Image>().color = Color.white;
            playerData.balance -= spinBets;
        }
        if (spinBuyLimit >= 0 && spinLeft <= 0 && gridCheck.slingoAnimationFinished)
        {

            isMessageActive = false;
            playerData.balance -= UIManager.Instance.GetMoneyValue(spinButton.GetComponentInChildren<TextMeshProUGUI>().text.Substring(6));

            spinButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
            spinBuyLimit--;
            spinLeftText.text = spinBuyLimit.ToString();
        }
        else
        {
            spinButton.GetComponent<Image>().color = Color.gray;
            spinLeft--;
            spinLeftText.text = spinLeft.ToString();
        }
        StartCoroutine(Spinner());
    }



    public void Spin(GameObject slot, int index)
    {
        rnd = UnityEngine.Random.Range(min, max);

        int wildArrow = UnityEngine.Random.Range(0, wildArrowChance + 1);
        int wildPick = UnityEngine.Random.Range(0, wildChance + 1);

        if (wildArrow == 2)
        {
            slotWildArrow.Add(index);
            wildsArrow.Enqueue(slot);
            slot.GetComponentInChildren<Image>().sprite = wildsImages[1];
            slot.GetComponentInChildren<Image>().enabled = true;
            slot.GetComponentInChildren<Outline>().GetComponent<Animator>().SetBool("Wild", true);
            slot.GetComponentInChildren<TextMeshProUGUI>().text = "";
            wildPicks++;
        }
        else
        {
            if (wildPick == 5)
            {
                wilds.Enqueue(slot);
                slot.GetComponentInChildren<Image>().sprite = wildsImages[0];
                slot.GetComponentInChildren<Image>().enabled = true;

                slot.GetComponentInChildren<Outline>().GetComponent<Animator>().SetBool("Wild", true);
                slot.GetComponentInChildren<TextMeshProUGUI>().text = ""; //Clears number behind the star when getting wild
                wildPicks++;
                return;
            }

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
            gridGeneration.numberPositions[number].gameObject.transform.parent.transform.parent.GetComponent<StarManager>().StarParticleEffect();
            //Transform goTrans = gridGeneration.numberPositions[number].gameObject.transform.parent.GetChild(0).GetComponentInChildren<Image>();
            Image starImg = gridGeneration.numberPositions[number].gameObject.transform.parent.GetChild(0).GetComponentInChildren<Image>();
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
        spentText.text = "Indsats: " + "\n" + stakes.ToString("F2") + "kr";

    }

    private void SpinsLeft()
    {
        if (wildPicks == 0 && gridCheck.slingoAnimationFinished)
        {
            SpinButtonReset();
        }
    }

    IEnumerator Spinner()
    {
        foreach (Animator item in spinAnimations)
        {
            item.SetBool("Spinning", true);
        }

        if (spinLeft >= 0)
        {
            spinButtonAnimation.SetBool("Spin", true);
            yield return new WaitForSeconds(0.1f);
            spinButtonAnimation.SetBool("Spin", false);
        }


        yield return new WaitForSeconds(spinWaitTime - 0.1f);
        min = 1;
        max = 16;
        int index = 0;
        foreach (Animator item in spinAnimations)
        {
            index++;
            yield return new WaitForSeconds(0.3f);
            item.SetBool("Spinning", false);
            Spin(item.gameObject, index);
            min += 15;
            max += 15;
        }

        if (wildPicks > 0)
        {
            yield return new WaitForSeconds(0.5f);

            WildTransparency(false);
            GridNumbers bestChoice = AI.BestChoice(wilds.Count, slotWildArrow);
            if (bestChoice != null)
            {
                if(slotWildArrow.Count > 0) bestChoice.gameObject.GetComponentInParent<Image>().sprite = BackgroundImages[0];
                else bestChoice.gameObject.GetComponentInParent<Image>().sprite = BackgroundImages[2];

                bestChoiceText = gridGeneration.numberPositions[bestChoice.number].gameObject.GetComponentInChildren<TextMeshProUGUI>();
                blinkEffect.FlashingEffect(true, bestChoice.gameObject.GetComponent<TextMeshProUGUI>());
            }
        }

        yield return new WaitForSeconds(0.4f);
        SpinsLeft();

    }

    public void SpinButtonReset()
    {
        if(spinLeft > 0)
        {
            spinButton.GetComponent<Image>().color = Color.white;
        }
        else
        {
            if (spinBuyLimit == 5 && !isMessageActive)
            {
                spinCountHeader.text = "Max spin køb";
                spinLeftText.text = spinBuyLimit.ToString();

                resetButton.color = Color.white;
                resetButton.GetComponentInParent<Button>().enabled = true;

                isMessageActive = true;
                StartCoroutine(MessageHandler(CostMessage, 0f, "Du har opbrugt all dine spins :( Ekstra spins vil koste pr. spin"));
                Button costMsgButton = CostMessage.GetComponentInChildren<Button>();
                costMsgButton.onClick.RemoveListener(collectReward.Collect);
                costMsgButton.GetComponentInChildren<TextMeshProUGUI>().text = "Spin for " + "\n" + $"{UIManager.Instance.DisplayMoney(calculations.PriceCaculator())}";
                return;
            }
            else if (spinBuyLimit == 0)
            {
                string messageText = "SPIL SLUT" + "\n" + "Du har tjent " + UIManager.Instance.DisplayMoney(gridCheck.rewards[gridCheck.slingoCount]);
                if (gridCheck.slingoCount >= 3)
                {
                    StartCoroutine(MessageHandler(CostMessage, 1f, messageText));
                    CostMessage.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>().text = "Tag gevints";
                    CostMessage.GetComponentInChildren<Button>().onClick.AddListener(collectReward.Collect);
                }
                else
                {
                    StartCoroutine(MessageHandler(CostMessage, 1f, messageText));
                    //CostMessage.GetComponentInChildren<TextMeshProUGUI>().text = "SPILLET SLUT";
                    CostMessage.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>().text = "Næste Spil";
                }
            }
            else
            {
                spinButton.GetComponent<Image>().color = Color.black;
                spinButton.GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
                spinButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                spinButton.GetComponentInChildren<TextMeshProUGUI>().text = "Pris: " + UIManager.Instance.DisplayMoney(calculations.PriceCaculator());
            }
        }

        if (gridCheck.starsCount == 25)
        {
            spinButton.GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
            resetButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            spinButton.GetComponentInChildren<TextMeshProUGUI>().text = "JACKPOT FLASH!!!";
        }
        isSpinning = false;
    }

    public void StartButtonCostAnimation()
    {
        if (spinLeft !<= 0 && spinBuyLimit != 5) return; 
        StartCoroutine(SpinButtonCostAnimation());
    }

    public IEnumerator SpinButtonCostAnimation()
    {
        spinButton.GetComponent<Animator>().SetBool("Cost", true);
        yield return new WaitForSeconds(0.5f);
        spinButton.GetComponent<Animator>().SetBool("Cost", false);
        spinButton.GetComponent<Image>().color = Color.black;
        spinButton.GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
        spinButton.GetComponentInChildren<TextMeshProUGUI>().text = "Pris: " + UIManager.Instance.DisplayMoney(calculations.PriceCaculator());
        yield return new WaitForSeconds(0.3f);
        SpinButtonReset();
        StartSpin();
    }

    private void WildTransparency(bool stop, GameObject wildNumberPicked = null, int hIndex = 0)
    {
        if (!stop)
        {
            //Arrow
            if (slotWildArrow.Count > 0)
            {
                List<int> indexes = new List<int>() { 0, 0, 0, 0, 0 };
                foreach (GridNumbers number in gridGeneration.numberPositions.Values)
                {
                    if (!number.hasBeenHit && slotWildArrow.Contains(number.h))
                    {
                        Animator animatorObject = number.gameObject.GetComponentInParent<Animator>();
                        animatorObject.GetComponent<Image>().sprite = BackgroundImages[3];
                        animatorObject.GetComponent<Image>().enabled = true;
                        indexes[number.h - 1]++;
                    }
                }
                if (wildNumberPicked != null)
                {
                    wildNumberPicked.GetComponent<Image>().enabled = false;
                }
                int indexh = 0;
                foreach (int index in indexes)
                {
                    indexh++;
                    if (index == 0 && slotWildArrow.Contains(indexh))
                    {
                        wildPicked++;
                        blinkEffect.FlashingEffect(false, bestChoiceText);
                        WildTransparency(true, null, indexh);
                        //if (wildPicked == wildPicks && gridCheck.slingoAnimationFinished)
                        //{
                        //    SpinButtonReset();
                        //}
                    }
                }
            }
            else
            {
                //Super
                foreach (GridNumbers number in gridGeneration.numberPositions.Values)
                {
                    if (!number.hasBeenHit)
                    {
                        Animator animatorObject = number.gameObject.GetComponentInParent<Animator>();
                        animatorObject.GetComponent<Image>().sprite = BackgroundImages[1];
                        animatorObject.GetComponent<Image>().enabled = true;
                    }
                }
            }
        }
        else
        {
            if (hIndex != 0)
            {
                //Arrow
                foreach (GridNumbers number in gridGeneration.numberPositions.Values)
                {
                    if (!number.hasBeenHit && number.h == hIndex)
                    {
                        Animator animatorObject = number.gameObject.GetComponentInParent<Animator>();
                        animatorObject.GetComponent<Image>().sprite = BackgroundImages[3];
                        animatorObject.GetComponent<Image>().enabled = false;
                    }
                }
                slotWildArrow.Remove(hIndex);
            }
            else
            {
                //Super
                foreach (GridNumbers number in gridGeneration.numberPositions.Values)
                {
                    if (!number.hasBeenHit)
                    {
                        Animator animatorObject = number.gameObject.GetComponentInParent<Animator>();
                        animatorObject.GetComponent<Image>().sprite = BackgroundImages[1];
                        animatorObject.GetComponent<Image>().enabled = false;
                    }
                }
            }

            if (wildNumberPicked != null)
            {
                wildNumberPicked.GetComponent<Image>().enabled = false;
            }

            //Check for more wilds
            if (slotWildArrow.Count > 0)
            {

                foreach (int slot in slotWildArrow.ToArray())
                {
                    WildTransparency(false, null, slot);
                }
            }
            else if (wilds.Count > 0)
            {
                WildTransparency(false);
            }
        }
    }

    IEnumerator MessageHandler(GameObject messageObject, float secondsToWait, string messageText)
    {
        yield return new WaitForSeconds(secondsToWait);

        messageObject.SetActive(true);
        messageObject.GetComponentInChildren<TextMeshProUGUI>().text = messageText;
    }

    public void SetSpinBuyLimit(int buyLimit)
    {
        spinBuyLimit = buyLimit;
        spinLeftText.text = spinBuyLimit.ToString();
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
    }

    IEnumerator Fade(Image starImg = null)
    {
        if (starImg == null)
        {
            foreach (Image star in starImgs.ToArray())
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
