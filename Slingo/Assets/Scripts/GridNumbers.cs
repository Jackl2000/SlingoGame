using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridNumbers
{
    public int h { get; private set; }
    public int v { get; private set; }
    public bool hasBeenHit { get; private set; } = false;
    private GameObject gameObject;
    private bool diagonal;

    public GridNumbers(int horizontal, int vertical, GameObject gameObject)
    {
        h = horizontal;
        v = vertical;
        this.gameObject = gameObject;
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
        gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;
        hasBeenHit = true;
        gameObject.GetComponentInParent<GridCheck>().CheckGrid(h, v, diagonal);
    }
}