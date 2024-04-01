using UnityEngine;

public class Weapons : MonoBehaviour
{
    protected Colors colorState { get; private set; }
    public AttackType attackType { get; protected set; }

    public enum Colors { RED, BLUE, GREEN }
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

    public void ChangeColor()
    {
        colorState = NextColor(colorState);
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