using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI playerDamageText;
    [SerializeField] private TextMeshProUGUI playerCritchanceText;
    [SerializeField] private TextMeshProUGUI enemyHealthText;
    [SerializeField] private TextMeshProUGUI enemyDamageText;
    [SerializeField] private TextMeshProUGUI enemyCritchanceText;

    public List<Sprite> heartsSprite = new List<Sprite>();
    [SerializeField] private GameObject heartPrefab;

    [SerializeField] private Image enemyBorder;
    public List<Sprite> EnemyPortraits = new List<Sprite>();

    private List<GameObject> playerHearts = new List<GameObject>();
    private List<GameObject> enemyHearts = new List<GameObject>();

    [SerializeField] private TextMeshProUGUI levelText;
    private void Awake()
    {
        playerHealthText.text = PlayerStats.Instance.Health.ToString();
        playerDamageText.text = PlayerStats.Instance.Damage.ToString();
        playerCritchanceText.text = PlayerStats.Instance.Luck.ToString();
        if(PlayerStats.Instance.Level != 5) levelText.text = "LEVEL " + PlayerStats.Instance.Level.ToString();
        else levelText.text = "FINAL BOSS";
    }

    public List<GameObject> ShowHearts(int health, string type)
    {
        List<GameObject> hearts = new List<GameObject>();
        int xStart = 75;
        Vector2 anchorMax = new Vector2(0, 1);
        Vector2 anchorMin = new Vector2(0, 1);
        if (type == "enemy")
        {
            xStart = -425;
            anchorMax = new Vector2(1, 1);
            anchorMin = new Vector2(1, 1);
        }
        float fullHearts = health / 2f;
        int xIndex = 0;
        int yIndex = 0;
        int yPosition = 0;

        if(fullHearts < 1f)
        {
            GameObject halfHeart = Instantiate(heartPrefab, transform);
            RectTransform heartTransform = halfHeart.GetComponent<RectTransform>();
            heartTransform.anchorMax = anchorMax;
            heartTransform.anchorMin = anchorMin;
            halfHeart.GetComponent<Image>().sprite = heartsSprite[1];
            heartTransform.anchoredPosition = new Vector3(xStart, -500, 0);
            hearts.Add(halfHeart);
        }
        else
        {
            for (int i = 1; i <= fullHearts; i++)
            {
                GameObject heart = Instantiate(heartPrefab, transform);
                RectTransform heartTransform = heart.GetComponent<RectTransform>();
                heartTransform.anchorMax = anchorMax;
                heartTransform.anchorMin = anchorMin;
                heartTransform.anchoredPosition = new Vector3(xStart + (xIndex * 120), -500 - yPosition, 0);

                xIndex++;
                yIndex++;
                if (yIndex == 4)
                {
                    yPosition += 100;
                    xIndex = 0;
                    yIndex = 0;
                }
                hearts.Add(heart);
            }
            if (!Mathf.Approximately(fullHearts, Mathf.RoundToInt(fullHearts)) && fullHearts > 1f)
            {
                GameObject halfHeart = Instantiate(heartPrefab, transform);
                RectTransform heartTransform = halfHeart.GetComponent<RectTransform>();
                heartTransform.anchorMax = anchorMax;
                heartTransform.anchorMin = anchorMin;
                halfHeart.GetComponent<Image>().sprite = heartsSprite[1];
                int y = hearts.Count / 4;
                int x = hearts.Count - (y * 4);
                heartTransform.anchoredPosition = new Vector3(xStart + (x * 120), -500 - (y * 100), 0);
                hearts.Add(halfHeart);
            }
        }


        return hearts;
    }

    public void CombatUISetup(EnemyStats enemy)
    {
        playerHearts = ShowHearts(PlayerStats.Instance.Health, "player");
        enemyHearts = ShowHearts(enemy.Health, "enemy");
        if(enemy.name == "Goblin") enemyBorder.sprite = EnemyPortraits[0];
        else if (enemy.name == "Skeleton") enemyBorder.sprite = EnemyPortraits[1];
        else if (enemy.name == "Mushroom") enemyBorder.sprite = EnemyPortraits[2];

        enemyHealthText.text = enemy.Health.ToString();
        enemyDamageText.text = enemy.Damage.ToString();
        enemyCritchanceText.text = enemy.CritChance.ToString();
    }   

    public void UpdateUI(int damage, string type)
    {
        if(type == "player")
        {
            playerHearts = UpdateHearts(playerHearts, damage);
        }
        else
        {
            enemyBorder.GetComponentInParent<Animator>().SetBool("TookDamage", true);
            enemyHearts = UpdateHearts(enemyHearts, damage);
        }
    }

    private List<GameObject> UpdateHearts(List<GameObject> hearts, int damage)
    {
        float heartsTolose = damage / 2f;

        for (int i = hearts.Count - 1; i >= 0; i--)
        {
            if (hearts[i].GetComponent<Image>().sprite == heartsSprite[2]) continue;

            if (hearts[i].GetComponent<Image>().sprite == heartsSprite[0])
            {
                if (heartsTolose >= 1f)
                {
                    hearts[i].GetComponent<Image>().sprite = heartsSprite[2];
                    heartsTolose--;
                }
                else
                {
                    hearts[i].GetComponent<Image>().sprite = heartsSprite[1];
                    heartsTolose -= 0.5f;
                }
            }
            else
            {
                hearts[i].GetComponent<Image>().sprite = heartsSprite[2];
                heartsTolose -= 0.5f;
            }

            if (i == 0 && hearts[i].GetComponent<Image>().sprite == heartsSprite[2])
            {
                return hearts;
            }

            if (heartsTolose == 0)
            {
                return hearts;
            }
        }
        return hearts;
    }

    public void CombatUIReset()
    {
        foreach (GameObject heart in enemyHearts)
        {
            Destroy(heart);
        }
        enemyHearts.Clear();

        foreach (GameObject heart in playerHearts)
        {
            Destroy(heart);
        }
        playerHearts.Clear();
        playerHearts = ShowHearts(PlayerStats.Instance.MaxHealth, "player");
    }

    public void ResetOptionsUI(GameObject optionsParent)
    {
        //options
        optionsParent.GetComponent<Animator>().enabled = false;
        GameObject attack = optionsParent.transform.GetChild(0).gameObject;
        attack.GetComponent<Image>().fillAmount = 1;
        attack.transform.localPosition = new Vector3(-150, 0, 0);
        attack.transform.localScale = Vector3.one;
        attack.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
        attack.transform.localRotation = Quaternion.identity;
        GameObject defend = optionsParent.transform.GetChild(1).gameObject;
        defend.GetComponent<Image>().fillAmount = 1;
        defend.transform.localPosition = new Vector3(150, 0, 0);
        defend.transform.localScale = Vector3.one;
        defend.transform.localRotation = Quaternion.identity;
        defend.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
        optionsParent.GetComponent<Animator>().enabled = true;
    }

    public void Exit(string sceneName)
    {
        PlayerData.Instance.balance += PlayerData.Instance.CombatBonusReward;
        SceneSwap.Instance.SceneSwitch(sceneName);
        //SceneManager.LoadSceneAsync(sceneName);
    }
}
