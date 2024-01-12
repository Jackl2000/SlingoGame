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

    private Image panel;

    [Space(10)]
    [Header("Blink settigs")]
    public float blinkSpeed = 1.85f;
    public Color startColor = Color.white;
    public Color endColor = Color.black;


    private void Awake()
    {
        panel = this.gameObject.GetComponent<Image>();
        startColor = panel.color;
        
    }

    public void FlashingEffect()
    {
        foreach (GridNumbers number in gridGeneration.numberPositions.Values)
        {
            if (spinScript.wCount > 0)
            {
                panel.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * blinkSpeed, 1));
                if (!number.hasBeenHit)
                {
                    number.gameObject.GetComponent<TextMeshProUGUI>().color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * blinkSpeed, 1));
                }
            }
            if (spinScript.wCount <= 0)
            {
                if (!number.hasBeenHit)
                {
                    number.gameObject.GetComponent<TextMeshProUGUI>().color = startColor;
                }
            }
        }
        
    }

    private void Update()
    {
        FlashingEffect();
    }
}
