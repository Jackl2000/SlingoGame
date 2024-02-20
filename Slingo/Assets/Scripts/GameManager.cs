using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    GridCheck gridCheck;
    Animator animator;
    public CardGameManager cardGameManager;
    public SceneSwap sceneSwap;
    public GameObject prefab_CanvasLoader;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gridCheck = GetComponent<GridCheck>();
        sceneSwap = GameObject.Find("CanvasLoader").GetComponent<SceneSwap>();
    }

    private void Update()
    {
        if (gridCheck.slingoCount == 12 && gridCheck.slingoAnimationFinished && !animator.GetBool("IsLoading"))
        {
            animator.SetBool("IsLoading", true);
            sceneSwap.LoadScene(1);
        }
    }

    public void BonusGameSwitch(int sceneIndex)
    {
        sceneSwap.LoadScene(sceneIndex);
    }

}
