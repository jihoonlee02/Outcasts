using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Pawns")]
    [SerializeField] private Pawn m_tinker;
    [SerializeField] private Pawn m_ashe;
    [SerializeField] private ExitDoor m_tinkerExitDoor;
    [SerializeField] private ExitDoor m_asheExitDoor;
    [SerializeField] private Vector2 m_tinkerSpawn;
    [SerializeField] private Vector2 m_asheSpawn;
    public Pawn Tinker => m_tinker;
    public Pawn Ashe => m_ashe;

    //Level Manager will not handle next levels, that is gamemanagers job
    //[SerializeField] private Level currLevel;
    //[SerializeField] private Level[] levels;
    private void Awake()
    {   
        // Old Function of Level Manager
        //for (int i = 0; i < levels.Length - 1; i++)
        //{
        //    levels[i].nextLevel = levels[i + 1];
        //}
    }
    private void Start()
    {
        foreach (Pawn pawn in GameManager.Instance.PlayerPawns)
        {
            if (pawn.Data.name == "Tinker") m_tinker = pawn;
            else if (pawn.Data.name == "Ashe") m_ashe = pawn;
        }

        m_tinker.transform.position = m_tinkerSpawn;
        m_ashe.transform.position = m_asheSpawn;
    }

    private void Update()
    {
        // Though this is in the update method, it should only get invoked once...
        // ...hopefully
        if (m_asheExitDoor.OnDoor && m_tinkerExitDoor.OnDoor) OnLevelExit();
    }

    public void OnLevelExit()
    {
        // If Tinker and Ashe had a an animation entering the doors, this would be invoked here!
        // For now!
        m_tinker.gameObject.SetActive(false);
        m_ashe.gameObject.SetActive(false);

        //DialogueManager.Instance.DisplayDialogue();

        //This shouldn't be done immeditly
        GameManager.Instance.TransitionToNextScene();
    }
}

// Temp Level Struct
// If one will be made it will have its own attachable script to
// a prefab
//[Serializable, CreateAssetMenu(menuName = "Data/LevelData")]
//public class Level : ScriptableObject
//{
//    public Vector2 tinkerSpawn;
//    public Vector2 asheSpawn;
//    public string sceneName;
//    public ExitDoor tinkerExitDoor;
//    public ExitDoor asheExitDoor;
//    public Level nextLevel;
//}

