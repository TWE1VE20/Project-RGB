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
                if (child.gameObject.GetComponent<Gun>() != null)
                {
                    // B 오브젝트의 탄약 5 증가
                    Gun gun = child.gameObject.GetComponent<Gun>();
                    if (gun != null)
                    {
                        //gun.initAmmoAmount = gun.maxRounds;
                    }
                }
            }

            // A 오브젝트 소멸
            // Destroy(gameObject);
        }
    }
}