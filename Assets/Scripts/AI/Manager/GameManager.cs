using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Respawn")]
    public Vector3 respawnPoint; // 리스폰지점
    public string currentSceneName;
    public List<GameObject> Player = new List<GameObject>();
    
    [Header("save")]
    public int savePos = 0;
    [SerializeField] GameObject[] Enemy;
    
    public void Dead()
    {
        
        //Instantiate(gameObject, Manager.game.grave
        // 현재 씬 이름 가져오기
        currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // 씬 이름 출력
        Debug.Log("현재 씬 이름: " + currentSceneName);

        // 씬 재로드
        //Manager.Scene.LoadScene(currentSceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
        
        
        // 씬 전체에서 Player 레이어 오브젝트를 찾아 목록에 추가
        // 씬 전체에서 Player 레이어 오브젝트를 찾아 배열에 저장
        
        
    }
    
}
