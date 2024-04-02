using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Razor : MonoBehaviour
{
    public CapsuleCollider capsuleCollider; // SphereCollider ������Ʈ
    public float shrinkSpeed = 0.1f; // ������ ���� �ӵ� (�ʴ� ����)

    private void Start()
    {
        if (capsuleCollider == null)
        {
            Debug.LogError("SphereCollider �Ǵ� MeshCollider ������Ʈ�� �ʿ��մϴ�.");
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
