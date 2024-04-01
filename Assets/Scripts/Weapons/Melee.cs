using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapons
{
    public Melee() : base()
    {
        this.attackType = Weapons.AttackType.MELEE;
    }

    public override void Attack()
    {
        // 근접공격 모션
    }
}
