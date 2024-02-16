using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour
{
    public List<GameObject> dontDestroyObjects;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        foreach (GameObject obj in dontDestroyObjects)
        {
            DontDestroyOnLoad(obj);
        }
    }
}
