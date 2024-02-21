using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public GridCheck gridCheck;
    public spin spin;
    public CollectReward collectReward;
    public PlayerCharacter player;
    public EnemyStats enemyStats;

    public void SlingoBoardHideAnimationEvent()
    {
        gridCheck.SlingoBorderGoIdle();
    }

    public void SpinButtonChangeAnimationEvent()
    {
        spin.ChangeSpinButton();
    }

    public void SpinButtonChangeEndEvent()
    {
        spin.SpinButtonChangeFinished();
    }

    public void BalanceBorderStopAniEvent()
    {
        collectReward.stopAni();
    }



    //Adventure game
    public void PlayerTakeDamageEvent()
    {
        int random = Random.Range(0, 101);

        if(enemyStats.CritChance >= random)
        {
            int critDamage = enemyStats.Damage + Mathf.RoundToInt(PlayerStats.Instance.Damage / 2);
            player.PlayerTakeDamage(critDamage);
            player.GetComponentInParent<CombatUI>().UpdateUI(critDamage, "player");
        }
        else
        {
            player.PlayerTakeDamage(enemyStats.Damage);
            player.GetComponentInParent<CombatUI>().UpdateUI(enemyStats.Damage, "player");
        }
    }

    public void PlayerHasTakenDamageEvent()
    {
        player.PlayerReset();
    }

    public void EnemyTakeDamageEvent()
    {
        if(PlayerStats.Instance.CritAttack)
        {
            int critDamage = PlayerStats.Instance.Damage + Mathf.RoundToInt(PlayerStats.Instance.Damage / 2);
            enemyStats.EnemyTakeDamage(critDamage);
            enemyStats.GetComponentInParent<CombatUI>().UpdateUI(critDamage, "enemy");
        }
        else
        {
            enemyStats.EnemyTakeDamage(PlayerStats.Instance.Damage);
            enemyStats.GetComponentInParent<CombatUI>().UpdateUI(PlayerStats.Instance.Damage, "enemy");
        }
    }

    public void EnemyHasTakenDamageEvent()
    {
        enemyStats.EnemyHasTakenDamage();
    }

    public void EnemyBorderFinishedEvent()
    {
        GetComponent<Animator>().SetBool("TookDamage", false);
    }
}
