using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridNumbers
{
    public GameObject gameObject;

    public Image starBackgroundImg;
    private Animator starAnimation;

    public int number { get; private set; }
    public int h { get; private set; }
    public int v { get; private set; }
    public bool diagonal { get; private set; }
    public bool hasBeenHit { get; set; } = false;

    public GridNumbers(int horizontal, int vertical, GameObject gameObject, int value)
    {
        this.gameObject = gameObject;
        starBackgroundImg = gameObject.GetComponentInParent<Image>();
        if(starBackgroundImg != null ) starAnimation = starBackgroundImg.GetComponent<Animator>();

        number = value;
        h = horizontal;
        v = vertical;
        diagonal = CheckForDiagonal();
    }

    private bool CheckForDiagonal()
    {
        if ((h == v) || (h == 5 && v == 1) || (h == 4 && v == 2) || (h == 2 && v == 4) || h == 1 && v == 5)
        {
            return true;
        }
        else return false;
    }


    public void Hit()
    {
        if (hasBeenHit) return;
        hasBeenHit = true;
        try
        {
            gameObject.GetComponentInParent<GridCheck>().starsCount++;
            gameObject.GetComponentInParent<GridCheck>().CheckGrid(h, v, diagonal, false);
            gameObject.GetComponent<TextMeshProUGUI>().color = Color.black;

            starAnimation.SetBool("Hit", true);
            starBackgroundImg.transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("No child image");
        }
    }

    public void ResetData()
    {
        try
        {
            
            starBackgroundImg.GetComponentInChildren<Image>().enabled = false;
            starBackgroundImg.transform.GetChild(0).GetComponent<Image>().enabled = false;
            starBackgroundImg.transform.GetChild(1).GetComponent<Image>().enabled = false;
            starAnimation.SetBool("Hit", false);
            starAnimation.SetBool("Slingo", false);
            starAnimation.SetBool("Duppe", false);
        }
        catch (System.Exception)
        {
            Debug.Log("No child image");
        }
        
        hasBeenHit = false;
    }
}