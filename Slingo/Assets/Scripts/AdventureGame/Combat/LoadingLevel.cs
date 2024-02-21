using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        Debug.Log("Level is currently being loaded");
        yield return SceneManager.LoadSceneAsync("AdventureCombat");
        Debug.Log("Level loaded");
    }
}
