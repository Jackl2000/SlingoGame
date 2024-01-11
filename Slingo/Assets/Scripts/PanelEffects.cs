using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelEffects : MonoBehaviour
{
    [Header("Fade settigs")]
    public float fadeSpeed = .01f;

    public spin spinScript;
    Image panel;
    [HideInInspector]
    public float defualtAlpha;


    private void Awake()
    {
        panel = this.gameObject.GetComponent<Image>();
        defualtAlpha = panel.color.a;
        Debug.Log("default alpha: " + defualtAlpha);
    }

    public IEnumerator FlashBlinking()
    {
        panel.color = new Color(1, 1, 1, 0);
        for (int i = 0; i < 5; i++)
        {
            for (float alpha = 0.4f; alpha > 0.1f; alpha -= .01f)
            {
                //fade out
                panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, alpha);
                yield return new WaitForSeconds(fadeSpeed);
            }
            for (float alpha = 0; alpha < defualtAlpha; alpha += .01f)
            {
                //fade in
                panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, alpha);
                yield return new WaitForSeconds(fadeSpeed);
            }
        }
        panel.color = new Color(1, 1, 1, defualtAlpha);

    }

    public void StopBlinking()
    {
        StopCoroutine(FlashBlinking());
    }

    public void FlashingEffect()
    {
        StartCoroutine(FlashBlinking());
    }

}
