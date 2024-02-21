using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour
{
    public Animator animator;

    public float transitionTime = 1;

    public List<GameObject> dontDestroyObjects;
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
            LoadScene(1);
            Debug.Log(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void LoadScene(int index)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(TriggerSceneLoad(index));
        }
        else
        {
            StartCoroutine(TriggerSceneLoad(0));
        }
    }

    IEnumerator TriggerSceneLoad(int sceneIndex)
    {
        animator.SetTrigger("EnterScene");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);

        yield return new WaitForSeconds(5f);
        animator.SetBool("IsLoading", false);
    }
}
