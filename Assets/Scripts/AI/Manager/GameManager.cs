using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Respawn")]
    public Vector3 respawnPoint; // ����������
    public string respwanScene;

    public void Respawn()
    {
        //�÷��̾� ü��ȸ��, ��ġ���� ��ũ��Ʈ�� ������ �ɵ�.
        // SceneManager.LoadScene(respwanScene);
    }
}
