using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;


public class GridCheck : MonoBehaviour
{
    public Image resetButton;
    public Animator headerAnimator;

    [HideInInspector] public int slingoCount { get; private set; } = 0;
    [HideInInspector] public int starsCount { get; set; } = 0;
    [HideInInspector] public Dictionary<int, float> rewards{ get; private set; } = new Dictionary<int, float>();
    [HideInInspector] public bool slingoAnimationFinished = true;

    [Space(5)]
    [SerializeField] private GameObject slingoPanel;
    [SerializeField] private Sprite[] slingoBorderImages;

    [Space(5)]
    [SerializeField] private GameObject jackpotMessage;
    [SerializeField] private Sprite[] jackpotSlingoBorderImages;
    [SerializeField] private Sprite slingoBackgroundImage;

    private GridGeneration grid;
    private Dictionary<string, bool> gridSlingoList = new Dictionary<string, bool>();
    [HideInInspector] public bool slingoIsHit = false;
    private Image[] slingoBorders;
    private int rewardCount;

    // Start is called before the first frame update
    void Awake()
    {
        grid = GetComponent<GridGeneration>();
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
        UpdateRewards(1);

        if(slingoPanel != null)
        {
            slingoBorders = slingoPanel.GetComponentsInChildren<Image>().SkipLast(1).ToArray();
        }
    }

    public void UpdateRewards(float multiplyere)
    {
        rewards.Clear();
        rewards.Add(3, 1 * multiplyere);
        rewards.Add(4, 3 * multiplyere);
        rewards.Add(5, 5 * multiplyere);
        rewards.Add(6, 10 * multiplyere);
        rewards.Add(7, 25 * multiplyere);
        rewards.Add(8, 75 * multiplyere);
        rewards.Add(9, 150 * multiplyere);
        rewards.Add(10, 500 * multiplyere);
        rewards.Add(11, 500 * multiplyere);
        rewards.Add(12, 1000 * multiplyere);

        GameObject[] slingoRewards = GameObject.FindGameObjectsWithTag("SlingoReward").OrderBy(go => go.transform.position.y).ToArray();

        for (int i = 0; i < slingoRewards.Length; i++)
        {
            if (rewards.ContainsKey(i + 3) && i < 7)
            {
                slingoRewards[i].GetComponent<TextMeshProUGUI>().text = UIManager.Instance.DisplayMoney(rewards[i + 3]);
            }
        }
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

        try
        {
            foreach (Image item in slingoBorders)
            {
                if (item.sprite != slingoBorderImages[0] && item != slingoBorders[slingoBorders.Length - 1])
                {
                    item.sprite = slingoBorderImages[0];
                }
                else if (item == slingoBorders[slingoBorders.Length - 1])
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

    Dictionary<string, int> slingoTypes = new Dictionary<string, int>();

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
        foreach (GridNumbers number in grid.numberPositions.Values)
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
                    //StartCoroutine(SlingoAnimation(PlaySlingoAnimation("h", number.h)));
                    break;
                }
            }
        }


        foreach (GridNumbers number in grid.numberPositions.Values)
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
                    //StartCoroutine(SlingoAnimation(PlaySlingoAnimation("v", number.v)));
                    break;
                }
            }
        }


        if (diagonal)
        {
            if(!gridSlingoList["dl"])
            {

                foreach (GridNumbers number in grid.numberPositions.Values)
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
                            //StartCoroutine(SlingoAnimation(PlaySlingoAnimation("l", 0)));
                            break;
                        }
                    }
                }
            }

            if (!gridSlingoList["dr"])
            {
                foreach (GridNumbers number in grid.numberPositions.Values)
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
                            //StartCoroutine(SlingoAnimation(PlaySlingoAnimation("r", 0)));
                            break;
                        }
                    }
                }
            }
        }

        if ((rightIndex == 5 || leftIndex == 5 || vertIndex == 5 || horIndex == 5) && !check)
        {
            StartCoroutine(SlingoAnimation(PlaySlingoAnimation(slingoTypes)));
            slingoTypes.Clear();
        }
    }

    /// <summary>
    /// Checks if the new slingo gives a new reward
    /// </summary>
    private void CheckForReward()
    {
        if (rewards.ContainsKey(slingoCount))
        {
            if(slingoCount == 12)
            {
                slingoBorders[slingoBorders.Length - 1].sprite = jackpotSlingoBorderImages[1];
            }

            foreach (Image item in slingoBorders)
            {
                if (item.sprite != slingoBorderImages[1] && item != slingoBorders[slingoBorders.Length - 1])
                {
                    item.sprite = slingoBorderImages[1];
                    break;
                }
            }
        }
        if (slingoCount >= 3)
        {
            resetButton.GetComponentInChildren<TextMeshProUGUI>().text = "Collect " + UIManager.Instance.DisplayMoney(rewards[slingoCount]);
        }
        if(slingoCount >= 12 && slingoAnimationFinished)
        {
            jackpotMessage.SetActive(true);
        }

    }

    /// <summary>
    /// Get max amount of slingos by one number to use with cost of extra spins
    /// </summary>
    /// <returns>Returns max number of possible slingo by getting one more number</returns>
    public int CheckForMaxReward()
    {
        int maxReward = 0;
        foreach (GridNumbers number in grid.numberPositions.Values)
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
                foreach (GridNumbers numbers in grid.numberPositions.Values)
                {
                    if (numbers.h == slingoTypes[slingoType])
                    {
                        numbersInSlingo.Add(numbers.gameObject);
                    }
                }

            }
            else if (slingoType == "v")
            {
                foreach (GridNumbers numbers in grid.numberPositions.Values)
                {
                    if (numbers.v == slingoTypes[slingoType])
                    {
                        numbersInSlingo.Add(numbers.gameObject);
                    }
                }
            }
            else if (slingoType == "l")
            {
                foreach (GridNumbers numbers in grid.numberPositions.Values)
                {
                    if (numbers.diagonal && numbers.h == numbers.v)
                    {
                        numbersInSlingo.Add(numbers.gameObject);
                    }
                }
            }
            else
            {
                foreach (GridNumbers numbers in grid.numberPositions.Values)
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
        yield return new WaitForSeconds(0.1f);

        foreach (GameObject go in slingoNumbers)
        {
            go.GetComponent<TextMeshProUGUI>().text = "";
            Image wallImage = go.GetComponentInChildren<Image>();
            wallImage.sprite = slingoBackgroundImage;
            wallImage.transform.GetChild(0).GetComponent<Image>().enabled = false;
            wallImage.transform.GetChild(1).GetComponent<Image>().enabled = true;
            wallImage.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            wallImage.enabled = true;

            if (go.GetComponentInChildren<Animator>().GetBool("Slingo"))
            {
                go.GetComponentInChildren<Animator>().Play("Base Layer.SlingoAnimation", -1, 0);
            }
            else
            {
                go.GetComponentInChildren<Animator>().SetBool("Slingo", true);
            }

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.4f);
        headerAnimator.SetBool("isTwerking", false);
        slingoAnimationFinished = true;
    }
}