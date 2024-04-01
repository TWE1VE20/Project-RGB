using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapons : MonoBehaviour
{
    protected Colors colorState { get; private set; }
    protected AttackType attackType;

    public enum Colors { RED, BLUE, GREEN }
    public enum AttackType { MELEE, GUN }

    public Weapons()
    {
        colorState = Colors.RED;
    }

    public virtual void Attack(){ }

    public void ChangeColor() 
    {
        colorState = NextColor(colorState);
    }

    private Colors NextColor(Colors color)
    {
        switch(color)
        {
            case Colors.RED:
                return Colors.BLUE;
            case Colors.BLUE:
                return Colors.GREEN;
            case Colors.GREEN:
                return Colors.RED;
            default:
                return Colors.RED;
        }
    }
}