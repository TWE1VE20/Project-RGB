using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeStamina : Slider
{
    protected override void Start()
    {
        base.Start();
        Manager.timestamina.staminaSlider = this;
    }
}
