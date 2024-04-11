using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBanging : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1.0f; // 회전 속도
    [SerializeField] private float rotationAngle = 30.0f; // 회전 각도 (도)
    [SerializeField] private float rotationWait = 3.0f;

    private bool isRotating = false; // 회전 중인지 여부
    private float currentRotationAngle = 0.0f; // 현재 회전 각도

    void Update()
    {
        if (!isRotating)
        {
            StopAllCoroutines();
            // 회전 시작
            StartCoroutine(RotateObject());
        }
    }

    private IEnumerator RotateObject()
    {
        isRotating = true;

        // 회전 방향 설정
        bool isForward = true;

        while (true)
        {
            // 현재 바라보고 있는 방향 계산
            Vector3 forward = transform.forward;
            forward.y = 0.0f; // Y축 방향 제외

            // 현재 회전 각도 계산
            float targetRotationAngle;
            if (isForward)
            {
                targetRotationAngle = Vector3.Angle(forward, transform.forward) + rotationAngle;
            }
            else
            {
                targetRotationAngle = Vector3.Angle(forward, transform.forward) - rotationAngle;
            }

            // Lerp 함수를 사용하여 회전 보간
            while (Mathf.Abs(currentRotationAngle - targetRotationAngle) > 0.1f)
            {
                currentRotationAngle = Mathf.Lerp(currentRotationAngle, targetRotationAngle, rotationSpeed * Time.deltaTime);
                transform.localRotation = Quaternion.Euler(0f, currentRotationAngle, 0f);
                yield return null;
            }

            // 회전 방향 반전
            isForward = !isForward;

            // 잠시 대기
            yield return new WaitForSeconds(rotationWait);
            isRotating = false;
        }
    }

    //// 회전 속도
    //public float rotateSpeed = 10.0f;

    //// 최소 회전 각도
    //public float minAngle = -30.0f;

    //// 최대 회전 각도
    //public float maxAngle = 30.0f;

    //// 현재 회전 각도
    //private float currentAngle;

    //void Update()
    //{
    //    // 현재 y 좌표 기준 회전 각도 계산
    //    currentAngle = Mathf.Lerp(minAngle, maxAngle, Mathf.PingPong(Time.time * rotateSpeed, 1.0f));

    //    // y축 기준 회전
    //    transform.localRotation = Quaternion.Euler(0.0f, currentAngle, 0.0f);
    //}
    //임시 수식
}
