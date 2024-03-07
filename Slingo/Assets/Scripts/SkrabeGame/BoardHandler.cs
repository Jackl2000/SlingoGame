using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardHandler : MonoBehaviour
{
    public TextMeshProUGUI balanceText;
    public TextMeshProUGUI indsatsText;
    public TextMeshProUGUI gevinstText;
    public GameObject gameFinishedPanel;

    private float gevinst = 0f;
    private void Start()
    {
        balanceText.text = "Balance: " + UIManager.Instance.DisplayMoney(PlayerData.Instance.balance);
        indsatsText.text = "Indsats: " + UIManager.Instance.DisplayMoney(PlayerData.Instance.totalIndsats);
        gevinstText.text = "Bonus gevinst: " + UIManager.Instance.DisplayMoney(gevinst);
    }

    public void CardEarned(float reward)
    {
        if (reward == 0)
        {
            gameFinishedPanel.SetActive(true);
            gameFinishedPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Tillykke du har vundet " + UIManager.Instance.DisplayMoney(gevinst);
            return;
        }
        else gevinst += reward;

        gevinstText.text = "Bonus gevinst: " + UIManager.Instance.DisplayMoney(gevinst);

        if(gevinst == 50 * PlayerData.Instance.bet)
        {
            gameFinishedPanel.SetActive(true);
            gameFinishedPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Tillykke du har vundet " + UIManager.Instance.DisplayMoney(gevinst);
        }
    }

    public void ExitWithReward(string sceneName)
    {
        PlayerData.Instance.balance += gevinst;
        SceneSwap.Instance.SceneSwitch(sceneName);
    }
}
