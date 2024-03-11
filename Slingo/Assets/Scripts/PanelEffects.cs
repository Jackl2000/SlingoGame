using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [HideInInspector] public bool blinkeffectStart = false;

    private TextMeshProUGUI bestChoiceText;

    [HideInInspector] public TextMeshProUGUI spinLeftText;

    public void FlashingEffect(bool active, TextMeshProUGUI text)
    {
        if (active)
        {
            bestChoiceText = text;
            blinkeffectStart = true;
        }
        else
        {
            blinkeffectStart = false;
            text.color = Color.white;
        }
    }

    private void Update()
    {
        if (spinLeftText != null)
        {
            spinLeftText.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * blinkSpeed, 1));
        }

        if (blinkeffectStart)
        {
            bestChoiceText.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * blinkSpeed, 1));
        }
    }
}
