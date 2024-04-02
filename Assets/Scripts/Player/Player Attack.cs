using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] WeaponHolder weaponHolder;
    [SerializeField] ShootingPoint shootingpoint;

    [Header("Spec")]
    [SerializeField] float ColorChangeSpeed;
    private bool canColorChange;

    [Header("Debug")]
    [SerializeField] bool debug;

    public bool isGuard { get; private set; }

    private void Start()
    {
        canColorChange = true;
        isGuard = false;
    }
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

    private void OnChangeColor(InputValue value)
    {
        if (canColorChange != false && gameObject.GetComponent<PlayerController>().IsAlive)
        {
            Vector2 scroll = value.Get<Vector2>();
            if (scroll.y > 0)
                weaponHolder.weaponsList[weaponHolder.current].ChangeColor(false);
            else
                weaponHolder.weaponsList[weaponHolder.current].ChangeColor(true);
            StartCoroutine(ColorChangeDuration(ColorChangeSpeed));
        }
    }

    IEnumerator ColorChangeDuration(float time)
    {
        canColorChange = false;
        yield return new WaitForSeconds(time);
        canColorChange = true;
    }
}