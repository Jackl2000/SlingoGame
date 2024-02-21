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
        player.PlayerTakeDamage();
        player.GetComponentInParent<CombatUI>().UpdateUI(enemyStats.Damage, "player");
    }

    public void PlayerHasTakenDamageEvent()
    {
        player.PlayerReset();
    }

    public void EnemyTakeDamageEvent()
    {
        enemyStats.EnemyTakeDamage(3);
        enemyStats.GetComponentInParent<CombatUI>().UpdateUI(3, "enemy");
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
