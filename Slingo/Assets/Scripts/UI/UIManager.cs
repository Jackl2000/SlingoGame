using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIManager
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }
    }

    public string DisplayMoney(float money)
    {
        return money.ToString("n2") + " kr";
    }

    public float GetMoneyValue(string money)
    {
        return float.Parse(money.Substring(0, money.Length - 3));
    }

    /// <summary>
    /// Change individuel letter/number in text over time for a nice animation
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="text">The text object</param>
    /// <param name="speed">How much time between each letter/number</param>
    /// <param name="colors">Colors to change between, has to be 4 colors in an array</param>
    /// <param name="startingColor">Color to change back to</param>
    /// <param name="times">How many times should it loop</param>
    /// <param name="backWards">Only insert hvis parameter if you do not want the text to change its color back to the orignal color</param>
    /// <returns></returns>
    public IEnumerator TextColorAnimation(MonoBehaviour mono, TextMeshProUGUI text, float speed, Color[] colors, Color startingColor, int times, bool backWards = false)
    {
        if (backWards) text.textInfo.wordInfo = text.textInfo.wordInfo.Reverse().ToArray();

        for (int i = 0; i < text.textInfo.wordInfo.Length; i++)
        {
            if (text.textInfo.wordInfo[i].characterCount == 0)
            {
                continue;
            }
            else
            {
                TMP_WordInfo wordInfo = text.textInfo.wordInfo[i];
                for (int j = 0; j < wordInfo.characterCount; j++)
                {
                    int characterIndex = 0;
                    if (!backWards)
                    {
                        characterIndex = wordInfo.firstCharacterIndex + j;
                    }
                    else
                    {
                        characterIndex = wordInfo.lastCharacterIndex - j;
                    }

                    int meshIndex = text.textInfo.characterInfo[characterIndex].materialReferenceIndex;
                    int vertexIndex = text.textInfo.characterInfo[characterIndex].vertexIndex;

                    Color32[] vertexColor = text.textInfo.meshInfo[meshIndex].colors32;

                    if (backWards)
                    {
                        vertexColor[vertexIndex + 0] = startingColor;
                        vertexColor[vertexIndex + 1] = startingColor;
                        vertexColor[vertexIndex + 2] = startingColor;
                        vertexColor[vertexIndex + 3] = startingColor;
                    }
                    else
                    {
                        vertexColor[vertexIndex + 0] = colors[0];
                        vertexColor[vertexIndex + 1] = colors[1];
                        vertexColor[vertexIndex + 2] = colors[2];
                        vertexColor[vertexIndex + 3] = colors[3];
                    }

                    text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
                    yield return new WaitForSeconds(speed);
                }
            }
        }
        if (!backWards)
        {
            mono.StartCoroutine(TextColorAnimation(mono, text, speed, colors, startingColor, times, true));
        }
        else if (times > 1)
        {
            text.textInfo.wordInfo = text.textInfo.wordInfo.Reverse().ToArray();
            mono.StartCoroutine(TextColorAnimation(mono, text, speed, colors, startingColor, times - 1));
        }
    }

    public void ChangeTextColor(TextMeshProUGUI text, Color color)
    {
        text.color = color;
    }
}
