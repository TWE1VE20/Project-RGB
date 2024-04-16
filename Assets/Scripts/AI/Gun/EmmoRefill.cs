using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EmmoRefill : MonoBehaviour
{
    // Gun Tag 시스템에서 사용하는 태그 이름
    private const string GUN_TAG_NAME = "Gun";

    
    void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 Player인지 확인
        if (other.gameObject.layer == 3)
        {
            // 현재 씬에서 Gun Tag 오브젝트를 찾음
            GameObject[] gunTagObjects = GameObject.FindGameObjectsWithTag(GUN_TAG_NAME);
            foreach (GameObject gunTagObject in gunTagObjects)
            {
                // 자식 오브젝트가 B 오브젝트인지 확인

                if (gunTagObject.gameObject.GetComponentInChildren<AmmoSystem>() != null)
                {
                    // B 오브젝트의 탄약 증가
                    AmmoSystem ammo = gunTagObject.gameObject.GetComponentInChildren<AmmoSystem>();
                    if (ammo != null)
                    {
                        ammo.AmmoLeft = 48;
                        // 소지가능 탄약 최대값 입력 요망(플레이어측)
                    }
                }
                Debug.Log("Gun Tag 오브젝트 이름: " + gunTagObject.name);
            }
           

            //A 오브젝트 소멸여부 선택
            Destroy(gameObject);
        }
    }
}