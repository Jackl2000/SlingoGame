using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public GameObject playerPanel;

    private TextMeshProUGUI damageText;

    private void Start()
    {
        damageText = GetComponentInChildren<TextMeshProUGUI>(true);
    }
    public void PlayerTakeDamage(int damage)
    {
        GetComponent<Animator>().SetBool("TakeDamage", true);
        damageText.gameObject.SetActive(true);
        damageText.text = damage.ToString();
        damageText.gameObject.GetComponent<Animator>().SetBool("DamageTaken", true);
        PlayerStats.Instance.Health -= damage;
        if(PlayerStats.Instance.Health <= 0)
        {
            GetComponent<Animator>().SetBool("Death", true);
        }
        playerPanel.GetComponent<Animator>().SetBool("TookDamage", true);
    }

    public void PlayerReset()
    {
        GetComponent<Animator>().SetBool("TakeDamage", false);
        playerPanel.GetComponent<Animator>().SetBool("TookDamage", false);
        damageText.gameObject.GetComponent<Animator>().SetBool("DamageTaken", false);
        damageText.gameObject.SetActive(false);
    }
}
