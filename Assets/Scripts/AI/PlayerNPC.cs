using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNPC : MonoBehaviour, IDamagable
{
    [SerializeField] int hp;
    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log("attacked");
    }
}
