using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCheck : MonoBehaviour
{
    private GridGeneration grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<GridGeneration>();
    }

    public void CheckGrid(int h, int v, bool diagonal)
    {
        int horIndex = 0;
        foreach(GridNumbers number in grid.numberPositions.Values)
        {
            horIndex++;
            if(number.h == h)
            {
                if(!number.hasBeenHit)
                {
                    break;
                }
            }
            if(horIndex == 5)
            {
                Debug.Log("Slingo on horizontal " + h);
            }
        }

        int vertIndex = 0;
        foreach (GridNumbers number in grid.numberPositions.Values)
        {
            vertIndex++;
            if (number.v == v)
            {
                if (!number.hasBeenHit)
                {
                    break;
                }
            }
            if (horIndex == 5)
            {
                Debug.Log("Slingo on horizontal " + h);
            }
        }

        if(diagonal)
        {

        }
    }
}
