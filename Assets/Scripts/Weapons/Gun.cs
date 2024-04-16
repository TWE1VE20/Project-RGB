using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapons
{
    [Header("Component")]
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] gunAudioClip;
    [SerializeField] AmmoSystem ammoSystem;

    [Header("Gun Status")]
    [SerializeField] float timeforReload;
    [SerializeField] float timeforAttack;
    [SerializeField] int initAmmoAmount;
    public int maxRounds;   // 탄창 최대 총알수

    private void Awake()
    {
        base.Starting();
        SetAttackTime(timeforAttack);
        this.attackType = Weapons.AttackType.GUN;
        if (ammoSystem == null)
        {
            ammoSystem = gameObject.AddComponent<AmmoSystem>();
        }
        ammoSystem.audioSource = this.audioSource;
        ammoSystem.reloadTime = this.timeforReload;
        ammoSystem.AmmoLeft = this.initAmmoAmount;
        ammoSystem.maxRounds = this.maxRounds;
    }

    public void OnEnable()
    {
        audioSource.clip = gunAudioClip[0];
        if (audioSource.clip != null)
            audioSource.Play();
    }

    public override bool Attack()
    {
        bool isShoot = ammoSystem.GunAttack();
        if (isShoot)
            audioSource.clip = gunAudioClip[1];
        else
            audioSource.clip = gunAudioClip[2];
        if (audioSource.clip != null)
            audioSource.Play();
        return isShoot;
    }


}
