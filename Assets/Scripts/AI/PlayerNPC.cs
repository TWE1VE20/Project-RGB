using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNPC : MonoBehaviour, IDamagable
{
    [SerializeField] int hp;
    [SerializeField] PopUpUI DeadUIPrefab;
    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log("attacked");
        if ( hp < 0)
        {
            Manager.UI.ShowPopUpUI(DeadUIPrefab);
        }
    }

    public void TakeDamage(int damage, Vector3 EnemyPosition)
    {
        hp -= damage;
        Debug.Log("attacked");
        if (hp < 0)
        {
            Manager.UI.ShowPopUpUI(DeadUIPrefab);
        }

    }
    public void TakeDamage(Weapons.Colors color)
    {
        return;
    }
}
