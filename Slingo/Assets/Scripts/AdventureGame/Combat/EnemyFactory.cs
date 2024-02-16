using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public List<Sprite> EnemySprites = new List<Sprite>();
    public List<AnimatorController> EnemyAnimator = new List<AnimatorController>();
    public GameObject enemyPrefab;
    public GameObject canvasParent;

    public GameObject CreateEnemy(string enemyType, int level)
    {
        GameObject enemy = Instantiate(enemyPrefab, canvasParent.transform);
        switch(enemyType)
        {
            case "Goblin":
                EnemyStats goblinStats = enemy.GetComponent<EnemyStats>();
                goblinStats.sprite = EnemySprites[0];
                goblinStats.animator = EnemyAnimator[0];
                goblinStats.Damage = Random.Range(level, level * 5);
                goblinStats.Health = Random.Range(level, level * 2);
                goblinStats.CritChance = Random.Range(level, level * 3);
                break;
            case "Skeleton":
                EnemyStats skeletonStats = enemy.GetComponent<EnemyStats>();
                skeletonStats.sprite = EnemySprites[0];
                skeletonStats.animator = EnemyAnimator[0];
                skeletonStats.Damage = Random.Range(level, level * 5);
                skeletonStats.Health = Random.Range(level, level * 2);
                skeletonStats.CritChance = Random.Range(level, level * 3);
                break;
        }
        return enemy;
    }
}
