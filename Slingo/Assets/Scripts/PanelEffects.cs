using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelEffects : MonoBehaviour
{
    [Header("Blink settigs")]
    public float blinkSpeed = 1.85f;
    public Color startColor = Color.white;
    public Color endColor = Color.cyan;

    public spin spinScript;
    private Image panel;

    private void Awake()
    {
        panel = this.gameObject.GetComponent<Image>();
        
        startColor = panel.color;
    }

    public void FlashingEffect()
    {
        panel.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * blinkSpeed, 1));
    }

    private void Update()
    {
        if (spinScript.wCount > 0)
        {
            FlashingEffect();
        }
    }
}
