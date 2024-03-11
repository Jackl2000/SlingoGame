using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour
{
    public Animator animator;

    public float transitionTime = 1;

    public static SceneSwap Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SceneSwitch("BonusSpil_CardFlip");
            Debug.Log(SceneManager.GetActiveScene().buildIndex);
        }
    }


    public void SceneSwitch(string sceneName)
    {
        StartCoroutine(TriggerSceneLoad(sceneName));
        Debug.Log("Scene: " + sceneName);
    }


    IEnumerator TriggerSceneLoad(string sceneName)
    {
        animator.SetTrigger("EnterScene");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(5f);
        animator.SetBool("IsLoading", false);
    }
}
