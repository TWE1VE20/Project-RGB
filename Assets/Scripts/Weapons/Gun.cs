using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapons
{
    [Header("Component")]
    [SerializeField] Animator animator;

    [Header("Gun Status")]
    public int AmmoLeft;        // ���� �Ѿ� ����
    public int maxRounds;   // źâ �ִ� ũ��
    public int rounds { get; private set; }     // ���� �ѿ� ����ִ� �Ѿ� ����

    public Gun() : base()
    {
        this.attackType = Weapons.AttackType.MELEE;
        this.rounds = maxRounds;
    }

    public override void Attack()
    {
        if(rounds > 0) 
        {
            rounds--;
            // Shoot Animation
        }
        else
        {
            // no rounds Animation and Sounds
        }
    }

    public void Reload() 
    {
        /*
        if(Ammo > maxRounds)
        {
            rounds = maxRounds;
            AmmoLeft -= maxRounds;
        }
        else
        {
            rounds = AmmoLeft;
            Ammo = 0;
        }
        */
        rounds = maxRounds;
    }
}
