using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField] CharacterController playerController;

    [Header("Respawn")]
    public Vector3 respawnPoint; // ����������
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