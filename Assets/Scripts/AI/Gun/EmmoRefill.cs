using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmmoRefill : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� Player���� Ȯ��
        if (other.gameObject.layer == 3)
        {
            // C ������Ʈ�� ��� �ڽ� ������Ʈ �ݺ�
            foreach (Transform child in other.gameObject.transform)
            {
                // �ڽ� ������Ʈ�� B ������Ʈ���� Ȯ��
                if (child.gameObject.GetComponent<Gun>() != null)
                {
                    // B ������Ʈ�� ź�� 5 ����
                    Gun gun = child.gameObject.GetComponent<Gun>();
                    if (gun != null)
                    {
                        //gun.initAmmoAmount = gun.maxRounds;
                    }
                }
            }

            // A ������Ʈ �Ҹ�
            // Destroy(gameObject);
        }
    }
}