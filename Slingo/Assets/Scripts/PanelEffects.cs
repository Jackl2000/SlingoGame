using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelEffects : MonoBehaviour
{
    Image panel;
    float defualtAlpha;

    private void Awake()
    {
        panel = this.gameObject.GetComponent<Image>();
        defualtAlpha = panel.color.a;
        Debug.Log("default alpha: " + defualtAlpha);
    }

    IEnumerator FlashBlinking()
    {
        panel.color = new Color(1,1,1,0);
        for (int i = 0; i < 5; i++)
        {
            switch (panel.color.a.ToString())
            {
                case "0":
                    panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 1);
                    //Play sound
                    yield return new WaitForSeconds(0.5f);
                    break;
                case "1":
                    panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 0);
                    //Play sound
                    yield return new WaitForSeconds(0.5f);
                    break;
            }
        }
        panel.color = new Color(1, 1, 1, defualtAlpha);
    }

    public void FlashingEffect()
    {
        StartCoroutine(FlashBlinking());
    }

}
