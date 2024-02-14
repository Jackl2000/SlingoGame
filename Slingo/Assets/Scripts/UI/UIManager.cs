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

    public IEnumerator TextColorAnimation(MonoBehaviour mono, TextMeshProUGUI text, float speed, Color[] colors, int times, bool backWards = false)
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
                        vertexColor[vertexIndex + 0] = Color.white;
                        vertexColor[vertexIndex + 1] = Color.white;
                        vertexColor[vertexIndex + 2] = Color.white;
                        vertexColor[vertexIndex + 3] = Color.white;
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
            mono.StartCoroutine(TextColorAnimation(mono, text, speed, colors, times, true));
            //StartCoroutine(TextColorAnimation(text, speed, colors, times, true));
        }
        else if (times != 0)
        {
            text.textInfo.wordInfo = text.textInfo.wordInfo.Reverse().ToArray();
            mono.StartCoroutine(TextColorAnimation(mono, text, speed, colors, times - 1));
        }
    }
}
