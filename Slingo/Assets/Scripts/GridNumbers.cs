using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridNumbers
{
    public int h { get; private set; }
    public int v { get; private set; }
    public bool hasBeenHit { get; set; } = false;
    public GameObject gameObject;
    public bool diagonal { get; private set; }
    public int number { get; private set; }
    private Image childImage;
    private Animator starAnimation;

    public GridNumbers(int horizontal, int vertical, GameObject gameObject, int value)
    {
        h = horizontal;
        v = vertical;
        this.gameObject = gameObject;
        number = value;
        childImage = gameObject.GetComponentInParent<Image>();
        if(childImage != null ) starAnimation = childImage.GetComponent<Animator>();

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
        if(h == 5 && v == 5)
        {

        }
        hasBeenHit = true;
        try
        {
            gameObject.GetComponentInParent<GridCheck>().starsCount++;
            gameObject.GetComponentInParent<GridCheck>().CheckGrid(h, v, diagonal, false);
            gameObject.GetComponent<TextMeshProUGUI>().color = Color.black;

            starAnimation.SetBool("Hit", true);
            childImage.transform.GetChild(0).GetComponent<Image>().enabled = true;
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
            childImage.GetComponentInChildren<Image>().enabled = false;
            childImage.transform.GetChild(0).GetComponent<Image>().enabled = false;
            childImage.transform.GetChild(1).GetComponent<Image>().enabled = false;
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