using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GridCheck : MonoBehaviour
{
    //public Image resetButton;
    public Animator headerAnimator;
    public GameObject SlingoPanel;
    public Button slingoRewardButton;
    private spin spinScript;

    [HideInInspector] public int slingoCount { get; private set; } = 0;
    [HideInInspector] public int starsCount { get; set; } = 0;
    [HideInInspector] public Dictionary<int, float> rewards{ get; private set; } = new Dictionary<int, float>();
    [HideInInspector] public bool slingoAnimationFinished = true;

    [Space(5)]
    [SerializeField] private Sprite[] slingoBorderImages;

    [Space(5)]
    //[SerializeField] private GameObject jackpotMessage;
    [SerializeField] private Sprite[] jackpotSlingoBorderImages;
    [SerializeField] private Sprite slingoBackgroundImage;

    private GridGeneration gridGeneration;
    private Dictionary<string, bool> gridSlingoList = new Dictionary<string, bool>();
    [HideInInspector] public bool slingoIsHit = false;
    private List<Image> slingoBorders = new List<Image>();
    private int rewardCount;
    private Dictionary<string, int> slingoTypes = new Dictionary<string, int>();

    private float currentBet = 1;
    private int slingoAnimations = 0;

    // Start is called before the first frame update
    void Awake()
    {
        spinScript = this.GetComponentInChildren<spin>();

        gridGeneration = GetComponent<GridGeneration>();
        gridSlingoList.Add("h1", false);
        gridSlingoList.Add("h2", false);
        gridSlingoList.Add("h3", false);
        gridSlingoList.Add("h4", false);
        gridSlingoList.Add("h5", false);
        gridSlingoList.Add("v1", false);
        gridSlingoList.Add("v2", false);
        gridSlingoList.Add("v3", false);
        gridSlingoList.Add("v4", false);
        gridSlingoList.Add("v5", false);
        gridSlingoList.Add("dl", false);
        gridSlingoList.Add("dr", false);
        UpdateRewards(null, 1);
        

    }

    public void UpdateRewards(List<GameObject> slingoRewards, float multiplyere)
    {
        rewards.Clear();
        rewards.Add(0, 0);
        rewards.Add(1, 0);
        rewards.Add(2, 0);
        rewards.Add(3, 1 * multiplyere);
        rewards.Add(4, 2 * multiplyere);
        rewards.Add(5, 4 * multiplyere);
        rewards.Add(6, 6 * multiplyere);
        rewards.Add(7, 10 * multiplyere);
        rewards.Add(8, 12 * multiplyere);
        rewards.Add(9, 15 * multiplyere);
        if (!GameManager.Instance.BonusGameEnable)
        {
            rewards.Add(10, 20 * multiplyere);
            rewards.Add(11, 20 * multiplyere);
            rewards.Add(12, 30 * multiplyere);
        }
        else
        {
            rewards.Add(10, 15 * multiplyere);
            rewards.Add(11, 15 * multiplyere);
            rewards.Add(12, 15 * multiplyere);
        }

        if (slingoRewards != null)
        {
            for (int i = 0; i < slingoRewards.Count(); i++)
            {
                if (rewards.ContainsKey(i + 3) && i < 7)
                {
                    slingoRewards[i].GetComponent<TextMeshProUGUI>().text = UIManager.Instance.DisplayMoney(rewards[i + 3]);
                }
            }
        }

        if(slingoCount == 0) slingoRewardButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = UIManager.Instance.DisplayMoney(rewards[3]);
        else if(slingoCount >= 9) slingoRewardButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = UIManager.Instance.DisplayMoney(rewards[3]);
    }

    /// <summary>
    /// Reset all the data about the grid like slingo count and etc.
    /// </summary>
    public void ResetGrid()
    {
        foreach(string item in gridSlingoList.Keys.ToList())
        {
            gridSlingoList[item] = false;
        }
        slingoCount = 0;
        starsCount = 0;

        slingoRewardButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = UIManager.Instance.DisplayMoney(1);
        slingoRewardButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "3 rækker";

        try
        {
            foreach (Image item in slingoBorders)
            {
                if (item.sprite != slingoBorderImages[0] && item != slingoBorders[slingoBorders.Count - 1])
                {
                    item.sprite = slingoBorderImages[0];
                }
                else if (item == slingoBorders[slingoBorders.Count - 1])
                {
                    item.sprite = jackpotSlingoBorderImages[0];
                }
                else break;
            }
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e.Message);
        }

    }

    /// <summary>
    /// Checks for new slingo with new number
    /// </summary>
    public void CheckGrid(int h, int v, bool diagonal, bool check)
    {
        int horIndex = 0;
        int vertIndex = 0;
        int leftIndex = 0;
        int rightIndex = 0;
        rewardCount = 0;
        foreach (GridNumbers number in gridGeneration.numberPositions.Values)
        {
            if (number.h == h)
            {
                
                if (!number.hasBeenHit)
                {
                    break;
                }
                horIndex++;

                if (horIndex == 5 && !gridSlingoList["h" + h])
                {
                    if(check)
                    {
                        rewardCount++;
                        break;
                    }
                    gridSlingoList["h" + h] = true;
                    slingoIsHit = true;
                    slingoCount++;
                    CheckForReward();
                    slingoTypes.Add("h", number.h);
                    break;
                }
            }
        }


        foreach (GridNumbers number in gridGeneration.numberPositions.Values)
        {
            if (number.v == v)
            {
                
                if (!number.hasBeenHit)
                {
                    break;
                }
                vertIndex++;

                if (vertIndex == 5 && !gridSlingoList["v" + v])
                {
                    if (check)
                    {
                        rewardCount++;
                        break;
                    }
                    gridSlingoList["v" + v] = true;
                    slingoIsHit = true;
                    slingoCount++;
                    CheckForReward();
                    slingoTypes.Add("v", number.v);
                    break;
                }
            }
        }


        if (diagonal)
        {
            if(!gridSlingoList["dl"])
            {

                foreach (GridNumbers number in gridGeneration.numberPositions.Values)
                {
                    if (number.h == number.v)
                    {
                        if (!number.hasBeenHit)
                        {
                            break;
                        }
                        leftIndex++;
                        if (leftIndex == 5)
                        {
                            if (check)
                            {
                                rewardCount++;
                                break;
                            }
                            gridSlingoList["dl"] = true;
                            slingoIsHit = true;
                            slingoCount++;
                            CheckForReward();
                            slingoTypes.Add("l", 0);
                            break;
                        }
                    }
                }
            }

            if (!gridSlingoList["dr"])
            {
                foreach (GridNumbers number in gridGeneration.numberPositions.Values)
                {
                    if ((number.h == 5 && number.v == 1) || (number.h == 4 && number.v == 2) || (number.h == 3 && number.v == 3) || (number.h == 2 && number.v == 4) || (number.h == 1 && number.v == 5))
                    {

                        if (!number.hasBeenHit)
                        {
                            break;
                        }
                        rightIndex++;
                        if (rightIndex == 5 && !gridSlingoList["dr"])
                        {
                            if (check)
                            {
                                rewardCount++;
                                break;
                            }
                            gridSlingoList["dr"] = true;
                            slingoIsHit = true;
                            slingoCount++;
                            CheckForReward();
                            slingoTypes.Add("r", 0);
                            break;
                        }
                    }
                }
            }
        }

        if ((rightIndex == 5 || leftIndex == 5 || vertIndex == 5 || horIndex == 5) && !check)
        {
            slingoAnimations++;
            StartCoroutine(SlingoAnimation(PlaySlingoAnimation(slingoTypes)));
            slingoTypes.Clear();
            StartCoroutine(UpdateButton());
        }
    }

    private IEnumerator UpdateButton()
    {
        if(slingoCount >= 9)
        {
            StartCoroutine(UIManager.Instance.TextColorAnimation(this, slingoRewardButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), 0.04f, new Color[4] { Color.red, Color.green, Color.blue, Color.yellow }, Color.white, 2));
            StartCoroutine(UIManager.Instance.TextColorAnimation(this, slingoRewardButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>(), 0.02f, new Color[4] { Color.red, Color.green, Color.blue, Color.yellow }, Color.white, 2));
            slingoRewardButton.GetComponent<MoneyEffect>().playAnimation = true;
            slingoRewardButton.GetComponent<Animator>().SetBool("Slingo", true);
            yield return new WaitForSeconds(0.3f);
            if(slingoCount == 9)
            {
                slingoRewardButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "JACKPOT FLASH";
                slingoRewardButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "10 rækker";
            }
            else if(slingoCount == 10)
            {
                slingoRewardButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "SUPER JACKPOT FLASH";
                slingoRewardButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "BIGGEST PRICE!";
            }

            yield return new WaitForSeconds(0.7f);
            slingoRewardButton.GetComponent<Animator>().SetBool("Slingo", false);
        }
        else if(slingoCount >= 3)
        {
            StartCoroutine(UIManager.Instance.TextColorAnimation(this, slingoRewardButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), 0.04f, new Color[4] { Color.red, Color.green, Color.blue, Color.yellow }, Color.white, 2));
            StartCoroutine(UIManager.Instance.TextColorAnimation(this, slingoRewardButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>(), 0.02f, new Color[4] { Color.red, Color.green, Color.blue, Color.yellow }, Color.white, 2));
            slingoRewardButton.GetComponent<MoneyEffect>().playAnimation = true;
            slingoRewardButton.GetComponent<Animator>().SetBool("Slingo", true);
            yield return new WaitForSeconds(0.3f);
            slingoRewardButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = UIManager.Instance.DisplayMoney(rewards[slingoCount + 1]);
            slingoRewardButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (slingoCount + 1).ToString() + " rækker";
            yield return new WaitForSeconds(0.7f);
            slingoRewardButton.GetComponent<Animator>().SetBool("Slingo", false);
        }
    }
    /// <summary>
    /// Checks if the new slingo gives a new reward
    /// </summary>
    private void CheckForReward()
    {
        if(slingoBorders.Count > 0 && SlingoPanel.activeSelf)
        {
            if (rewards.ContainsKey(slingoCount) && slingoCount != 0)
            {
                foreach (Image item in slingoBorders)
                {
                    if (item.sprite == slingoBorders.Last())
                    {
                        break;
                    }
                    item.sprite = slingoBorderImages[1];
                    if(slingoCount == 12)
                    {
                        foreach(Image imageItem in slingoBorders)
                        {
                            if (imageItem.sprite == slingoBorders.Last()) break;
                            imageItem.sprite = slingoBorderImages[1];
                        }
                        break;
                    }
                    else if (item.sprite == slingoBorders[slingoCount - 1].sprite)
                    {
                        break;
                    }
                }

                if (slingoCount == 12)
                {
                    slingoBorders[slingoBorders.Count - 1].sprite = jackpotSlingoBorderImages[1];
                }
            }
        }
    }

    /// <summary>
    /// Get max amount of slingos by one number to use with cost of extra spins
    /// </summary>
    /// <returns>Returns max number of possible slingo by getting one more number</returns>
    public int CheckForMaxReward()
    {
        int maxReward = 0;
        foreach (GridNumbers number in gridGeneration.numberPositions.Values)
        {
            if(!number.hasBeenHit)
            {
                number.hasBeenHit = true;
                CheckGrid(number.h, number.v, number.diagonal, true);
                number.hasBeenHit = false;
                if (rewardCount > maxReward)
                {
                    maxReward = rewardCount;
                }
            }
        }
        return maxReward;
    }

    public void SlingoBorderGoIdle()
    {
        SlingoPanel.GetComponent<Animator>().SetBool("Start", false);
        SlingoPanel.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
        SlingoPanel.SetActive(false);
    }

    public void ViewSlingoRewards()
    {
        List<GameObject> slingoRewards = new List<GameObject>();

        if(!SlingoPanel.gameObject.activeSelf)
        {
            SlingoPanel.SetActive(true);
            SlingoPanel.GetComponent<Animator>().SetBool("Show", true);

            if (slingoBorders.Count == 0)
            {
                GameObject[] borders = GameObject.FindGameObjectsWithTag("SlingoBoarder").OrderBy(x => x.transform.parent.position.y).ToArray();

                foreach (GameObject go in borders)
                {
                    slingoBorders.Add(go.GetComponent<Image>());
                }
            }
            GameObject[] slingoRewardsArray = GameObject.FindGameObjectsWithTag("SlingoReward").OrderBy(x => x.transform.parent.position.y).ToArray();
            slingoRewards.AddRange(slingoRewardsArray);
            if (spinScript.spinBets != currentBet && slingoRewards != null)
            {

                currentBet = spinScript.spinBets;
                UpdateRewards(slingoRewards, currentBet);
            }
            CheckForReward();
        }
        else
        {
            SlingoPanel.GetComponent<Animator>().SetBool("Show", false);
            StartCoroutine(SlingoPanelDelay());
        }
    }

    private IEnumerator SlingoPanelDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SlingoPanel.SetActive(false);
    }

    /// <summary>
    /// Returns a list of game objects to play the slingo animation on
    /// </summary>
    private List<GameObject> PlaySlingoAnimation(Dictionary<string, int> slingoTypes)
    {
        List<GameObject> numbersInSlingo = new List<GameObject>();
        foreach (string slingoType in slingoTypes.Keys)
        {
            if (slingoType == "h")
            {
                foreach (GridNumbers numbers in gridGeneration.numberPositions.Values)
                {
                    if (numbers.h == slingoTypes[slingoType])
                    {
                        numbersInSlingo.Add(numbers.gameObject);
                    }
                }

            }
            else if (slingoType == "v")
            {
                foreach (GridNumbers numbers in gridGeneration.numberPositions.Values)
                {
                    if (numbers.v == slingoTypes[slingoType])
                    {
                        numbersInSlingo.Add(numbers.gameObject);
                    }
                }
            }
            else if (slingoType == "l")
            {
                foreach (GridNumbers numbers in gridGeneration.numberPositions.Values)
                {
                    if (numbers.diagonal && numbers.h == numbers.v)
                    {
                        numbersInSlingo.Add(numbers.gameObject);
                    }
                }
            }
            else
            {
                foreach (GridNumbers numbers in gridGeneration.numberPositions.Values)
                {
                    if ((numbers.h == numbers.v && numbers.h == 3) || (numbers.h == 5 && numbers.v == 1) || (numbers.h == 4 && numbers.v == 2) || (numbers.h == 2 && numbers.v == 4) || numbers.h == 1 && numbers.v == 5)
                    {
                        numbersInSlingo.Add(numbers.gameObject);
                    }
                }
            }
        }
        
        return numbersInSlingo;
    }

    /// <summary>
    /// Plays the slingo animation on the list of game object
    /// </summary>
    private IEnumerator SlingoAnimation(List<GameObject> slingoNumbers)
    {
        slingoAnimationFinished = false;
        headerAnimator.SetBool("isTwerking", true);
        yield return new WaitForSeconds(0.5f);

        foreach (GameObject go in slingoNumbers)
        {
            go.GetComponent<TextMeshProUGUI>().text = "";
            Image starBackgroundImg = go.GetComponentInParent<Image>();
            starBackgroundImg.sprite = slingoBackgroundImage;
            starBackgroundImg.transform.GetChild(0).GetComponent<Image>().enabled = false;
            starBackgroundImg.transform.GetChild(1).GetComponent<Image>().enabled = true;
            starBackgroundImg.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            starBackgroundImg.enabled = true;
            starBackgroundImg.transform.parent.GetComponent<NumberManager>().PlaySparkelEffect();
            

            if (go.GetComponentInParent<Animator>().GetBool("Slingo"))
            {
                go.GetComponentInParent<Animator>().Play("Base Layer.SlingoAnimation", -1, 0);
            }
            else
            {
                go.GetComponentInParent<Animator>().SetBool("Slingo", true);
            }

            yield return new WaitForSeconds(0.2f);
        }
        

        headerAnimator.SetBool("isTwerking", false);
        slingoAnimations--;
        if (slingoAnimations == 0)
        {
            slingoAnimationFinished = true;
            if (spinScript.wildPicks == 0 || spinScript.wildPicks == spinScript.wildPicked)
            {
                spinScript.SpinButtonReset();
            }
        }
    }
}