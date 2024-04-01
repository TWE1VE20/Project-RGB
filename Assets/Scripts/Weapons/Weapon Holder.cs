using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
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
            reload = ReloadWait(weaponsList[current].reloadTime);
            StartCoroutine(reload);
        }
        else
            if (debug) Debug.Log("Melee Don't have Reload");
    }
    IEnumerator ReloadWait(float time)
    {
        reloading = true;
        if (weaponsList[current].CanReload() == false)
        {
            yield break;
        }
        yield return new WaitForSeconds(time);
        weaponsList[current].Reload();
        reloading = false;
    }

    IEnumerator AttackWait(float time)
    {
        attacking = true;
        if (weaponsList[current].Attack() == false)
        {
            attacking = false;
            if (debug) Debug.Log("no ammo");
            yield break;
        }
        if (debug) Debug.Log("Bang!");
        yield return new WaitForSeconds(time);
        attacking = false;
    }
}
