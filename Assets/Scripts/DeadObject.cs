using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class DeadObject : MonoBehaviour
{
    
    [SerializeField] LayerMask playerLayer;
    /*[SerializeField] PlayerController[] playerControllers*/
    public void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

            playerController.IsAlive = false;
        }
    }
}
