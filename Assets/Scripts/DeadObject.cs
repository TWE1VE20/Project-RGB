using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadObject : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    public void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            
        }
    }
}
