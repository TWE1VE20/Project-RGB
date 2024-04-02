using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Razor : MonoBehaviour
{
    public CapsuleCollider capsuleCollider; // SphereCollider 컴포넌트
    public float shrinkSpeed = 0.1f; // 반지름 감소 속도 (초당 단위)

    private void Start()
    {
        if (capsuleCollider == null)
        {
            Debug.LogError("SphereCollider 또는 MeshCollider 컴포넌트가 필요합니다.");
            return;
        }
    }

    private void Update()
    {
        if (capsuleCollider != null)
        {
            capsuleCollider.radius -= shrinkSpeed * Time.deltaTime;
        }
    }
}
