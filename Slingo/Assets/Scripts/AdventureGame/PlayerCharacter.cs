using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public GameObject playerPanel;

    public void PlayerTakeDamage()
    {
        GetComponent<Animator>().SetBool("TakeDamage", true);
        playerPanel.GetComponent<Animator>().SetBool("TookDamage", true);
    }

    public void PlayerReset()
    {
        GetComponent<Animator>().SetBool("TakeDamage", false);
        playerPanel.GetComponent<Animator>().SetBool("TookDamage", false);
    }
}
