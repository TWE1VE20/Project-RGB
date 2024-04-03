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
        // 반사 애니메이션
        OnReflection?.Invoke();
    }
}
