using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    [Header("Fly")]
    [SerializeField] float targetHeight; // 부유할 높이
    [SerializeField] float amplitude; // 진폭
    [SerializeField] float period; // 주기(초)

    private void Update()
    {
        // 현재 시간
        float time = Time.time;

        // y 값 계산
        float y = targetHeight + amplitude * Mathf.Sin(2 * Mathf.PI * time / period);

        // 위치 업데이트
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
