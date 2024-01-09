using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridCheck : MonoBehaviour
{
    private GridGeneration grid;
    private Dictionary<string, bool> gridSlingoList = new Dictionary<string, bool>();
    private int hSlingo = 0;
    private int vSlingo = 0;
    private bool hComplete = false;
    private bool vComplete = false;
    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<GridGeneration>();
        gridSlingoList.Add("h1", false);
        gridSlingoList.Add("h2", false);
        gridSlingoList.Add("h3", false);
        gridSlingoList.Add("h4", false);
        gridSlingoList.Add("h5", false);
        gridSlingoList.Add("v1", false);
        gridSlingoList.Add("v2", false);
        gridSlingoList.Add("v3", false);
        gridSlingoList.Add("v4", false);
        gridSlingoList.Add("v5", false);
        gridSlingoList.Add("dl", false);
        gridSlingoList.Add("dr", false);
    }

    public void ResetGrid()
    {
        foreach(string item in gridSlingoList.Keys.ToList())
        {
            gridSlingoList[item] = false;
        }

        hSlingo = 0;
        vSlingo = 0;
        hComplete = false; 
        vComplete = false;
    }

    public void CheckGrid(int h, int v, bool diagonal)
    {
        if(!hComplete)
        {
            int horIndex = 0;
            foreach (GridNumbers number in grid.numberPositions.Values)
            {
                if (number.h == h)
                {
                    horIndex++;
                    if (!number.hasBeenHit)
                    {
                        break;
                    }
                }
                if (horIndex == 5 && !gridSlingoList["h" + h])
                {
                    Debug.Log("Slingo on horizontal " + h);
                    gridSlingoList["h" + h] = true;

                    hSlingo++;
                    if (hSlingo == 5)
                    {
                        hComplete = true;
                    }
                    break;
                }
            }
        }

        if(!vComplete)
        {
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
                if (vertIndex == 5 && !gridSlingoList["v" + v])
                {
                    Debug.Log("Slingo on vertical " + v);
                    gridSlingoList["v" + v] = true;

                    vSlingo++;
                    if (vSlingo == 5)
                    {
                        vComplete = true;
                    }
                    break;
                }
            }
        }


        if(diagonal)
        {
            if(!gridSlingoList["dl"])
            {
                int leftIndex = 0;
                foreach (GridNumbers number in grid.numberPositions.Values)
                {
                    if (number.h == number.v)
                    {
                        leftIndex++;
                        if (!number.hasBeenHit)
                        {
                            break;
                        }
                        if (leftIndex == 5)
                        {
                            Debug.Log("Slingo on left diagonal");
                            gridSlingoList["dl"] = true;
                            break;
                        }
                    }
                }
            }

            if (!gridSlingoList["dr"])
            {
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
                        if (rightIndex == 5 && !gridSlingoList["dr"])
                        {
                            Debug.Log("Slingo on right diagonal");
                            gridSlingoList["dr"] = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}
