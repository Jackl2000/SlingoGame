using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingLevel : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public Animator sceneAnimator;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerStats.Instance.Level != 5) levelText.text = "LEVEL " + PlayerStats.Instance.Level;
        else levelText.text = "FINAL BOSS";
        StartCoroutine(PlaySceneTransistion());
    }

    private IEnumerator PlaySceneTransistion()
    {
        yield return new WaitForSeconds(1.5f);
        sceneAnimator.SetBool("Loading", true);
    }

    public IEnumerator LoadScene()
    {
        yield return SceneManager.LoadSceneAsync("AdventureCombat");
    }
}
