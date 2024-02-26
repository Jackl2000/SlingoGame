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
    [Space(7)]
    public int startSpins = 10;
    [HideInInspector] public int spinLeft;
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
    [HideInInspector]public List<int> slotWildArrow = new List<int>();

    public TextMeshProUGUI collectMessageText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button stopSpinningButton;

    #region others variables
    [HideInInspector] public float spinBets = 1;
    [HideInInspector] public float stakes = 0;
    /// <summary>
    /// How many wildpicks you have
    /// </summary>
    [HideInInspector] public int wildPicks = 0;
    [HideInInspector] public int numberPressed;
    /// <summary>
    /// spins left before you pay
    /// </summary>

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
    [SerializeField] public Animator MessageAnimator;
    [SerializeField] private GameObject CollectMessage;

    private bool isMessageActive = false;
    private bool warning = true;
    [HideInInspector] public bool indsatsChoosen = false;

    #endregion


    private void Awake()
    {
        spinLeft = startSpins;
        spinLeftText.text = startSpins.ToString();
        resetButton.color = Color.gray;
        resetButton.GetComponentInParent<Button>().enabled = false;

        AI = GetComponentInParent<AI>();
        blinkEffect = GetComponent<PanelEffects>();
        calculations = GetComponent<Calculations>();
        playerData = GameObject.Find("PlayerData").gameObject.GetComponent<PlayerData>();

        
        if (spinButton != null) spinButtonAnimation = spinButton.GetComponent<Animator>();
        foreach (GameObject spinSlot in slotsList)
        {
            slotTextList.Add(spinSlot.GetComponentInChildren<TextMeshProUGUI>());
            spinAnimations.Add(spinSlot.gameObject.GetComponent<Animator>());
        }
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
        numberPressed = Convert.ToInt32(gridButton.GetComponentInChildren<TextMeshProUGUI>().text);
        if (wildPicks > 0)
        {
            gridButton.GetComponent<NumberManager>().StopHighlighting(gridGeneration.numberPositions[AI.currentNumber].gameObject.transform.parent.transform.parent.gameObject);
            StartCoroutine(GameManager.Instance.WildArrowColumnAnimation(true));
        }

        StarDupping(wildNumberPicked, numberPressed);

        if (wildPicks == 0)
        {
            return;
        }

        if (slotWildArrow.Count > 0) SlotWildArrow(wildNumberPicked, numberPressed);
        else if (wilds.Count > 0) SlotSuperWild(wildNumberPicked, numberPressed);

    }

    private void StarDupping(GameObject wildNumberPicked, int numberPressed)
    {
        if (gridGeneration.numberPositions[numberPressed].hasBeenHit && wildNumberPicked && starImgs.Contains(wildNumberPicked.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>()))
        {
            Animator animator = wildNumberPicked.transform.GetChild(0).GetComponent<Animator>();
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

    public void SlotSuperWild(GameObject wildNumberPicked, int numberPressed)
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

    public void SlotWildArrow(GameObject wildNumberPicked, int numberPressed)
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

            if (slotWildArrow.Count > 0) bestChoice.gameObject.GetComponentInParent<Image>().sprite = BackgroundImages[2];
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

        if(!indsatsChoosen)
        {
            SelectSpins();
            return;
        }


        float costPrSpin = calculations.PriceCaculator();

        if (spinLeft < 0 && costPrSpin > 0 && !isMessageActive && warning)
        {
            isMessageActive = true;
            StartCoroutine(MessageHandler(keepSpinningPanel, 0, $"Vil du fors�tte med at spinne, dit n�ste spin koster {UIManager.Instance.DisplayMoney(calculations.PriceCaculator())}"));
            return;
        }
        MessageAnimator.SetBool("MinimizePlate", false);
        isSpinning = true;
        Stakes();
        ColorReset();
        StartCoroutine(Fade());

        keepSpinningPanel.SetActive(false);

        foreach (GridNumbers gridnumber in gridGeneration.numberPositions.Values)
        {
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

        if (spinLeft == startSpins)
        {
            isMessageActive = false;
            spinButton.GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(false);
            spinButton.GetComponent<Image>().color = Color.white;
            playerData.balance -= spinBets;
        }
        if (spinBuyLimit >= 0 && spinLeft <= 0 && gridCheck.slingoAnimationFinished)
        {
            resetButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
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
            if(spinLeft == 3)
            {
                blinkEffect.spinLeftText = spinLeftText;
            }
        }
        StartCoroutine(Spinner());
    }

    private void SelectSpins()
    {
        indsatsChoosen = true;
        GetComponent<SpinsValue>().ViewSpinsBets();
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
            yield return new WaitForSeconds(0.35f);
            gridGeneration.numberPositions[number].Hit();
            NumberManager numberManager = gridGeneration.numberPositions[number].gameObject.transform.parent.transform.parent.GetComponent<NumberManager>();
            numberManager.PlaySparkelEffect();


            Image starImg = gridGeneration.numberPositions[number].gameObject.transform.parent.GetChild(0).GetComponentInChildren<Image>();
            starImg.color = new Color(starImg.color.r, starImg.color.g, starImg.color.b, 0.4f);
            starImgs.Add(starImg);

            textToGoEmpty.Add(gridGeneration.numberPositions[number].gameObject.GetComponent<TextMeshProUGUI>());
            //animator on starbackground sets to bool to false to avoid bug where StarAnimation dont play
            numberManager.gameObject.transform.GetChild(0).GetComponentInChildren<Animator>().SetBool("isBestChoice", false);
        }
    }

    public void Stakes()
    {
        if (spinLeft == startSpins)
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
                if (slotWildArrow.Count > 0) bestChoice.gameObject.GetComponentInParent<Image>().sprite = BackgroundImages[2];
                else bestChoice.gameObject.GetComponentInParent<Image>().sprite = BackgroundImages[2];

                bestChoiceText = gridGeneration.numberPositions[bestChoice.number].gameObject.GetComponentInChildren<TextMeshProUGUI>();
                blinkEffect.FlashingEffect(true, bestChoice.gameObject.GetComponent<TextMeshProUGUI>());
            }
            StartCoroutine(GameManager.Instance.WildArrowColumnAnimation(false));
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
            //Besked pop op efter de f�rste 10 spins er brugt
            if (spinBuyLimit == 5 && !isMessageActive)
            {
                spinCountHeader.text = "Max spin k�b";
                spinLeftText.text = spinBuyLimit.ToString();
                

                isMessageActive = true;
                MessageAnimator.SetBool("MinimizePlate", true);
                
                //ChangeSpinButton();
                StartCoroutine(MessageHandler(CostMessage, 0f, "Du har opbrugt all dine spins og kan k�be op til 5 spins. " +
                                                                $"K�b et spin?"));
                StartCoroutine(StartButtonCostAnimation());
                return;
            }
            else if (spinBuyLimit == 0)
            {
                string messageText = "SPIL SLUT" + "\n" + "Du har tjent " + UIManager.Instance.DisplayMoney(gridCheck.rewards[gridCheck.slingoCount]);
                if (gridCheck.slingoCount >= 3)
                {
                    CollectMessage.SetActive(true);
                    collectMessageText.text = messageText;
                    CollectMessage.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>().text = "Tag Gevinst";


                }
                else
                {
                    StartCoroutine(MessageHandler(CollectMessage, 1f, messageText));
                    CollectMessage.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>().text = "N�ste Spil";
                    Debug.Log("went else");
                }
            }
            else
            {
                spinButton.GetComponent<Image>().color = Color.black;
                spinButton.GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
                spinButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                spinButton.GetComponentInChildren<TextMeshProUGUI>().text = "Pris: " + UIManager.Instance.DisplayMoney(calculations.PriceCaculator());
                resetButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
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

    private IEnumerator StartButtonCostAnimation()
    {
        yield return new WaitForSeconds(1f);
        spinButton.GetComponent<Animator>().SetBool("Cost", true);
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
    }

    public void ChangeSpinButton()
    {
        spinButton.GetComponent<Animator>().SetBool("Cost", false);
        spinButton.GetComponent<Image>().color = Color.black;
        spinButton.GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
        spinButton.GetComponentInChildren<TextMeshProUGUI>().text = "Pris: " + UIManager.Instance.DisplayMoney(calculations.PriceCaculator());
    }

    public void SpinButtonChangeFinished()
    {
        spinButton.GetComponentInChildren<ParticleSystem>().Play();
        SpinButtonReset();
        resetButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        resetButton.GetComponentInParent<Button>().enabled = true;
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
                        //animatorObject.GetComponent<Image>().sprite = BackgroundImages[1];
                        //animatorObject.GetComponent<Image>().enabled = true;
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
                        animatorObject.GetComponent<Image>().sprite = BackgroundImages[1];
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
                //wildNumberPicked.GetComponent<Image>().enabled = false;
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
