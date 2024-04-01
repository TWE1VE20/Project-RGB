using UnityEngine;

public class Weapons : MonoBehaviour
{
    public Colors colorState { get; private set; }
    public AttackType attackType { get; protected set; }

    public enum Colors { RED, GREEN, BLUE }
    public enum AttackType { MELEE, GUN }

    public float reloadTime { get; private set; }
    public float attackTime { get; private set; }

    public Weapons()
    {
        colorState = Colors.RED;
        reloadTime = 0;
    }

    protected void Starting()
    {
        colorState = Colors.RED;
        reloadTime = 0;
    }

    public virtual bool Attack() { return false; }
    public virtual void Reload() { }
    public virtual bool CanReload() { return false; }

    public void ChangeColor(bool isNext)
    {
        if (isNext)
            colorState = NextColor(colorState);
        else
            colorState = PrevColor(colorState);
    }

    public void SetReloadTime(float time)
    {
        reloadTime = time;
    }
    public void SetAttackTime(float time)
    {
        attackTime = time;
    }

    private Colors NextColor(Colors color)
    {
        switch (color)
        {
            case Colors.RED:
                return Colors.GREEN;
            case Colors.GREEN:
                return Colors.BLUE;
            case Colors.BLUE:
                return Colors.RED;
            default:
                return Colors.RED;
        }
    }
    private Colors PrevColor(Colors color)
    {
        switch (color)
        {
            case Colors.RED:
                return Colors.BLUE;
            case Colors.GREEN:
                return Colors.RED;
            case Colors.BLUE:
                return Colors.GREEN;
            default:
                return Colors.RED;
        }
    }
}