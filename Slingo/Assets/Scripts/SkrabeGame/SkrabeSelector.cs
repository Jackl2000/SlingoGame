using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkrabeSelector : MonoBehaviour
{
    public Material targetMaterial;

    private List<GameObject> cards = new List<GameObject>();
    private GameObject selectedCard;
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            cards.Add(transform.GetChild(i).gameObject);
        }
    }

    public GameObject SelectCard(GameObject hole)
    {
        foreach (GameObject card in cards)
        {
            if(Vector3.Distance(hole.transform.position, card.transform.position) < 60f)
            {
                card.transform.GetChild(1).GetComponent<Image>().material = targetMaterial;
                card.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "";
                selectedCard = card;
                break;
            }
        }
        return selectedCard;
    }

    public void ResetSelctedCard()
    {
        selectedCard.transform.GetChild(1).gameObject.SetActive(false);
        cards.Remove(selectedCard);
        TextMeshProUGUI rewardText = selectedCard.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        if (rewardText == null) GetComponentInParent<BoardHandler>().CardEarned(0);
        else GetComponentInParent<BoardHandler>().CardEarned(UIManager.Instance.GetMoneyValue(rewardText.text));
        selectedCard = null;
    }
}
