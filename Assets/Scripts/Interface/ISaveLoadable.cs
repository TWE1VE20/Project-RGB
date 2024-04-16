using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveLoadable
{
    public void SaveData(GameData gameData);
    public void LoadData(GameData gameData);
}