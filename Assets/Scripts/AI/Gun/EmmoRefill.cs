using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmmoRefill : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 Player인지 확인
        if (other.gameObject.layer == 3)
        {
            // C 오브젝트의 모든 자식 오브젝트 반복
            foreach (Transform child in other.gameObject.transform)
            {
                // 자식 오브젝트가 B 오브젝트인지 확인
                if (child.gameObject.GetComponent<AmmoSystem>() != null)
                {
                    // B 오브젝트의 탄약 증가
                    AmmoSystem ammo = child.gameObject.GetComponent<AmmoSystem>();
                  if (ammo != null)
                  {
                      ammo.AmmoLeft = 48;
                      // 소지가능 탄약 최대값 입력 요망(플레이어측)
                  }
                }
            }

            //A 오브젝트 소멸여부 선택
            Destroy(gameObject);
        }
    }
}