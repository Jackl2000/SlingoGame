using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridNumbers
{
    private int h;
    private int v;
    private GameObject gameObject;

    public GridNumbers(int horizontal, int vertical, GameObject gameObject)
    {
        h = horizontal;
        v = vertical;
        this.gameObject = gameObject;
    }

    public void Hit()
    {
        gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;
    }
}
