using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapons
{
    [Header("Componets")]
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] ReflectSystem reflectSystem;

    [Header("meleeStatus")]
    [SerializeField] float timeforAttack;

    private void Awake()
    {
        base.Starting();
        this.attackType = Weapons.AttackType.MELEE;
        SetAttackTime(timeforAttack);
        if (reflectSystem == null)
        {
            reflectSystem = gameObject.AddComponent<ReflectSystem>();
        }
        reflectSystem.animator = this.animator;
        reflectSystem.audioSource = this.audioSource;
    }

    public override bool Attack()
    {
        // 근접공격 모션
        return true;
    }
}
