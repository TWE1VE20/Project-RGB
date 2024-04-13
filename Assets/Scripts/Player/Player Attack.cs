using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour, IDamagable
{
    [Header("GameObject")]
    [SerializeField] WeaponHolder weaponHolder;

    [Header("Spec")]
    [SerializeField] float ColorChangeSpeed;
    [SerializeField] float angle;
    private bool canColorChange;

    [Header("Debug")]
    [SerializeField] bool debug;
    [SerializeField] bool NoDeath;

    public bool isGuard { get; private set; }

    private void Start()
    {
        canColorChange = true;
        isGuard = false;
    }

    public void TakeDamage(int damage)
    {
        gameObject.GetComponent<PlayerController>().IsAlive = false;
    }
    public void TakeDamage(int damage, Vector3 EnemyPosition)
    {
        if (gameObject.GetComponent<PlayerController>().IsAlive)
        {
            if (isGuard)
            {
                Vector3 dirToTarget = (EnemyPosition - gameObject.transform.position).normalized;
                if ((Vector3.Dot(transform.forward, dirToTarget) > Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad)))
                {
                    Debug.Log("Deflect");
                    weaponHolder.Reflect();
                    Debug.DrawRay(transform.position, dirToTarget, Color.black);
                }
                else
                {
                    if(!NoDeath)
                        gameObject.GetComponent<PlayerController>().IsAlive = false;
                    Debug.Log("Dead");
                }
            }
            else
            {
                if (!NoDeath)
                    gameObject.GetComponent<PlayerController>().IsAlive = false;
                Debug.Log("Dead");
            }
        }
    }
    public void TakeDamage(Weapons.Colors color)
    {
        return;
    }

    private void OnAttack(InputValue value)
    {
        if (gameObject.GetComponent<PlayerController>().IsAlive)
        {
            if (value.isPressed)
            {
                if (debug) Debug.Log("Player Attacks");
                weaponHolder.Attack();
            }
        }
    }

    private void OnReload(InputValue value)
    {
        weaponHolder.Reload();
    }

    private void OnGuard(InputValue value)
    {
        if (value.isPressed)
            if(weaponHolder.weaponsList[weaponHolder.current].attackType == Weapons.AttackType.MELEE)
                isGuard = true;
        else
            isGuard = false;
    }

    private void OnChangeColor(InputValue value)
    {
        if (canColorChange != false)
        {
            Vector2 scroll = value.Get<Vector2>();
            StartCoroutine(ColorChangeDuration(ColorChangeSpeed, scroll.y < 0));
        }
    }

    IEnumerator ColorChangeDuration(float time, bool next)
    {
        canColorChange = false;
        if (next)
            weaponHolder.weaponsList[weaponHolder.current].ChangeColor(true);
        else
            weaponHolder.weaponsList[weaponHolder.current].ChangeColor(false);
        yield return new WaitForSecondsRealtime(time);
        canColorChange = true;
    }
}
