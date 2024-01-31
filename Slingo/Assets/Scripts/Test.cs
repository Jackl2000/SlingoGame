using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public event Action StuffHappens;

    
    public void DoStuff()
    {
        StuffHappens?.Invoke(); ;
    }

}
