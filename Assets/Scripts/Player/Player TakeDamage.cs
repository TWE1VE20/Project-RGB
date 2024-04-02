using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTakeDamage : IDamagable
{
    [Header("Components")]
    [SerializeField] PlayerAttack playerattack;
    [SerializeField] ShootingPoint shootingPoint;

    [Header("Events")]
    public UnityEvent OnPlayerDead;

    public void TakeDamage(int damage)
    {
        OnPlayerDead?.Invoke();
    }

    public void TakeDamage(int damage, Vector3 EnemyPosition)
    {
        if (playerattack.isGuard)
        {
            shootingPoint.Reflect();
        }
        else
            OnPlayerDead?.Invoke();
    }

    public void TakeDamage(Weapons.Colors color)
    {
        return;
    }
}
