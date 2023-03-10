using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Pawns")]
    //[SerializeField] private Pawn m_tinker;
    //[SerializeField] private Pawn m_ashe;
    [SerializeField] private ExitDoor m_tinkerExitDoor;
    [SerializeField] private ExitDoor m_asheExitDoor;
    [SerializeField] private Transform m_tinkerSpawn;
    [SerializeField] private Transform m_asheSpawn;
    [SerializeField] private AudioClip m_music;
    //public Pawn Tinker => m_tinker;
    //public Pawn Ashe => m_ashe;

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
        GameManager.Instance.LevelManager = this;
        GameManager.Instance.Tinker.transform.localPosition = m_tinkerSpawn.position;
        GameManager.Instance.Ashe.transform.localPosition = m_asheSpawn.position;
        GameManager.Instance.Tinker.gameObject.SetActive(true);
        GameManager.Instance.Ashe.gameObject.SetActive(true);

        if (AudioManager.Instance.CurrentAudio != m_music)
            AudioManager.Instance.SetAudioClip(m_music);

        m_tinkerSpawn.gameObject.SetActive(false);
        m_asheSpawn.gameObject.SetActive(false);
        GameManager.Instance.DoorTransition.OpenDoors();
        AudioManager.Instance.PlayAudio();
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
        GameManager.Instance.Tinker.gameObject.SetActive(false);
        GameManager.Instance.Ashe.gameObject.SetActive(false);

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

