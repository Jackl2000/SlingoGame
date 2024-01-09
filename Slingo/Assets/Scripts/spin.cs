using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class spin : MonoBehaviour
{
    public List<TMP_Text> slotTextList;

    [HideInInspector]
    public int slotNumber;
    
    int rnd;
    int min = 1;
    int max = 15;

    public void Spin()
    {
        foreach (var slotText in slotTextList)
        {
            rnd = Random.Range(min, max);
            min += 15;
            max += 15;

            slotNumber = rnd;
            slotText.text = rnd.ToString();
        }

        min = 1;
        max = 15;
    }

}
