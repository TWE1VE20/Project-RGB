using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapons
{
    [Header("Component")]
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AmmoSystem ammoSystem;

    [Header("Gun Status")]
    [SerializeField] float timeforReload;
    [SerializeField] float timeforAttack;
    [SerializeField] int initAmmoAmount;
    public int maxRounds;   // 탄창 최대 크기

    private void Awake()
    {
        base.Starting();
        SetAttackTime(timeforAttack);
        this.attackType = Weapons.AttackType.GUN;
        if (ammoSystem == null)
        {
            ammoSystem = gameObject.AddComponent<AmmoSystem>();
        }
        ammoSystem.animator = this.animator;
        ammoSystem.audioSource = this.audioSource;
        ammoSystem.reloadTime = this.timeforReload;
        ammoSystem.AmmoLeft = this.initAmmoAmount;
        ammoSystem.maxRounds = this.maxRounds;
    }

    public override bool Attack()
    {
        return ammoSystem.GunAttack();
    }


}
