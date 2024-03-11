using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberManager : MonoBehaviour
{
    public spin spinScript;
    public Animator starBackgroundAnimator;

    private Animator numberAnimator;

    private ParticleSystem starParticle;
    private Image starImg;
    private Image starWildImg;

    private void Awake()
    {
        starParticle = GetComponentInChildren<ParticleSystem>();
        numberAnimator = GetComponent<Animator>();  

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(WildAnimationHighLighter());
        }
    }

    public void PlaySparkelEffect()
    {
        starParticle.Play();
    }

    public void PlayHighlighterDot()
    {
        starBackgroundAnimator.SetBool("isBestChoice", true);
        this.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
    }

    public void StopHighlighting(GameObject numberObj)
    {
        starBackgroundAnimator.SetBool("isBestChoice", false);
        numberObj.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
    }

    IEnumerator WildAnimationHighLighter()
    {
        string name = transform.parent.gameObject.name;
        if (name.Substring(5) == spinScript.slotsList[0].name.Substring(3) 
            )
        {
            Debug.Log("P pressed");
            numberAnimator.SetBool("isArrowWildHit", true);
            yield return new WaitForSeconds(0.8f);
            numberAnimator.SetBool("isArrowWildHit", false);
        }

    }

    
}
