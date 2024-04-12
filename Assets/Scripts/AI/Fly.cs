using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fly : MonoBehaviour
{
    [Header("Fly")]
    [SerializeField] public float targetHeight; // ������� �Ÿ�
    [SerializeField] public float realHeight; // ���� ����
    [SerializeField] float amplitude; // ����
    [SerializeField] float period; // �ֱ�(��)
    [SerializeField] EnemyAI enemy;
    
    private void Update()
    {
        // ���� �ð�
        float time = Time.time;

        // y �� ���
        //float y = realHeight + amplitude * Mathf.Sin(2 * Mathf.PI * time / period);
        float y = targetHeight + amplitude * Mathf.Sin(2 * Mathf.PI * time / period);

        // ��ġ ������Ʈ
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
