using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public GridCheck gridCheck;
    public spin spin;
    public void SlingoBoardHideAnimationEvent()
    {
        gridCheck.SlingoBorderGoIdle();
    }

    public void SpinButtonChangeAnimationEvent()
    {
        spin.ChangeSpinButton();
    }

    public void SpinButtonChangeEndEvent()
    {
        spin.SpinButtonChangeFinished();
    }
}
