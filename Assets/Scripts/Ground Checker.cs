using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] SphereCollider groundCollider;
    [SerializeField] float radius;
    [SerializeField] float distance;

    [Header("Debug")]
    [SerializeField] bool debug;

    private bool isHit;
    private Vector3 hitPos;
    private float hitDistance;
    public bool isGround { get; private set; }

    private void FixedUpdate()
    {
        isGround = Raycast();
    }

    bool Raycast()
    {
        isHit = Physics.SphereCast(transform.position, radius, Vector3.down, out RaycastHit hit, distance, groundLayer);
        hitPos = hit.point;
        hitDistance = hit.distance;
        return isHit;
    }

    private void OnDrawGizmosSelected()
    {
        if (debug)
        {
            if (isHit)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + Vector3.down * hitDistance);
                Gizmos.DrawWireSphere(hitPos + Vector3.up * radius, radius);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + Vector3.down * distance);
                Gizmos.DrawWireSphere(transform.position + Vector3.down * distance, radius);
            }
        }
    }
}
