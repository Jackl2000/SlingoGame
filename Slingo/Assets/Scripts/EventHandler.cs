using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    [Header("Slingo")]
    public GridCheck gridCheck;
    public spin spin;
    public CollectReward collectReward;

    [Space(10)]
    [Header("AdventureGame")]
    public PlayerCharacter player;
    public EnemyStats enemyStats;
    public LoadingLevel LoadingScene;
    public CombatSystem combatSystem;
    public ChestChance chestChance;

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


    //Card flip game
    public void CardFlippedEvent()
    {
        GetComponentInParent<CardGameManager>().FlipCard(gameObject);
    }



    //Adventure game
    public void SceneTransitionEndEvent()
    {
        StartCoroutine(LoadingScene.LoadScene());
    }

    public void CombatStartEvent()
    {
        combatSystem.CombatSetup();
        gameObject.SetActive(false);
    }

    public void PlayOptionsEffectEvent()
    {
        transform.GetChild(transform.childCount - 1).GetComponent<ParticleSystem>().Play();
    }

    public void OptionsPicked()
    {
        if (GetComponent<Animator>().GetBool("Attack")) transform.GetChild(0).GetComponent<Animator>().enabled = true;
        else transform.GetChild(1).GetComponent<Animator>().enabled = true;
    }

    public void EnemyDicesAttackDefenceEvent()
    {
        if (combatSystem.OptionsPanel.GetComponent<Animator>().GetBool("Attack")) return;

        combatSystem.OptionsPanel.GetComponent<Animator>().enabled = false;
        if (combatSystem.playerWin)
        {
            combatSystem.OptionsPanel.transform.GetChild(1).GetComponent<Animator>().SetBool("Successfully", true);
        }
        combatSystem.OptionsPanel.transform.GetChild(1).GetComponent<Animator>().SetBool("DefendPicked", true);
        combatSystem.OptionsPanel.transform.GetChild(0).gameObject.SetActive(false);
        combatSystem.messagePanel.GetComponent<Animator>().SetBool("Show", true);
    }

    public void PlayerDicesAttackEvent()
    {
        combatSystem.playerDices.GetComponent<Animator>().SetBool("Attack", false);
        combatSystem.OptionsPanel.GetComponent<Animator>().enabled = false;
        if (combatSystem.playerWin)
        {
            combatSystem.OptionsPanel.transform.GetChild(0).GetComponent<Animator>().SetBool("Successfully", true);
        }
        combatSystem.OptionsPanel.transform.GetChild(0).GetComponent<Animator>().SetBool("AttackPicked", true);
        combatSystem.OptionsPanel.transform.GetChild(1).gameObject.SetActive(false);
        combatSystem.messagePanel.GetComponent<Animator>().SetBool("Show", true);
    }

    public void EnemyCrittMessage()
    {
        if(combatSystem.enemyCrits == true)
        {
            combatSystem.messagePanel.GetComponentInChildren<TextMeshProUGUI>().text = "Kritisktræffer";
            combatSystem.messagePanel.GetComponent<Animator>().SetBool("Show", true);
            StartCoroutine(EnemyCritMessageFadeaway());
        }
    }

    private IEnumerator EnemyCritMessageFadeaway()
    {
        yield return new WaitForSeconds(1);
        combatSystem.messagePanel.GetComponent<Animator>().SetBool("Show", false);
    }

    public void SwordIconAttackEvent()
    {
        GetComponentInChildren<ParticleSystem>().Play();
    }

    public void PlayerSwordStrikeEnemyDiceEvent()
    {
        combatSystem.enemyDices.GetComponent<Animator>().SetBool("LostAttack", true);
    }

    public void DiceAnimationIsFinishedEvent()
    {
        GetComponent<Animator>().SetBool("GoBackToDefault", true);
        combatSystem.CharacterAttack();
        combatSystem.messagePanel.GetComponent<Animator>().SetBool("Show", false);
    }

    public void EnemyDicesResetEvent()
    {
        combatSystem.enemyDicesRestart = true;
    }

    public void PlayerTakeDamageEvent()
    {
        bool temp = combatSystem.enemyCrits;

        Debug.Log("EV CRITTS " + temp);

        if(temp == true)
        {
            int critDamage = enemyStats.Damage + Mathf.RoundToInt(enemyStats.Damage / 2);
            player.PlayerTakeDamage(critDamage);
            player.GetComponentInParent<CombatUI>().UpdateUI(critDamage, "player");
            combatSystem.enemyCrits = false;
        }
        else
        {
            player.PlayerTakeDamage(enemyStats.Damage);
            player.GetComponentInParent<CombatUI>().UpdateUI(enemyStats.Damage, "player");
        }
    }

    public void CharacterLoseBloodEvent()
    {
        GetComponentInChildren<ParticleSystem>().Play();
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
            PlayerStats.Instance.CritAttack = false;
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


    public void chestParticle()
    {
        
        chestChance.chest.GetComponentInChildren<ParticleSystem>().Play();
       
    }
}
