using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void TakeDamage(int damage);
    public void TakeDamage(int damage, Vector3 EnemyPosition);
}
