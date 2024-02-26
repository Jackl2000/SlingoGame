using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatSystem : MonoBehaviour
{
    public List<string> EnemyTypes = new List<string>();
    public EnemyFactory EnemyCreator;
    public GameObject EnemyDice;
    public GameObject EnemyDice2;
    public GameObject PlayerDice;
    public GameObject PlayerDice2;
    public GameObject Description;
    public GameObject OptionsPanel;
    public GameObject enemySpawnPoint;
    public GameObject GameOverPanel;
    public Animator sceneTranisition;

    private CombatUI combatUI;
    private float speed;
    public GameObject playerObject;
    private GameObject enemyObject;
    private GameObject movingCharacter;
    private GameObject target;

    private Vector2 playerStartPosition;
    private Vector2 enemyStartPosition;
    private float startDistanceToTarget;

    private bool choice = false;
    private bool attacking = false;
    private int playerFullHealth;
    private bool attackFinished = false;
    private bool characterMoveBack = false;
    private bool combatReset = false;

    private void Start()
    {
        sceneTranisition.SetBool("Combat", true);
        combatUI = EnemyDice.GetComponentInParent<CombatUI>();
    }

    public void CombatSetup()
    {
        //GameObject player = null;
        PlayerStats.Instance.Health = PlayerStats.Instance.MaxHealth;
        GameObject enemy = EnemyCreator.CreateEnemy(EnemyTypes[Random.Range(0, EnemyTypes.Count)], PlayerStats.Instance.Level, enemySpawnPoint);
        playerStartPosition = playerObject.transform.position;
        enemyStartPosition = enemy.transform.position;

        playerObject.GetComponent<EventHandler>().enemyStats = enemy.GetComponent<EnemyStats>();
        enemy.GetComponent<EventHandler>().player = playerObject.GetComponent<PlayerCharacter>();

        combatUI.CombatUISetup(enemy.GetComponent<EnemyStats>());
        startDistanceToTarget = Vector3.Distance(playerStartPosition, enemyStartPosition);
        StartCoroutine(Combat(playerObject, enemy));
    }

    private IEnumerator Combat(GameObject player, GameObject enemy)
    {
        //enemy roll
        int enemyRoll = Roll();
        int enemyRoll2 = Roll();
        yield return StartCoroutine(DiceRoll(EnemyDice, EnemyDice2, enemyRoll, enemyRoll2));

        //player guess
        Description.SetActive(true);
        OptionsPanel.SetActive(true);
        while(!choice)
        {
            yield return new WaitForSeconds(0.2f);
        }
        Description.SetActive(false);
        OptionsPanel.SetActive(false);

        //Player roll
        int playerRoll = Roll();
        int playerRoll2 = Roll();
        yield return StartCoroutine(DiceRoll(PlayerDice, PlayerDice2, playerRoll, playerRoll2));

        //someone attack
        yield return new WaitForSeconds(0.5f);
        CheckAttack(player, enemy, playerRoll + playerRoll2, enemyRoll + enemyRoll2);

        while(!combatReset)
        {
            yield return new WaitForSeconds(0.2f);
        }

        //check for combat end
        if (enemy.GetComponent<EnemyStats>().Health <= 0)
        {
            CombatEnded(true);
        }
        else if(PlayerStats.Instance.Health <= 0) CombatEnded(false);
        else
        {
            //repeat
            ResetValues();
            yield return Combat(player, enemy);
        }
    }

    private IEnumerator DiceRoll(GameObject dice, GameObject dice2, int roll, int roll2)
    {
        dice.GetComponent<Animator>().enabled = true;
        dice2.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(1f);
        dice.GetComponent<Animator>().enabled = false;
        dice.transform.rotation = Quaternion.identity;
        DiceReset(dice, roll);

        yield return new WaitForSeconds(1f);
        dice2.GetComponent<Animator>().enabled = false;
        dice2.transform.rotation = Quaternion.identity;
        DiceReset(dice2, roll2);
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

    private void CheckAttack(GameObject player, GameObject enemy, int playerRoll, int enemyRoll)
    {
        playerObject = player;
        enemyObject = enemy;

        if (playerRoll > enemyRoll)
        {
            if(attacking)
            {
                player.GetComponent<Animator>().SetBool("Run", true);
                movingCharacter = playerObject;
                target = enemyObject;
                return;
            }
            else
            {
                enemy.GetComponent<Animator>().SetBool("Run", true);
                movingCharacter = enemyObject;
                target = playerObject;
                return;
            }
        }
        else if (playerRoll < enemyRoll)
        {
            if(attacking)
            {
                enemy.GetComponent<Animator>().SetBool("Run", true);
                movingCharacter = enemyObject;
                target = playerObject;
                return;
            }
        }
        combatReset = true;
    }

    private void CombatEnded(bool victory)
    {
        Debug.Log("Combat ended in a win?:" + victory);
        if(victory)
        {
            PlayerStats.Instance.Level++;
            //ResetValues();
            //combatUI.CombatUIReset();
            //CombatSetup();
            StartCoroutine(EnteringNewLevel());
        }
        else
        {
            GameOverPanel.SetActive(true);
        }
    }

    private IEnumerator EnteringNewLevel()
    {
        Debug.Log("Entering new level very soon");
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync("AdventureGameLoadLevelScene");
    }

    private void Update()
    {
        if (movingCharacter != null && !attackFinished)
        {
            MoveCharacter(target.transform.position);
        }
        else if (characterMoveBack)
        {
            if(movingCharacter.GetComponent<PlayerCharacter>() != null)
            {
                MoveCharacter(playerStartPosition, 1);
            }
            else
            {
                MoveCharacter(enemyStartPosition, 1);
            }
        }
    }

    private void MoveCharacter(Vector3 targetPosition, float rangeToTarget = 80)
    {
        Vector3 direction = new Vector3(targetPosition.x, movingCharacter.transform.position.y, 0) - movingCharacter.transform.position;
        float distanceToTarget = Vector3.Distance(movingCharacter.transform.position, targetPosition);
        if (distanceToTarget >= rangeToTarget)
        {
            speed = 2 + (startDistanceToTarget - distanceToTarget) / 100;
            movingCharacter.transform.position += direction * speed * Time.deltaTime;
        }
        else if(!attackFinished)
        {
            StartCoroutine(Attack(movingCharacter));
        }
        else if(!combatReset)
        {
            Turn(movingCharacter);
            combatReset = true;
            movingCharacter.GetComponent<Animator>().SetBool("Run", false);
        }
    }

    private IEnumerator Attack(GameObject attacker)
    {
        attackFinished = true;

        if(attacker == playerObject)
        {
            int random = Random.Range(0, 101);
            Debug.Log("Player selected number:" + random);
            if (PlayerStats.Instance.Luck >= random)
            {
                PlayerStats.Instance.CritAttack = true;
                attacker.GetComponent<Animator>().SetBool("CritAttack", true);
                yield return new WaitForSeconds(0.4f);
                attacker.GetComponent<Animator>().SetBool("CritAttack", false);
                yield return new WaitForSeconds(0.2f);
                Turn(attacker);
                characterMoveBack = true;
            }
            else
            {
                attacker.GetComponent<Animator>().SetBool("Attack", true);
                yield return new WaitForSeconds(0.4f);
                attacker.GetComponent<Animator>().SetBool("Attack", false);
                yield return new WaitForSeconds(0.2f);
                Turn(attacker);
                characterMoveBack = true;
            }
        }
        else
        {
            attacker.GetComponent<Animator>().SetBool("Attack", true);
            yield return new WaitForSeconds(0.4f);
            attacker.GetComponent<Animator>().SetBool("Attack", false);
            yield return new WaitForSeconds(0.2f);
            Turn(attacker);
            characterMoveBack = true;
        }
    }

    private void Turn(GameObject character)
    {
        if (character.transform.rotation.y == 1 || character.transform.rotation.y == 1) character.transform.rotation = new Quaternion(0, 0, 0, 0);
        else character.transform.rotation = new Quaternion(0, 1, 0, 0);
    }
    private void ResetValues()
    {
        choice = false;
        characterMoveBack = false;
        attackFinished = false;
        movingCharacter = null;
        combatReset = false;
    }
}
