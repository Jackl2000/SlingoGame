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

    public void EnterBonusGame(bool isBonusGameHit)
    {
        animator.SetBool("IsBonusGameHit", isBonusGameHit);
        cardGameManager.ShuffleCards();
    }

}
