using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public bool[] sceneSaved;

    public GameSceneData gameScene;

    public GameData()
    {
        // Set default value
        sceneSaved = new bool[32];
        gameScene = new GameSceneData();
    }
}