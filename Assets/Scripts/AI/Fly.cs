using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fly : MonoBehaviour
{
    [Header("Fly")]
    [SerializeField] public float targetHeight; // 지상과의 거리
    [SerializeField] public float realHeight; // 실제 높이
    [SerializeField] float amplitude; // 진폭
    [SerializeField] float period; // 주기(초)
    [SerializeField] EnemyAI enemy;
    
    private void Update()
    {
        // 현재 시간
        float time = Time.time;

        // y 값 계산
        //float y = realHeight + amplitude * Mathf.Sin(2 * Mathf.PI * time / period);
        float y = targetHeight + amplitude * Mathf.Sin(2 * Mathf.PI * time / period);

        // 위치 업데이트
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
