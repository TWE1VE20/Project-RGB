using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBanging : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1.0f; // ȸ�� �ӵ�
    [SerializeField] private float rotationAngle = 30.0f; // ȸ�� ���� (��)
    [SerializeField] private float rotationWait = 3.0f;

    private bool isRotating = false; // ȸ�� ������ ����
    private float currentRotationAngle = 0.0f; // ���� ȸ�� ����

    void Update()
    {
        if (!isRotating)
        {
            StopAllCoroutines();
            // ȸ�� ����
            StartCoroutine(RotateObject());
        }
    }

    private IEnumerator RotateObject()
    {
        isRotating = true;

        // ȸ�� ���� ����
        bool isForward = true;

        while (true)
        {
            // ���� �ٶ󺸰� �ִ� ���� ���
            Vector3 forward = transform.forward;
            forward.y = 0.0f; // Y�� ���� ����

            // ���� ȸ�� ���� ���
            float targetRotationAngle;
            if (isForward)
            {
                targetRotationAngle = Vector3.Angle(forward, transform.forward) + rotationAngle;
            }
            else
            {
                targetRotationAngle = Vector3.Angle(forward, transform.forward) - rotationAngle;
            }

            // Lerp �Լ��� ����Ͽ� ȸ�� ����
            while (Mathf.Abs(currentRotationAngle - targetRotationAngle) > 0.1f)
            {
                currentRotationAngle = Mathf.Lerp(currentRotationAngle, targetRotationAngle, rotationSpeed * Time.deltaTime);
                transform.localRotation = Quaternion.Euler(0f, currentRotationAngle, 0f);
                yield return null;
            }

            // ȸ�� ���� ����
            isForward = !isForward;

            // ��� ���
            yield return new WaitForSeconds(rotationWait);
            isRotating = false;
        }
    }

    //// ȸ�� �ӵ�
    //public float rotateSpeed = 10.0f;

    //// �ּ� ȸ�� ����
    //public float minAngle = -30.0f;

    //// �ִ� ȸ�� ����
    //public float maxAngle = 30.0f;

    //// ���� ȸ�� ����
    //private float currentAngle;

    //void Update()
    //{
    //    // ���� y ��ǥ ���� ȸ�� ���� ���
    //    currentAngle = Mathf.Lerp(minAngle, maxAngle, Mathf.PingPong(Time.time * rotateSpeed, 1.0f));

    //    // y�� ���� ȸ��
    //    transform.localRotation = Quaternion.Euler(0.0f, currentAngle, 0.0f);
    //}
    //�ӽ� ����
}
