using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] ShootingPoint shootingpoint;

    [Header("Weapons")]
    public List<Weapons> weaponsList;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI GunUI;
    [SerializeField] TextMeshProUGUI MeleeUI;

    public int current = 0;
    private bool reloading = false;
    private bool attacking = false;

    private IEnumerator reload;
    private IEnumerator attack;

    [Header("Debug")]
    [SerializeField] bool debug;

    private void Start()
    {
        ChangeWeapon();
    }

    private void Update()
    {
        if(weaponsList[current].attackType == Weapons.AttackType.GUN)
            UpdateUI();
    }

    public bool Attack() 
    {
        switch (weaponsList[current].attackType)
        {
            case Weapons.AttackType.GUN:
                if (reloading == true)
                {
                    StopCoroutine(reload);
                    if (debug) Debug.Log("Reload failed");
                    reloading = false;
                }
                if (attacking != true)
                {
                    attack = GunAttackWait(weaponsList[current].attackTime);
                    StartCoroutine(attack);
                    return true;
                }
                return false;
            case Weapons.AttackType.MELEE:
                if(attacking != true)
                {
                    attack = MeleeAttackWait(weaponsList[current].attackTime);
                    StartCoroutine(attack);
                    return true;
                }
                return false;
        }
        return false;
    }

    public void Reload()
    {
        if (weaponsList[current].attackType == Weapons.AttackType.GUN)
        {
            if(debug) Debug.Log("Reloading");
            reload = ReloadWait(weaponsList[current].GetComponent<AmmoSystem>().reloadTime);
            StartCoroutine(reload);
        }
        else
            if (debug) Debug.Log("Melee Don't have Reload");
    }
    public void Reflect() 
    {
        weaponsList[current].GetComponent<ReflectSystem>().Reflect();
    }
    IEnumerator ReloadWait(float time)
    {
        reloading = true;
        if (weaponsList[current].GetComponent<AmmoSystem>().CanReload() == false)
        {
            yield break;
        }
        yield return new WaitForSeconds(time);
        weaponsList[current].GetComponent<AmmoSystem>().Reload();
        reloading = false;
    }
    IEnumerator GunAttackWait(float time)
    {
        attacking = true;
        if (weaponsList[current].Attack() == false)
        {
            attacking = false;
            if (debug) Debug.Log("Attack failed");
            yield break;
        }
        shootingpoint.Fire();
        if (debug) Debug.Log("Attack Success");
        yield return new WaitForSeconds(time);
        attacking = false;
    }
    IEnumerator MeleeAttackWait(float time)
    {
        attacking = true;
        shootingpoint.CQC();
        if (debug) Debug.Log("Melee Attack Success");
        yield return new WaitForSeconds(time);
        attacking = false;
    }

    private void ChangeWeapon()
    {
        for(int i = 0; i < weaponsList.Count; i++)
        {
            if (i != current)
                weaponsList[i].gameObject.SetActive(false);
            else
                weaponsList[i].gameObject.SetActive(true);
        }
    }

    private void UpdateUI()
    {
        GunUI.text = string.Format("{0} / {1}", weaponsList[current].GetComponent<AmmoSystem>().rounds, weaponsList[current].GetComponent<AmmoSystem>().AmmoLeft);
    }

    private void OnWeapon1(InputValue value)
    {
        current = 0;
        ChangeWeapon();
        GunUI.gameObject.SetActive(true);
        MeleeUI.gameObject.SetActive(false);
    }

    private void OnWeapon2(InputValue value)
    {
        current = 1;
        ChangeWeapon();
        GunUI.gameObject.SetActive(false);
        MeleeUI.gameObject.SetActive(true);
    }
}
