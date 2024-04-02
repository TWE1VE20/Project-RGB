using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerDebug : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] bool debug;

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
}
