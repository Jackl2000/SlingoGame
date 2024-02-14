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

    
}
