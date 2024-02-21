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
                goblinStats.Damage = RandomStatInRange(level * level, level * 3 + 2);
                goblinStats.Health = RandomStatInRange(level, level * 2);
                goblinStats.CritChance = RandomStatInRange(level, level + 4);
                break;
            case "Skeleton":
                enemy.GetComponent<Animator>().runtimeAnimatorController = EnemyAnimator[1];
                EnemyStats skeletonStats = enemy.GetComponent<EnemyStats>();
                skeletonStats.name = enemyType;
                skeletonStats.Damage = RandomStatInRange(level + 1, level * 2 + 1);
                skeletonStats.Health = RandomStatInRange(level * 2 + 6, level * 6);
                skeletonStats.CritChance = RandomStatInRange(level, level * 2);
                break;
            case "Mushroom":
                enemy.GetComponent<Animator>().runtimeAnimatorController = EnemyAnimator[2];
                EnemyStats mushroomStats = enemy.GetComponent<EnemyStats>();
                enemy.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 250);
                enemy.transform.localPosition = new Vector3(0, -10, 0);
                mushroomStats.name = enemyType;
                mushroomStats.Damage = RandomStatInRange(level + 1, level + 5);
                mushroomStats.Health = RandomStatInRange(level, level + 4);
                mushroomStats.CritChance = RandomStatInRange(level * level, level * 4);
                break;
        }
        return enemy;
    }

    private int RandomStatInRange(int min, int max)
    {
        int tmpMin = min;
        if(min > max)
        {
            min = max;
            max = tmpMin;
        }
        return Random.Range(min, max + 1);
    }
}
