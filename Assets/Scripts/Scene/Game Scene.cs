using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField] CharacterController playerController;

    public override IEnumerator LoadingRoutine()
    {
        Debug.Log("GameScene Load");
        yield return new WaitForSeconds(0.5f);

    }

    public override void SceneLoad()
    {
        playerController.enabled = false;
        playerController.enabled = true;
    }
}