using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour
{
    public Animator animator;

    public float transitionTime = 1;

    public List<GameObject> dontDestroyObjects;

    public static SceneSwap instance;
    public static SceneSwap Instance { get; private set; }

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            
            DontDestroyOnLoad(this.gameObject.transform.parent.gameObject);

            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                LoadScene(0);
            }
            Debug.Log(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void LoadScene(int sceneIndex)
    {        
        StartCoroutine(TriggerSceneLoad(sceneIndex));
    }

    IEnumerator TriggerSceneLoad(int sceneIndex)
    {
        animator.SetTrigger("EnterScene");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }
}
