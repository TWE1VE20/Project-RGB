using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] ShootingPoint shootingpoint;
    private void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Player Attacks");
            shootingpoint.Fire();
        }
    }
}
