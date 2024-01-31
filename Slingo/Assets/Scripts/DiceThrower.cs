using JetBrains.Annotations;
using log4net.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class DiceThrower : MonoBehaviour
{


    public Dice diceToThrow;
    public int amountOfDice = 2;
    public float throwForce = 8f;
    public float rollForce = 10f;
    public int currentValue;

    public DiceGameUI gameUI;
    private List<GameObject> spawnedDice = new List<GameObject>();


    /*
     * Change keycode in the future. The space if for testing only
     * 
     */
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) PlayerRollDice();
    }

    public async Task RollDice()
    {   
        //Checks if there are dices to throw
        if (diceToThrow == null)
        {
            Debug.LogError("Dice To Throw is null");
            return;
        }
        //Desroys previos dices
        foreach (var die in spawnedDice)
        {
            Destroy(die);
        }

        //Spawns and throws dices with a slight delay between each dice.
        for (int i = 0; i < amountOfDice; i++)
        {
            Dice dice = Instantiate(diceToThrow, transform.position, transform.rotation);
            spawnedDice.Add(dice.gameObject);
            dice.RollDice(throwForce, rollForce, i);
            await Task.Delay(1000);

        }

    }
    /*
     * Calculates the total value from the topfaces of the dices. 
     * Saves the value
     */

    public int TotalValueOfDices()
    {
        int totalValue = 0;

        // Iterate over a copy of the list to avoid issues with modifying the list during iteration
        foreach (var die in new List<GameObject>(spawnedDice))
        {
            // Check if the GameObject is null (destroyed)
            if (die == null)
                continue;

            // Get the Dice component
            Dice diceComponent = die.GetComponent<Dice>();

            // Check if the Dice component is null (destroyed)
            if (diceComponent == null)
                continue;

            // Access the properties or methods of the Dice component
            int topFaceValue = diceComponent.GetNumberOnTopFace();
            totalValue += topFaceValue;
        }

        return totalValue;
    }

    public async void PlayerRollDice()
    {
        //Rolls dices
        await RollDice();
        //Waits for dices to Stop
        await WaitForDiceToStop();
        //Gets and saves the total value of the topface.
        int playerTotalValue = TotalValueOfDices();
        Debug.Log(playerTotalValue);
        //Show players result in UI
        gameUI.ShowPlayerResult(playerTotalValue);
    }

    public async Task<int> oppponentRollDice()
    {
        //Rolls the dices
        await RollDice();
        //Waits for dices to stop
        await WaitForDiceToStop();
        //Saves the TopFace value of the dices
        int opponentTotalValue = TotalValueOfDices();
        //Makes sure there isn't a previous value
        if(currentValue != 0)
        {
            currentValue = 0;
        }
        currentValue = opponentTotalValue;
        //Dobbelt checks that the values got taken in
        if(currentValue == 0)
        {
            currentValue = TotalValueOfDices();
        }
        //Returns dice value
        return currentValue;
    }
  
    private async Task WaitForDiceToStop()
    {
        bool allDiceStopped;

        do
        {
            allDiceStopped = true;

            foreach (var die in spawnedDice)
            {
                if (die == null)
                {
                    continue;
                }

                Dice diceComponent = die.GetComponent<Dice>();

                if (diceComponent == null)
                    continue;

                Rigidbody rb = diceComponent.GetComponent<Rigidbody>();

                if (rb.velocity.sqrMagnitude > 0.001f)
                {
                    allDiceStopped = false;
                    break; // At least 1 dice is moving
                }
            }

            if (!allDiceStopped)
            {
                await Task.Delay(100); // Adjust the delay duration as needed
            }

        } while (!allDiceStopped);
    }

    //Was only for testing
    //Should be deleted in the end.
    public int DelayPrDice()
    {
        int delay = 0;
        int delayMultipier = 500;

        delay += (amountOfDice * delayMultipier);

        return delay;
    }
    

}
