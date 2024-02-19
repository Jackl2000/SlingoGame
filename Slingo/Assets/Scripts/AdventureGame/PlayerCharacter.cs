using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("CritAttack", false);
        if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetBool("Attack", true);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetBool("CritAttack", true);
        }
    }
    public void PlayerTakeDamage()
    {
        GetComponent<Animator>().SetBool("TakeDamage", true);
    }

    public void PlayerReset()
    {
        GetComponent<Animator>().SetBool("TakeDamage", false);
    }
}
