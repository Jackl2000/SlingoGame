using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    GridCheck gridCheck;
    [Header("References")]
    public spin spinScript;
    public GridGeneration gridGeneration;
    public AI aiScript;
    public CollectReward collectReward;

    [Space(7)]
    [Header("Message settings")]
    public Toggle toggle;
    public GameObject messageTipObject;
    public bool isFirstRun = true;


    public int runs = 5;

    public List<GameObject> Columns = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        gridCheck = GetComponent<GridCheck>();
    }

    private void Update()
    {
        BonusGameHit();
    }

    int toReversCount;
    public IEnumerator WildArrowColumnAnimation(bool hasPicked)
    {
        List<Animator> numbersAnimator = new List<Animator>();
        List<Animator> reversedAnimatorList = new List<Animator>();

        foreach (GridNumbers number in gridGeneration.numberPositions.Values) 
        {
            if (spinScript.slotWildArrow.Contains(number.h))
            {
                toReversCount++;
                if (!number.hasBeenHit)
                {
                    numbersAnimator.Add(number.gameObject.transform.parent.parent.GetComponent<Animator>());
                }
            }
            if (toReversCount >= 5)
            {
                numbersAnimator.Reverse();
                reversedAnimatorList.AddRange(numbersAnimator);
                numbersAnimator.Clear();
                toReversCount = 0;
            }
        }
        if (!hasPicked)
        {
            foreach (Animator animator in reversedAnimatorList)
            {
                animator.gameObject.GetComponentInChildren<Image>().enabled = true;
                animator.SetTrigger("BeatTrigger");

                yield return new WaitForSeconds(0.2f);

                animator.SetBool("HasPicked", hasPicked);
            }
        }
        //Stops animation
        if (hasPicked && gridGeneration.numberPositions[aiScript.currentNumber].h == gridGeneration.numberPositions[spinScript.numberPressed].h)
        {
            foreach (Animator animator in reversedAnimatorList)
            {
                animator.SetBool("HasPicked", hasPicked);
            }
        }

    }

    private void BonusGameHit()
    {
        if (gridCheck.slingoCount == 12 && gridCheck.slingoAnimationFinished && !SceneSwap.Instance.gameObject.GetComponentInChildren<Animator>().GetBool("IsLoading"))
        {
            SceneSwap.Instance.SceneSwitch(1);
            SceneSwap.Instance.gameObject.GetComponentInChildren<Animator>().SetBool("IsLoading", true);
        }
    }

    public void BonusGameSwitch(int sceneIndex)
    {
        SceneSwap.Instance.SceneSwitch(sceneIndex);
    }

    bool wHasBeenTipped = false;
    bool sHasBeenTipped = false;

    public void TipMessage()
    {
        if (spinScript.wildsArrow.Count > 0 && wHasBeenTipped == false) // arrow wild appeared set text
        {
            messageTipObject.SetActive(true);
            messageTipObject.GetComponentInChildren<TextMeshProUGUI>().text = "Placere en stjerne på en fri plads i det fremhævet række";
            wHasBeenTipped=true;
        }
        if (spinScript.wilds.Count > 0 && sHasBeenTipped == false && spinScript.wildsArrow.Count == 0) //super wild appeared set text 
        {
            messageTipObject.SetActive(true);
            messageTipObject.GetComponentInChildren<TextMeshProUGUI>().text = "Placere en stjerne på en fri plads hvor som helst på pladen";
            sHasBeenTipped=true;
        }
    }

    private void SlingoSimulator()
    {
        if (runs >= 0)
        {
            spinScript.indsatsChoosen = true;
            spinScript.StartSpin();
            if (toggle)
            {
                toggle.isOn = true;
                spinScript.HideMessage(toggle);
            }

            if (spinScript.wildPicks > 0 && aiScript.currentNumber != 0)
            {
                spinScript.WildPick(gridGeneration.numberPositions[aiScript.currentNumber].gameObject.transform.parent.transform.parent.gameObject.GetComponent<Button>());
            }
        }
        if (spinScript.spinBuyLimit <= 0)
        {
            collectReward.Collect();
            runs--;
        }
    }

    //IEnumerator SlingoSimulator()
    //{
    //    yield return new WaitForSeconds(1f);
    //}
   
    //Spins i alt
    //Slingo i alt
    //Indsats i alt
    //Gevints i alt
}
