using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField] CharacterController playerController;

    [Header("Respawn")]
    public Vector3 respawnPoint; // 리스폰지점
    public string currentSceneName;
    public List<GameObject> Player = new List<GameObject>();
    public override IEnumerator LoadingRoutine()
    {
        Debug.Log("GameScene Load");
        yield return new WaitForSeconds(2f);
    }

    public override void SceneLoad()
    {
        playerController.enabled = false;
        playerController.enabled = true;
    }

    public void BackToTitle()
    {
        Debug.Log("Back To Title");
        Manager.Scene.LoadScene("Title");
    }
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