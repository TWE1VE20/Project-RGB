using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] WeaponHolder weaponHolder;
    [SerializeField] ShootingPoint shootingpoint;

    [Header("Debug")]
    [SerializeField] bool debug;
    private void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            if(debug) Debug.Log("Player Attacks");
            shootingpoint.Fire();
            weaponHolder.Attack();
        }
    }

    private void OnReload(InputValue value)
    {
        weaponHolder.Reload();
    }
}
