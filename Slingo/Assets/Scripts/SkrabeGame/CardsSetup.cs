using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardsSetup : MonoBehaviour
{
    public Sprite lostSprite;
    private List<GameObject> cards = new List<GameObject>();
    private float[] numbers = new float[] { 0, 0.5f, 2.5f, 5, 10, 12, 15, 20, 35 };
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            cards.Add(transform.GetChild(i).gameObject);
        }
        SetupNumbers();
    }

    private void SetupNumbers()
    {
        List<TextMeshProUGUI> rewards = new List<TextMeshProUGUI>();
        foreach (GameObject go in cards)
        {
            rewards.Add(go.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>());
        }

        ShuffleRewards(rewards);
    }

    private void ShuffleRewards(List<TextMeshProUGUI> rewards)
    {
        List<float> possibleRewards = new List<float>();
        possibleRewards.AddRange(numbers);
        foreach (TextMeshProUGUI text in rewards)
        {
            int randomIndex = 0;
            if (possibleRewards.Count >= 2) randomIndex = Random.Range(0, possibleRewards.Count);
            text.text = UIManager.Instance.DisplayMoney(possibleRewards[randomIndex] * PlayerData.Instance.bet);
            if (text.text == UIManager.Instance.DisplayMoney(0))
            {
                GameObject parent = text.GetComponentInParent<Animator>().gameObject;
                parent.GetComponent<Animator>().enabled = false;
                parent.GetComponent<Image>().sprite = lostSprite;
                text.gameObject.SetActive(false);
            }
            possibleRewards.RemoveAt(randomIndex);
        }
    }
}
