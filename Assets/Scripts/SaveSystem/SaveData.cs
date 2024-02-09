using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // General Stats
    public string ProfileName;
    public string CurrScene;
    public bool[] ChestCollected;
    public int ChestsCount;
    public int TinkerGemCount;
    public int AsheGemCount;
    public int Points;
    public float PlayerTimeInSeconds;
    
    // Challenge Achievements
    // Run Past Torches 50 times
    // Do Slime thing
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string a_Json)
    {
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }
}

public interface ISaveable
{
    void PopulateSaveData(SaveData a_SaveData);
    void LoadFromSaveData(SaveData a_SaveData);
}
