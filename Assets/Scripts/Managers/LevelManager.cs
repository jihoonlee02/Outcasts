using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //Fully Version will be scene transition rather than position transition
    [Header("Next RoomEvent")]
    [SerializeField] private string currentScene;

    [Header("Pawns")]
    [SerializeField] private Pawn m_tinker;
    [SerializeField] private Pawn m_ashe;
    public Pawn Tinker => m_tinker;
    public Pawn Ashe => m_ashe;

    [SerializeField] private Level currLevel;
    [SerializeField] private Level[] levels;
    private void Awake()
    { 
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < levels.Length - 1; i++)
        {
            levels[i].nextLevel = levels[i + 1];
        }
    }
    private void Start()
    {
        m_tinker.transform.position = currLevel.tinkerSpawn;
        m_ashe.transform.position = currLevel.asheSpawn;
        SceneManager.LoadSceneAsync(currLevel.sceneName);
    }

    private void Update()
    {
        if (currLevel.asheExitDoor.OnDoor && currLevel.tinkerExitDoor.OnDoor) OnLevelExit();
    }

    public void OnLevelExit()
    {
        currLevel = currLevel.nextLevel;
        SceneManager.LoadSceneAsync(currLevel.sceneName);
    }
}

// Temp Level Struct
// If one will be made it will have its own attachable script to
// a prefab
[Serializable, CreateAssetMenu(menuName = "Data/LevelData")]
public class Level : ScriptableObject
{
    public Vector2 tinkerSpawn;
    public Vector2 asheSpawn;
    public string sceneName;
    public ExitDoor tinkerExitDoor;
    public ExitDoor asheExitDoor;
    public Level nextLevel;
}

