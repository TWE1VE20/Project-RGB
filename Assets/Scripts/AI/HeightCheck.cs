using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightCheck : MonoBehaviour
{
    [SerializeField] Fly fly;
    [SerializeField] GameObject HeightChecker;

    public float raycastLength = 10.0f; // Ray�� ����
    public void Update()
    {
        // Ray�� ����
        Ray ray = new Ray(HeightChecker.transform.position, HeightChecker.transform.forward);
        // Raycast�� �����ϰ� �浹 ������ ����
        if (Physics.Raycast(ray, out RaycastHit hit, raycastLength))
        {
            // �浹�� ������Ʈ�� Transform.position �� ���
            Debug.Log("�浹�� ������Ʈ ��ġ: " + hit.transform.position);
            Debug.DrawRay(transform.position, HeightChecker.transform.forward, Color.red);
            fly.realHeight = hit.transform.position.y + fly.targetHeight;
        }
    }
    
}
