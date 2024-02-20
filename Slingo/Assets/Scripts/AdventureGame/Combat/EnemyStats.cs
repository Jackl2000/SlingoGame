using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    [HideInInspector] public Sprite sprite;
    [HideInInspector] public RuntimeAnimatorController animatorController;

    public int Damage { get; set; }
    public int Health { get; set; }
    public int CritChance { get; set; }

    private void Start()
    {
        sprite = GetComponent<Image>().sprite;
        animatorController = GetComponent<RuntimeAnimatorController>();
    }

    public void EnemyTakeDamage()
    {
        GetComponent<Animator>().SetBool("TakeDamage", true);
    }

    public void EnemyHasTakenDamage()
    {
        GetComponent<Animator>().SetBool("TakeDamage", false);
        if (Health <= 0)
        {
            GetComponent<Animator>().SetBool("Death", true);
        }
    }

    public void EnemyDeath()
    {

    }
}
