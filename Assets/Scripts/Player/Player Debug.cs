using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerDebug : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] PlayerAttack playerAttack;

    [Header("Debug")]
    [SerializeField] bool debug;
    [SerializeField] GameObject TestObject;

    [Header("Events")]
    public UnityEvent OnReflectdebuging;

    private void Start()
    {
        if (!debug)
            gameObject.SetActive(false);
    }

    private void OnReflectDebug() 
    {
        OnReflectdebuging?.Invoke();
    }

    private void OnGetHit()
    {
        if(TestObject != null)
            playerAttack.TakeDamage(1, TestObject.transform.position);
    }
}
