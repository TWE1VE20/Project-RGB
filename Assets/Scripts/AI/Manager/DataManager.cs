using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;

public class DataManager : MonoBehaviour
{
    [SerializeField] string fileName;

    private GameData gameData;
    public GameData GameData { get { return gameData; } }

    private void Start()
    {
        LoadData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

#if UNITY_EDITOR
    private string path => Path.Combine(Application.dataPath, $"Data/{fileName}");
#else
    private string path => Path.Combine(Application.persistentDataPath, $"Data/{fileName}");
#endif

    public void NewData()
    {
        gameData = new GameData();
    }

    public void SaveData()
    {
        if (Directory.Exists(Path.GetDirectoryName(path)) == false)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(path, json);
    }

    public void LoadData()
    {
        if (File.Exists(path) == false)
        {
            NewData();
            return;
        }

        string json = File.ReadAllText(path);
        try
        {
            gameData = JsonUtility.FromJson<GameData>(json);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Load data fail : {ex.Message}");
            NewData();
        }
    }
}
