using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public event Action CardFlipped;

    public string cardText { get; set; }
    public GameObject LostGameObject { get; set; }
}
