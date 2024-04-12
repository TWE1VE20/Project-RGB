using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;

public class ShootingPoint : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] WeaponHolder weaponHolder;

    [Header("Spec")]
    [SerializeField] float maxDistance;
    [SerializeField] float maxDistance2;

    [Header("Particles")]
    [SerializeField] ParticleSystem[] particles;

    [Header("Debug")]
    [SerializeField] Transform hitPoint;
    [SerializeField] bool debug;
    [SerializeField] bool showHitPoint;

    private void Start()
    {
        if (hitPoint != null)
        {
            if (debug && showHitPoint)
                hitPoint?.gameObject.SetActive(true);
            else
                hitPoint?.gameObject.SetActive(false);
        }
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
            // 명중한 물체가 어떤 반응을 할지 추가
            hitInfo.collider.gameObject.GetComponent<IBreakable>()?.Break(weaponHolder.weaponsList[weaponHolder.current].colorState);
            hitInfo.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(weaponHolder.weaponsList[weaponHolder.current].colorState);
            if(particles != null)
                Instantiate(particles[0], hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
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

    public void CQC()
    {
        if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out RaycastHit hitInfo, maxDistance2))
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
            // 명중한 물체가 어떤 반응을 할지 추가
            hitInfo.collider.gameObject.GetComponent<IBreakable>()?.Break(weaponHolder.weaponsList[weaponHolder.current].colorState);
            hitInfo.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(weaponHolder.weaponsList[weaponHolder.current].colorState);
        }
        else
        {
            if (debug)
            {
                Debug.Log("Noting Hit");
                Debug.DrawRay(transform.position, transform.forward * maxDistance2, Color.gray, 0.1f);
            }
        }
    }

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
            hitInfo.collider.gameObject.GetComponent<IStunable>()?.Stun();
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
        Gizmos.DrawLine(transform.position, transform.position + forward * 0.5f);
    }
}
