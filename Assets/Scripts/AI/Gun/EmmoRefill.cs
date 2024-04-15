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
                if (child.gameObject.GetComponent<AmmoSystem>() != null)
                {
                    // B ������Ʈ�� ź�� ����
                    AmmoSystem ammo = child.gameObject.GetComponent<AmmoSystem>();
                  if (ammo != null)
                  {
                      ammo.AmmoLeft = 48;
                      // �������� ź�� �ִ밪 �Է� ���(�÷��̾���)
                  }
                }
            }

            //A ������Ʈ �Ҹ꿩�� ����
            Destroy(gameObject);
        }
    }
}