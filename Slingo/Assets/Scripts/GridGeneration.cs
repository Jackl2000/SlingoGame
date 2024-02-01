using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridGeneration : MonoBehaviour
{
    public Dictionary<int, GridNumbers> numberPositions = new Dictionary<int, GridNumbers>();
    private List<GameObject> columns = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GetColumns();
    }

    /// <summary>
    /// Clears the grid and creates a new
    /// </summary>
    public void ReGenerateGrid()
    {
        foreach (GridNumbers number in numberPositions.Values)
        {
            if(number.hasBeenHit)
            {
                number.ResetData();
            }
        }
        GetComponent<GridCheck>().ResetGrid();
        columns.Clear();
        numberPositions.Clear();
        GetColumns();
    }

    /// <summary>
    /// Gets the colums of the grid
    /// </summary>
    private void GetColumns()
    {
        GameObject[] columnArray = GameObject.FindGameObjectsWithTag("Column").OrderBy(go => go.name).ToArray();
        columns.AddRange(columnArray);
        for (int i = 0; i < columns.Count; i++)
        {
            int index = Convert.ToInt32(columns[i].name[columns[i].name.Length - 1].ToString());
            GenerateColumn(columns[i], index, i + 1);
        }
    }

    /// <summary>
    /// Generate the numbers inside each column
    /// </summary>
    private void GenerateColumn(GameObject column, int range, int columnCount)
    {
        List<TextMeshProUGUI> fields = new List<TextMeshProUGUI>();
        TextMeshProUGUI[] field = column.GetComponentsInChildren<TextMeshProUGUI>();
        fields.AddRange(field);
        List<int> usedNumbers = new List<int>();

        for (int i = 0; i < fields.Count; i++)
        {
            fields[i].text = GenerateNumber(range ,usedNumbers).ToString();
            usedNumbers.Add(Convert.ToInt32(fields[i].text));
            numberPositions.Add(Convert.ToInt32(field[i].text), new GridNumbers(columnCount, i + 1, fields[i].gameObject, Convert.ToInt32(fields[i].text)));
        }
    }

    /// <summary>
    /// Returns a random number within a range which does not exists in the list
    /// </summary>
    /// <param name="range">Determines the range of the number </param>
    /// <param name="usedNumbers">A list containing used numbers that this method can not return</param>
    /// <returns></returns>
    private int GenerateNumber(int range, List<int> usedNumbers)
    {
        int random = 0;
        switch (range)
        {
            case 1:
                random = UnityEngine.Random.Range(1, 16);
                if (usedNumbers.Contains(random))
                {
                    return GenerateNumber(range, usedNumbers);
                }
                else return random;
            case 2:
                random = UnityEngine.Random.Range(16, 31);
                if (usedNumbers.Contains(random))
                {
                    return GenerateNumber(range, usedNumbers);
                }
                else return random;
            case 3:
                random = UnityEngine.Random.Range(31, 46);
                if (usedNumbers.Contains(random))
                {
                    return GenerateNumber(range, usedNumbers);
                }
                else return random;
            case 4:
                random = UnityEngine.Random.Range(46, 61);
                if (usedNumbers.Contains(random))
                {
                    return GenerateNumber(range, usedNumbers);
                }
                else return random;
            case 5:
                random = UnityEngine.Random.Range(61, 76);
                if (usedNumbers.Contains(random))
                {
                    return GenerateNumber(range, usedNumbers);
                }
                else return random;
            default:
                return 0;
        }
    }
    /// <summary>
    /// Cheatsheet, click "P" to fill plate immediately
    /// </summary>
    public void FillPlate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (int gridNumber in numberPositions.Keys)
            {
                numberPositions[gridNumber].Hit();
            }
        }
    }
    private void Update()
    {
        if(Application.isEditor)
        {
            FillPlate();
        }
    }
}
