using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightCheck : MonoBehaviour
{
    [SerializeField] Fly fly;
    [SerializeField] GameObject HeightChecker;

    public float raycastLength = 10.0f; // Ray의 길이
    public void Update()
    {
        // Ray를 방출
        Ray ray = new Ray(HeightChecker.transform.position, HeightChecker.transform.forward);
        // Raycast를 수행하고 충돌 정보를 저장
        if (Physics.Raycast(ray, out RaycastHit hit, raycastLength))
        {
            // 충돌한 오브젝트의 Transform.position 값 출력
            Debug.Log("충돌한 오브젝트 위치: " + hit.transform.position);
            Debug.DrawRay(transform.position, HeightChecker.transform.forward, Color.red);
            fly.realHeight = hit.transform.position.y + fly.targetHeight;
        }
    }
    
}
