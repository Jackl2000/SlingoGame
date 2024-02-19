using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFactory : MonoBehaviour
{
    public List<Sprite> EnemySprites = new List<Sprite>();
    public List<RuntimeAnimatorController> EnemyAnimator = new List<RuntimeAnimatorController>();
    public GameObject enemyPrefab;
    public GameObject canvasParent;

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
                goblinStats.Damage = Random.Range(level, level * 3);
                goblinStats.Health = Random.Range(level, level * 2);
                goblinStats.CritChance = Random.Range(level, level * 3);
                break;
            case "Skeleton":
                enemy.GetComponent<Animator>().runtimeAnimatorController = EnemyAnimator[1];
                EnemyStats skeletonStats = enemy.GetComponent<EnemyStats>();
                skeletonStats.Damage = Random.Range(level, level * 2);
                skeletonStats.Health = Random.Range(level, level * 6);
                skeletonStats.CritChance = Random.Range(level, level * 2);
                break;
            case "Mushroom":
                enemy.GetComponent<Animator>().runtimeAnimatorController = EnemyAnimator[2];
                EnemyStats mushroomStats = enemy.GetComponent<EnemyStats>();
                enemy.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 250);
                enemy.transform.localPosition = new Vector3(0, -10, 0);
                mushroomStats.Damage = Random.Range(level, level + 3);
                mushroomStats.Health = Random.Range(level, level * 3);
                mushroomStats.CritChance = Random.Range(level, level * 4);
                break;
        }
        return enemy;
    }
}
