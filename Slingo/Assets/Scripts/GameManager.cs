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
    }

    private void Update()
    {
        if (gridCheck.slingoCount == 12 && gridCheck.slingoAnimationFinished && !SceneSwap.Instance.gameObject.GetComponentInChildren<Animator>().GetBool("IsLoading"))
        {
            SceneSwap.Instance.LoadScene(1);
            SceneSwap.Instance.gameObject.GetComponentInChildren<Animator>().SetBool("IsLoading", true);
            //animator.SetBool("IsLoading", true);
        }
    }

    public void BonusGameSwitch(int sceneIndex)
    {
        sceneSwap.LoadScene(sceneIndex);
    }

}
