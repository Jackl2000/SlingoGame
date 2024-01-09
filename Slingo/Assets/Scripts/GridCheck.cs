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
            if(number.h == h)
            {
                horIndex++;
                if (!number.hasBeenHit)
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
            if (number.v == v)
            {
                vertIndex++;
                if (!number.hasBeenHit)
                {
                    break;
                }
            }
            if (vertIndex == 5)
            {
                Debug.Log("Slingo on vertical " + v);
            }
        }

        if(diagonal)
        {
            int leftIndex = 0;
            foreach (GridNumbers number in grid.numberPositions.Values)
            {
                if(number.h == number.v)
                {
                    leftIndex++;
                    if(!number.hasBeenHit)
                    {
                        break;
                    }
                    if(leftIndex == 5)
                    {
                        Debug.Log("Slingo on left diagonal");
                    }
                }
            }

            int rightIndex = 0;
            foreach (GridNumbers number in grid.numberPositions.Values)
            {
                if ((number.h == 5 && number.v == 1) || (number.h == 4 && number.v == 2) || (number.h == 3 && number.v == 3) || (number.h == 2 && number.v == 4) || (number.h == 1 && number.v == 5))
                {
                    rightIndex++;
                    if (!number.hasBeenHit)
                    {
                        break;
                    }
                    if (rightIndex == 5)
                    {
                        Debug.Log("Slingo on right diagonal");
                    }
                }
            }
        }
    }
}
