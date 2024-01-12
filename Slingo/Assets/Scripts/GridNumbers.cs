using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridNumbers
{
    public int h { get; private set; }
    public int v { get; private set; }
    public bool hasBeenHit { get; private set; } = false;
    public GameObject gameObject;
    private bool diagonal;
    private Image childImage;

    public GridNumbers(int horizontal, int vertical, GameObject gameObject)
    {
        h = horizontal;
        v = vertical;
        this.gameObject = gameObject;
        childImage = gameObject.GetComponentInChildren<Image>();
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


    public void Hit(bool joker)
    {
        if (hasBeenHit) return;
        hasBeenHit = true;
        gameObject.GetComponentInParent<GridCheck>().CheckGrid(h, v, diagonal);
        gameObject.GetComponent<TextMeshProUGUI>().text = string.Empty;
        if(!joker)
        {
            childImage.GetComponentInChildren<Image>().enabled = true;
            childImage.transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
        else
        {
            childImage.transform.GetChild(1).GetComponent<Image>().enabled = true;
        }

    }

    public void ResetData()
    {
        childImage.GetComponentInChildren<Image>().enabled = false;
        childImage.transform.GetChild(0).GetComponent<Image>().enabled = false;
        childImage.transform.GetChild(1).GetComponent<Image>().enabled = false;
        hasBeenHit = false;
    }
}