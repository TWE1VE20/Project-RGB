using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Respawn")]
    public Vector3 respawnPoint; // ����������
    public string currentSceneName;
    public List<GameObject> Player = new List<GameObject>();
    
    [Header("save")]
    public int savePos = 0;
    [SerializeField] GameObject[] Enemy;
    
    public void Dead()
    {
        
        //Instantiate(gameObject, Manager.game.grave
        // ���� �� �̸� ��������
        currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // �� �̸� ���
        Debug.Log("���� �� �̸�: " + currentSceneName);

        // �� ��ε�
        //Manager.Scene.LoadScene(currentSceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
        
        
        // �� ��ü���� Player ���̾� ������Ʈ�� ã�� ��Ͽ� �߰�
        // �� ��ü���� Player ���̾� ������Ʈ�� ã�� �迭�� ����
        
        
    }
    
}
