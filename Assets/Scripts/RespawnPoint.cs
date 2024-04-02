using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour 
{
    [SerializeField] GameObject grave;
    public void OnTriggerEnter(Collider other)
    {
        Manager.timeflow.respawnPoint = grave.transform.position;
    }
}
