using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBreakable
{
    public void Break();

    public void Break(Weapons.Colors color);
}
