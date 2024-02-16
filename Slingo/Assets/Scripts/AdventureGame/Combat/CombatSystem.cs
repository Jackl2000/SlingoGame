using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatSystem : MonoBehaviour
{
    public List<string> EnemyTypes = new List<string>();

    public EnemyFactory EnemyCreator;

    public GameObject EnemyDice;

    public GameObject PlayerDice;

    public GameObject Description;

    public GameObject OptionsPanel;

    private bool choice = false;
    private bool attacking = false;
    private int playerFullHealth;
    private void Start()
    {
        CombatSetup();
    }

    private void CombatSetup()
    {
        GameObject player = null;
        //playerFullHealth = player.health;
        int level = 1;
        GameObject enemy = EnemyCreator.CreateEnemy(EnemyTypes[Random.Range(0, EnemyTypes.Count)], level);
        StartCoroutine(Combat(player, enemy));
    }

    private IEnumerator Combat(GameObject player, GameObject enemy)
    {
        //enemy roll
        EnemyDice.GetComponent<Animator>().enabled = true;
        int enemyRoll = Roll();
        Debug.Log(enemyRoll);
        yield return new WaitForSeconds(1f);
        EnemyDice.GetComponent<Animator>().enabled = false;
        EnemyDice.transform.rotation = Quaternion.identity;
        DiceReset(EnemyDice, enemyRoll);

        //player guess
        Description.SetActive(true);
        OptionsPanel.SetActive(true);
        while(!choice)
        {
            yield return new WaitForSeconds(0.5f);
        }
        Description.SetActive(false);
        OptionsPanel.SetActive(false);
        choice = false;

        //Player roll
        PlayerDice.GetComponent<Animator>().enabled = true;
        int playerRoll = Roll();
        Debug.Log(playerRoll);
        yield return new WaitForSeconds(1f);
        PlayerDice.GetComponent<Animator>().enabled = false;
        PlayerDice.transform.rotation = Quaternion.identity;
        DiceReset(PlayerDice, playerRoll);

        //someone attack
        yield return new WaitForSeconds(1f);
        Attack(player, enemy, playerRoll, enemyRoll);

        //check for end combat
        if (enemy.GetComponent<EnemyStats>().Health <= 0)
        {
            CombatEnded(true);
        }
        //else if(player.health <= 0) CombatEnded(false);
        else
        {
            //repeat
            yield return Combat(player, enemy);
        }
    }

    private int Roll()
    {
        return Random.Range(1, 7);
    }

    private void DiceReset(GameObject dice, int index)
    {
        Image[] diceImages = dice.GetComponentsInChildren<Image>();
        foreach (Image image in diceImages)
        {
            image.enabled = false;
        }
        dice.transform.GetChild(index - 1).GetComponent<Image>().enabled = true;
    }

    public void PlayerChoice(bool attack)
    {
        choice = true;
        attacking = attack;
    }

    private void Attack(GameObject player, GameObject enemy, int playerRoll, int enemyRoll)
    {
        if(playerRoll > enemyRoll)
        {
            if(attacking)
            {
                //enemy.GetComponent<EnemyStats>().Health -= player.damage;
                Debug.Log("player attack");
            }
        }
        else if (playerRoll < enemyRoll)
        {
            if(attacking)
            {
                Debug.Log("Enemy attack");
                //player.health -= enemy.GetComponent<EnemyStats>().Damage;
            }
        }
    }

    private void CombatEnded(bool victory)
    {
        Debug.Log("Did you win? " + victory);
    }
}
