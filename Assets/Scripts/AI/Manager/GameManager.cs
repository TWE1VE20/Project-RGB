using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Respawn")]
    public Vector3 respawnPoint; // 리스폰지점
    public string respwanScene;

    public void Respawn()
    {
        //플레이어 체력회복, 위치변경 스크립트만 넣으면 될듯.
        // SceneManager.LoadScene(respwanScene);
    }
}
