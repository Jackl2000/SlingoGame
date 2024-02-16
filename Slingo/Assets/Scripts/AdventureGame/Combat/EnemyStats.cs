using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    [HideInInspector] public Sprite sprite;
    [HideInInspector] public AnimatorController animator;

    public int Damage { get; set; }
    public int Health { get; set; }
    public int CritChance { get; set; }

    private void Start()
    {
        sprite = GetComponent<Image>().sprite;
        animator = GetComponent<AnimatorController>();
    }
}
