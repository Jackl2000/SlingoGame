using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dices : MonoBehaviour
{
    public Sprite[] diceFaces; //Array for diceFaces
    public GameObject dice1;
    public GameObject dice2;
    public GameObject dice3;
    public float rollInterval = 1f; //Interval between dice rolls
    public int hp;
    public int damage;
    public int luck;
    public TextMeshProUGUI textHPNo;
    public TextMeshProUGUI textDamageNo;
    public TextMeshProUGUI textLuckNo;


    private SpriteRenderer[] diceRenderers;
    private bool isRolling = false;
    private int[] diceResults;
    private bool[] statAssigned;
    private int currentDiceIndex = 0;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        diceRenderers = new SpriteRenderer[3]; //3 dices, 3 renderers
        diceResults = new int[3];

        //Get sprite renderer for each dice
        diceRenderers[0] = dice1.GetComponent<SpriteRenderer>();
        diceRenderers[1] = dice2.GetComponent<SpriteRenderer>();
        diceRenderers[2] = dice3.GetComponent<SpriteRenderer>();
        RollAllDice();
        animator = GetComponent<Animator>();
        statAssigned = new bool[3];
        //RollDice();

        for(int i = 0; i < diceResults.Length; i++)
        {
            diceResults[i] = 0;
            statAssigned[i] = false;
        }

    }

    public void SelectStat(string stat)
    {
        if(currentDiceIndex < 3)
        {
            int TotalValue = 0;
            foreach(var die in diceResults)
            {
                TotalValue += die;
            }

            switch (stat)
            {

                case "Liv:":
                    textHPNo.text = TotalValue.ToString();
                    break;
                case "Skade:":
                    textDamageNo.text = TotalValue.ToString();
                    break;
                case "Held:":
                    textLuckNo.text = TotalValue.ToString();
                    break;
                default:
                    Debug.LogError("invalid stat");
                    break;
            }

            textHPNo.GetComponentInChildren<Button>().enabled = false;
            textDamageNo.GetComponentInChildren<Button>().enabled = false;
            textLuckNo.GetComponentInChildren<Button>().enabled = false;

            currentDiceIndex++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            RollAllDice();
        }
    }


    public void RollAllDice()
    {
        //Run Reset before next roll
        ResetDiceState();

        //Roll each dice one by one
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(RollDiceCoroutine(i, UnityEngine.Random.Range(0, diceFaces.Length)));
        }
        if (Convert.ToInt32(textHPNo.text) == 1)
        {
            textHPNo.GetComponentInChildren<Button>().enabled = true;
        }
        if (Convert.ToInt32(textDamageNo.text) == 1)
        {
            textDamageNo.GetComponentInChildren<Button>().enabled = true;
        }
        if (Convert.ToInt32(textLuckNo.text) == 1)
        {
            textLuckNo.GetComponentInChildren<Button>().enabled = true;
        }




    }

    private IEnumerator RollDiceCoroutine(int index, int randomIndex)
    {
        diceRenderers[index].GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(index * rollInterval + 2f);

        diceRenderers[index].sprite = diceFaces[randomIndex];
        diceResults[index] = randomIndex + 1;
        diceRenderers[index].GetComponent<Animator>().enabled = false;
        diceRenderers[index].transform.rotation = Quaternion.identity;
        Debug.Log("Dice " + (index + 1) + "Result " + diceResults[index]);

        DiceReset(diceRenderers[index].gameObject, diceResults[index]);

        if (index == 2)
        {
            CalculateTotalValueOfDices();
        }

    }

    public void RollOneDice()
    {
        int randomDiceIndex = UnityEngine.Random.Range(0, diceFaces.Length);
        Debug.Log(randomDiceIndex);
    }

    public void CalculateTotalValueOfDices()
    {
        int total = 0;
        foreach (int result in diceResults)
        {
            total += result;
        }
        Debug.Log("Samlet værdi af slaget: " + total);
    }

    public void ResetDiceState()
    {
        //Reset the visual state of all dice
        foreach(SpriteRenderer rendere in diceRenderers)
        {
            rendere.sprite = null;
        }
        //Reset the result of state
        for(int i = 0; i < diceResults.Length; i++)
        {
            diceResults[i] = 0;
        }
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

}
