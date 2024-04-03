using UnityEngine;

public class Weapons : MonoBehaviour
{
    public Colors colorState { get; private set; }
    public AttackType attackType { get; protected set; }

    public enum Colors { RED, GREEN, BLUE }
    public enum AttackType { MELEE, GUN }
    public float attackTime { get; private set; }
    protected void Starting()
    {
        colorState = Colors.RED;
    }
    public virtual bool Attack() { return false; }
    public void ChangeColor(bool isNext)
    {
        if (isNext)
            colorState = NextColor(colorState);
        else
            colorState = PrevColor(colorState);
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