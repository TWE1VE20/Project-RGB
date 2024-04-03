using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] ShootingPoint shootingpoint;

    [Header("Weapons")]
    public List<Weapons> weaponsList;

    public int current = 0;
    private bool reloading = false;
    private bool attacking = false;

    private IEnumerator reload;
    private IEnumerator attack;

    [Header("Debug")]
    [SerializeField] bool debug;

    public bool Attack() 
    {
        if(reloading == true)
        {
            StopCoroutine(reload);
            if (debug) Debug.Log("Reload failed");
            reloading = false;
        }
        if (attacking != true) 
        {
            attack = AttackWait(weaponsList[current].attackTime);
            StartCoroutine(attack);
            return true;
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

    IEnumerator AttackWait(float time)
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
}
