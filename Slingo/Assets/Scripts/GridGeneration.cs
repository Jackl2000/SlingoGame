using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridGeneration : MonoBehaviour
{
    List<GameObject> columns = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] columArray = GameObject.FindGameObjectsWithTag("Column");
        columns.AddRange(columArray);
        for (int i = 0; i < columns.Count; i++)
        {
            int index = Convert.ToInt32(columns[i].name[columns[i].name.Length - 1].ToString());
            GenerateColumn(columns[i], index);
        }   
    }


    private void GenerateColumn(GameObject column, int range)
    {
        List<TextMeshProUGUI> fields = new List<TextMeshProUGUI>();
        TextMeshProUGUI[] field = column.GetComponentsInChildren<TextMeshProUGUI>();
        fields.AddRange(field);

        List<int> usedNumbers = new List<int>();

        for (int i = 0; i < fields.Count; i++)
        {
            fields[i].text = GenerateNumber(range ,usedNumbers).ToString();
            usedNumbers.Add(Convert.ToInt32(fields[i].text));

        }
    }

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
                Debug.Log("Not right");
                return 0;
        }
    }
}
