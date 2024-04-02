using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShootingPoint : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] WeaponHolder weaponHolder;

    [Header("Spec")]
    [SerializeField] float maxDistance;

    [Header("Debug")]
    [SerializeField] Transform hitPoint;
    [SerializeField] bool debug;

    private void Start()
    {
        if (debug)
            hitPoint.gameObject.SetActive(true);
        else
            hitPoint.gameObject.SetActive(false);
    }

    public void Fire()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, maxDistance))
        {
            if (debug)
            {
                Debug.Log("Hit!");
                Debug.Log(hitInfo.collider.gameObject.name);
                Debug.Log(hitInfo.distance);
                hitPoint.position = hitInfo.point;
                if (weaponHolder.weaponsList[weaponHolder.current].colorState == Weapons.Colors.RED)
                    Debug.DrawRay(transform.position, transform.forward * hitInfo.distance, Color.red, 0.1f);
                else if (weaponHolder.weaponsList[weaponHolder.current].colorState == Weapons.Colors.GREEN)
                    Debug.DrawRay(transform.position, transform.forward * hitInfo.distance, Color.green, 0.1f);
                else if (weaponHolder.weaponsList[weaponHolder.current].colorState == Weapons.Colors.BLUE)
                    Debug.DrawRay(transform.position, transform.forward * hitInfo.distance, Color.blue, 0.1f);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("Noting Hit");
                Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.gray, 0.1f);
            }
        }
    }

    [ContextMenu("Reflect")]
    public void Reflect() 
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, maxDistance))
        {
            if (debug)
            {
                Debug.Log("Hit!");
                Debug.Log(hitInfo.collider.gameObject.name);
                Debug.Log(hitInfo.distance);
                hitPoint.position = hitInfo.point;
                Debug.DrawRay(transform.position, transform.forward * hitInfo.distance, new Color(1,0,1,1), 0.1f);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("Noting Hit");
                Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.gray, 0.1f);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 forward = transform.forward;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + forward * 1);
    }
}
