using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarManager : MonoBehaviour
{
    public spin spinScript;

    public GameObject starObj;

    private ParticleSystem starParticle;
    private Image starImg;
    private Image starWildImg;

    private void Awake()
    {
        starParticle = GetComponentInChildren<ParticleSystem>();
        //starImg = this.transform.GetChild(0).GetComponent<Image>();
        //starWildImg = this.transform.GetChild(1).GetComponent<Image>();

    }
    
    public void PlaySparkelEffect()
    {
        starParticle.Play();
    }

    public void PlayHighlighter()
    {
        this.GetComponentInChildren<Animator>().SetBool("isBestChoice", true);
        this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
    }

    public void StopHighlighting(GameObject numberObj)
    {
        numberObj.GetComponentInChildren<Animator>().SetBool("isBestChoice", false);
        numberObj.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
    }

    
}
