using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PanelEffects : MonoBehaviour
{
    [Header("References")]
    public spin spinScript;
    public GridGeneration gridGeneration;

    [Space(10)]
    [Header("Blink settigs")]
    public float blinkSpeed = 1.85f;
    public Color startColor = Color.white;
    public Color endColor = Color.black;
    public bool isBlinking;
    public bool numbersPlateIsBlinking = false;

    public void FlashingEffect()
    {
        foreach (GridNumbers number in gridGeneration.numberPositions.Values)
        {
            if (spinScript.wildPicks > 0)
            {
                if (!number.hasBeenHit && number.gameObject.GetComponent<TextMeshProUGUI>().color != Color.yellow)
                {
                    number.gameObject.GetComponent<TextMeshProUGUI>().color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * blinkSpeed, 1));
                }
            }
            if (spinScript.wildPicks <= 0)
            {
                if (!number.hasBeenHit)
                {
                    number.gameObject.GetComponent<TextMeshProUGUI>().color = startColor;
                }
            }
        }
        
    }

    public void BlinkingEffect()
    {
        //color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * blinkSpeed, 1));
        this.gameObject.GetComponent<Graphic>().color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * blinkSpeed, 1));
    }

    private void Update()
    {
        if (numbersPlateIsBlinking)
        {
            FlashingEffect();
        }

        if (isBlinking)
        {
            BlinkingEffect();

        }
    }
}
