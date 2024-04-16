using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class DeadObject : MonoBehaviour
{
    
    [SerializeField] LayerMask playerLayer;
    public GameObject[] playerObjects; // Player 레이어 오브젝트 배열

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
        // 플레이어 오브젝트 리스폰 포인트로 이동
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
