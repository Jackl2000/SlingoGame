using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    [HideInInspector] public Sprite sprite;
    [HideInInspector] public RuntimeAnimatorController animatorController;

    public string Name { get; set; }
    public int Damage { get; set; }
    public int Health { get; set; }
    public int CritChance { get; set; }

    private TextMeshProUGUI damageText;

    private void Start()
    {
        sprite = GetComponent<Image>().sprite;
        animatorController = GetComponent<RuntimeAnimatorController>();
        damageText = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    public void EnemyTakeDamage(int damage)
    {
        GetComponent<Animator>().SetBool("TakeDamage", true);
        damageText.gameObject.SetActive(true);
        damageText.text = damage.ToString();
        Health -= damage;
    }

    public void EnemyHasTakenDamage()
    {
        GetComponent<Animator>().SetBool("TakeDamage", false);
        damageText.gameObject.SetActive(false);
        if (Health <= 0)
        {
            GetComponent<Animator>().SetBool("Death", true);
        }
    }
}
