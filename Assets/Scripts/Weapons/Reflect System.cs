using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReflectSystem : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnReflection;

    public Animator animator;
    public AudioSource audioSource;

    public void Reflect()
    {
        // �ݻ� �ִϸ��̼�
        OnReflection?.Invoke();
    }
}
