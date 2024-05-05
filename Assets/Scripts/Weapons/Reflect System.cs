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
    public AudioClip reflectSoundClip;

    public void Reflect()
    {
        // 반사시스템
        audioSource.clip = reflectSoundClip;
        if (audioSource.clip != null)
            audioSource.Play();
        OnReflection?.Invoke();
    }
}
