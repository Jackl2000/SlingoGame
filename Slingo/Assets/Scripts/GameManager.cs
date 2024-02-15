using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    GridCheck gridCheck;
    Animator animator;
    public CardGameManager cardGameManager;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        gridCheck = GetComponent<GridCheck>();
    }

    private void Update()
    {
        if (gridCheck.slingoCount == 12)
        {
            LoadScene("BonusSpil_CardFlip");
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void BonusGameSwitch()
    {
        if (!animator.GetBool("IsBonusGameHit")) 
        {
            animator.SetBool("IsBonusGameHit", true);
            cardGameManager.ShuffleCards();
        }
        else
        {
            animator.SetBool("IsBonusGameHit", false);
        }

    }

}
