using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFactory : MonoBehaviour
{
    public List<RuntimeAnimatorController> EnemyAnimator = new List<RuntimeAnimatorController>();
    public GameObject enemyPrefab;

    public GameObject CreateEnemy(string enemyType, int level, GameObject spawnPoint)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.transform);
        switch(enemyType)
        {
            case "Goblin":
                enemy.GetComponent<Animator>().runtimeAnimatorController = EnemyAnimator[0];
                EnemyStats goblinStats = enemy.GetComponent<EnemyStats>();
                enemy.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
                enemy.transform.localPosition = new Vector3(0, -40, 0);
                goblinStats.name = enemyType;
                if (level < 5)
                {
                    goblinStats.Damage = RandomStatInRange(level * level + 1, level * 3 + 2);
                    goblinStats.Health = RandomStatInRange(level, level * 4 - 2);
                    goblinStats.CritChance = RandomStatInRange(level, level + 4);
                }
                else BecomeFinalBoss(goblinStats);
                
                break;
            case "Skeleton":
                enemy.GetComponent<Animator>().runtimeAnimatorController = EnemyAnimator[1];
                EnemyStats skeletonStats = enemy.GetComponent<EnemyStats>();
                skeletonStats.name = enemyType;
                if (level < 5)
                {
                    skeletonStats.Damage = RandomStatInRange(level + 1, level * 2 + 1);
                    skeletonStats.Health = RandomStatInRange(level * 2 + 6, level * 6);
                    skeletonStats.CritChance = RandomStatInRange(level, level * 2);
                }
                else BecomeFinalBoss(skeletonStats);
                break;
            case "Mushroom":
                enemy.GetComponent<Animator>().runtimeAnimatorController = EnemyAnimator[2];
                EnemyStats mushroomStats = enemy.GetComponent<EnemyStats>();
                enemy.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 250);
                enemy.transform.localPosition = new Vector3(0, -10, 0);
                mushroomStats.name = enemyType;
                if (level < 5)
                {
                    mushroomStats.Damage = RandomStatInRange(level + 1, level + 5);
                    mushroomStats.Health = RandomStatInRange(level + 2, level + 4);
                    mushroomStats.CritChance = RandomStatInRange(level * level + 2, level * 4);
                }
                else BecomeFinalBoss(mushroomStats);
                break;
        }
        return enemy;
    }

    private int RandomStatInRange(int min, int max)
    {
        if (min <= 0) min = 1;
        if(max <= 0) max = 1;
        int tmpMin = min;
        if(min > max)
        {
            min = max;
            max = tmpMin;
        }
        return Random.Range(min, max + 1);
    }

    public void BecomeFinalBoss(EnemyStats enemy)
    {
        if (enemy.name == "Goblin")
        {
            enemy.Damage = RandomStatInRange(16, 32);
            enemy.Health = RandomStatInRange(10, 15);
            enemy.CritChance = RandomStatInRange(8, 12);
        }
        else if (enemy.name == "Skeleton")
        {
            enemy.Damage = RandomStatInRange(9, 14);
            enemy.Health = RandomStatInRange(25, 40);
            enemy.CritChance = RandomStatInRange(6, 10);
        }
        else if (enemy.name == "Mushroom")
        {
            enemy.Damage = RandomStatInRange(10, 18);
            enemy.Health = RandomStatInRange(15, 20);
            enemy.CritChance = RandomStatInRange(20, 35);
        }
    }
}
