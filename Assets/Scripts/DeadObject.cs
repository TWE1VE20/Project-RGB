using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class DeadObject : MonoBehaviour
{
    
    [SerializeField] LayerMask playerLayer;
    public GameObject[] playerObjects; // Player ���̾� ������Ʈ �迭

    private void Start()
    {
        playerObjects = GameObject.FindObjectsOfType<GameObject>();
        playerObjects = System.Array.FindAll(playerObjects, obj => obj.layer == LayerMask.NameToLayer("Player"));
        Manager.game.Player.Clear();
        foreach (GameObject obj in playerObjects)
        {
            Manager.game.Player.Add(obj);
        }
        respawn();
    }
    public void respawn()
    {
        // �÷��̾� ������Ʈ ������ ����Ʈ�� �̵�
        Manager.game.Player[0].transform.position = Manager.game.respawnPoint;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

            playerController.IsAlive = false;
        }
    }
}
