using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
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
    private void Awake()
    {
        playerHealthText.text = 5.ToString();
        playerDamageText.text = 5.ToString();
        playerCritchanceText.text = 5.ToString();

        playerHearts = ShowHearts(11, "player");
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
        for (int i = 1; i <= fullHearts; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            RectTransform heartTransform = heart.GetComponent<RectTransform>();
            heartTransform.anchorMax = anchorMax;
            heartTransform.anchorMin = anchorMin;
            heartTransform.anchoredPosition = new Vector3(xStart + (xIndex * 120), -400 - yPosition, 0);

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
            heartTransform.anchoredPosition = new Vector3(xStart + (x * 120), -400 - (y * 100), 0);
            hearts.Add(halfHeart);
        }
        return hearts;
    }

    public void CombatUISetup(EnemyStats enemy)
    {
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
                Debug.Log("Enemy death");
                return hearts;
            }

            if (heartsTolose == 0)
            {
                return hearts;
            }
        }
        Debug.Log("Something went wrong");
        return null;
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
        playerHearts = ShowHearts(11, "player");
    }
}
