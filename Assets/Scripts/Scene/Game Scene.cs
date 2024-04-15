using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField] CharacterController playerController;

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
}