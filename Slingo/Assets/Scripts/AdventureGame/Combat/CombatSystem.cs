using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CombatSystem : MonoBehaviour
{
    public List<string> EnemyTypes = new List<string>();
    public EnemyFactory EnemyCreator;
    public GameObject enemyDices;
    public GameObject playerDices;
    public GameObject Description;
    public GameObject OptionsPanel;
    public GameObject enemySpawnPoint;
    public GameObject GameOverPanel;
    public Animator sceneTranisition;
    public GameObject chest;
    public GameObject messagePanel;
    public bool enemyCrits;
   



    [HideInInspector] public bool playerWin;

    private GameObject enemyDice;
    private GameObject enemyDice2;
    private GameObject playerDice;
    private GameObject playerDice2;
    private CombatUI combatUI;
    private Animator optionsAnimator;
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
    private bool dicesDone = false;
    [SerializeField] private bool attackFinished = false;
    [SerializeField] private bool characterMoveBack = false;
    [SerializeField] private bool combatReset = false;
    private bool playerCritt;

    private void Start()
    {
        sceneTranisition.SetBool("Combat", true);
        optionsAnimator = OptionsPanel.GetComponent<Animator>();
        combatUI = enemyDices.GetComponentInParent<CombatUI>();
        enemyDice = enemyDices.transform.GetChild(0).gameObject;
        enemyDice2 = enemyDices.transform.GetChild(1).gameObject;
        playerDice = playerDices.transform.GetChild(0).gameObject;
        playerDice2 = playerDices.transform.GetChild(1).gameObject;
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
        enemyDices.GetComponent<Animator>().enabled = false;
        yield return StartCoroutine(DiceRoll(enemyDice, enemyDice2, enemyRoll, enemyRoll2));
        enemyDices.GetComponent<Animator>().enabled = true;

        //player guess
        Description.SetActive(true);
        OptionsPanel.SetActive(true);

        while(!choice)
        {
            yield return new WaitForSeconds(0.2f);
        }
        Description.SetActive(false);

        //Player roll
        int playerRoll = Roll();
        int playerRoll2 = Roll();
        yield return StartCoroutine(DiceRoll(playerDice, playerDice2, playerRoll, playerRoll2));

        //someone attack
        yield return new WaitForSeconds(0.5f);
        CheckAttack(player, enemy, playerRoll + playerRoll2, enemyRoll + enemyRoll2);

        while(!combatReset)
        {
            yield return new WaitForSeconds(0.2f);
        }

        if(messagePanel.GetComponentInChildren<TextMeshProUGUI>().text == "Uafgjort")
        {
            messagePanel.GetComponent<Animator>().SetBool("Show", true);
            DrawMessageDisapear();
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
        if(attack) optionsAnimator.SetBool("Attack", true);
        else optionsAnimator.SetBool("Defend", true);
    }
    /// <summary>
    /// Checks if action succeds or fails
    /// </summary>
    /// <param name="player"></param>
    /// <param name="enemy"></param>
    /// <param name="playerRoll"></param>
    /// <param name="enemyRoll"></param>
    private void CheckAttack(GameObject player, GameObject enemy, int playerRoll, int enemyRoll)
    {
        playerObject = player;
        enemyObject = enemy;
        enemyObject.GetComponent<EventHandler>().combatSystem = this;
        enemyDices.GetComponent<Animator>().SetBool("PlayerHasChosen", true);
        messagePanel.transform.SetAsLastSibling();

        if (playerRoll > enemyRoll)
        {
            if (attacking)
            {
                playerDices.GetComponent<Animator>().enabled = true;
                playerDices.GetComponent<Animator>().SetBool("Attack", true);
                movingCharacter = playerObject;
                target = enemyObject;
                enemyDices.GetComponent<Animator>().SetBool("PlayerAttack", true);
                playerWin = true;
                messagePanel.GetComponentInChildren<TextMeshProUGUI>().text = "Angreb lykkedes";
                Debug.Log("I attack" + playerCritt);


                int random = Random.Range(1, 101);
                Debug.Log("Player selected number:" + random);
                Debug.Log("luck " + PlayerStats.Instance.Luck);
                if (PlayerStats.Instance.Luck >= random)
                {
                    playerCritt = PlayerStats.Instance.CritAttack = true;

                    messagePanel.GetComponentInChildren<TextMeshProUGUI>().text = "Angreb lykkedes \n Kritisktræffer";
                }
            }
            else
            {
                movingCharacter = enemyObject;
                target = playerObject;
                enemyDices.GetComponent<Animator>().SetBool("Defended", false);
                enemyDices.GetComponent<Animator>().SetBool("PlayerAttack", false);
                playerWin = false;
                messagePanel.GetComponentInChildren<TextMeshProUGUI>().text = "Forsvar mislykkedes";

                int enemyCritChance = enemy.GetComponent<EnemyStats>().CritChance;
                int random = Random.Range(1, 101);
                Debug.Log("this luck no "+ random);
                Debug.Log("Ene critt chance" + enemyCritChance);
                if (enemyCritChance >= random)
                {
                    enemyCrits = true;
                    Debug.Log("enemy crit on chance: " + enemyCritChance  + " selected number: " + random);
                    Debug.Log(enemyCrits);
                    messagePanel.GetComponentInChildren<TextMeshProUGUI>().text = "Forsvar mislykkedes \n Kritisktræffer";
                }

            }
            enemyDices.GetComponent<Animator>().enabled = true;
            enemyDices.GetComponent<Animator>().SetBool("Start", false);
            return;
            
        }
        else if (playerRoll < enemyRoll)
        {
            if (attacking)
            {
                playerDices.GetComponent<Animator>().enabled = true;
                playerDices.GetComponent<Animator>().SetBool("Attack", true);
                movingCharacter = enemyObject;
                target = playerObject;
                enemyDices.GetComponent<Animator>().SetBool("PlayerAttack", true);
                playerWin = false;
                messagePanel.GetComponentInChildren<TextMeshProUGUI>().text = "Angreb mislykkedes";

                int enemyCritChance = enemy.GetComponent<EnemyStats>().CritChance;
                int random = Random.Range(1, 101);
                Debug.Log("this luck no " + random);
                Debug.Log("Ene critt chance" + enemyCritChance);
                if (enemyCritChance >= random)
                {
                    enemyCrits = true;
                    Debug.Log("enemy crit on chance: " + enemyCritChance + " selected number: " + random);
                    Debug.Log(enemyCrits);
                    messagePanel.GetComponentInChildren<TextMeshProUGUI>().text = "Angreb mislykkedes \n Kritisktræffer";
                }

            }
            else
            {
                enemyDices.GetComponent<Animator>().SetBool("Defended", true);
                enemyDices.GetComponent<Animator>().SetBool("PlayerAttack", false);
                playerWin = true;
                messagePanel.GetComponentInChildren<TextMeshProUGUI>().text = "Forsvar lykkedes";
            }
            enemyDices.GetComponent<Animator>().enabled = true;
            enemyDices.GetComponent<Animator>().SetBool("Start", false);
            return;
        }
        messagePanel.GetComponentInChildren<TextMeshProUGUI>().text = "Uafgjort";
        OptionsPanel.SetActive(false);
        combatReset = true;
    }

    private async void DrawMessageDisapear()
    {
        await Task.Delay(500);
        messagePanel.GetComponent<Animator>().SetBool("Show", false);
    }

    private void CombatEnded(bool victory)
    {
        if(victory)
        {
            PlayerStats.Instance.Level++;
            chest.GetComponent<ChestChance>().DropChest();
            chest.GetComponent<ChestChance>().TotalRewards();

            if(enemyObject.GetComponent<EnemyStats>().FinalBoss)
            {
                GameFinishedPanel();
            }
        }
        else
        {
            GameFinishedPanel();
            
        }
    }

    private void GameFinishedPanel()
    {
        GameOverPanel.SetActive(true);
        GameOverPanel.transform.SetAsLastSibling();
        GameOverPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Tillykke du har vundet " + UIManager.Instance.DisplayMoney(PlayerData.Instance.CombatBonusReward);
    }

    public void EnteringNewLevel()
    {
        if (PlayerStats.Instance.Level <= 5)
        {
            PlayerStats.Instance.Health = PlayerStats.Instance.MaxHealth;
            SceneManager.LoadScene("AdventureGameLoadLevelScene");
        }
        else GameFinishedPanel();
    }

    public void CharacterAttack()
    {
        OptionsPanel.SetActive(false);
        if(movingCharacter == null)
        {
            combatReset = true;
            return;
        }
        movingCharacter.GetComponent<Animator>().SetBool("Run", true);
        dicesDone = true;
    }

    private void Update()
    {
        if (movingCharacter != null && !attackFinished && dicesDone)
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
      
            if (playerCritt == true)
            {
                attacker.GetComponent<Animator>().SetBool("CritAttack", true);
                yield return new WaitForSeconds(0.4f);
                attacker.GetComponent<Animator>().SetBool("CritAttack", false);
                yield return new WaitForSeconds(0.2f);
                Turn(attacker);
                characterMoveBack = true;

                Debug.Log("I attack" + playerCritt);

            }
            else
            {
                playerCritt = false;
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
        if(enemyObject.GetComponent<EnemyStats>().Health > 0) enemyDices.GetComponent<Animator>().SetBool("Start", true);
        playerDices.GetComponent<Animator>().SetBool("Start", true);
        playerDices.GetComponent<Animator>().SetBool("Attack", false);
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
        dicesDone = false;
        combatUI.ResetOptionsUI(OptionsPanel);
        optionsAnimator.SetBool("Defend", false);
        optionsAnimator.SetBool("Attack", false);
        enemyDices.GetComponent<Animator>().SetBool("Start", true);
        enemyDices.GetComponent<Animator>().SetBool("LostAttack", false);
        enemyDices.GetComponent<Animator>().SetBool("PlayerHasChosen", false);
        playerDices.GetComponent<Animator>().SetBool("Start", false);
        optionsAnimator.transform.GetChild(0).GetComponent<Animator>().SetBool("AttackPicked", false);
        optionsAnimator.transform.GetChild(1).GetComponent<Animator>().SetBool("DefendPicked", false);
        optionsAnimator.transform.GetChild(0).GetComponent<Animator>().enabled = false;
        optionsAnimator.transform.GetChild(1).GetComponent<Animator>().enabled = false;
        optionsAnimator.transform.GetChild(0).gameObject.SetActive(true);
        optionsAnimator.transform.GetChild(1).gameObject.SetActive(true);
    }
}
