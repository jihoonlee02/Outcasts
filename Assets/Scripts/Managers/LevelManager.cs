using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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
    


    [Header("Dev Details")]
    [SerializeField] private bool isSetupScene = false;
    [SerializeField] private UnityEvent invokeAtStart;
    [SerializeField] private bool isTesting = false;
    [SerializeField] private GameObject levelThings;
    private void Awake()
    {   
        if (isTesting & !isSetupScene)
        {
            Instantiate(levelThings);
        }
    }
    private void Start()
    {
        GameManager.Instance.LevelManager = this;
        GameManager.Instance.Tinker.transform.position = m_tinkerSpawn.position;
        GameManager.Instance.Ashe.transform.position = m_asheSpawn.position;
        GameManager.Instance.Tinker.gameObject.SetActive(true);
        GameManager.Instance.Ashe.gameObject.SetActive(true);

        if (AudioManager.Instance.CurrentAudio != m_music)
            AudioManager.Instance.SetAudioClip(m_music);

        m_tinkerSpawn.gameObject.SetActive(false);
        m_asheSpawn.gameObject.SetActive(false);

        if (isSetupScene)
        {
            GameManager.Instance.TransitionToNextScene();
            return;
        }

        GameManager.Instance.DoorTransition.OpenDoors();
        AudioManager.Instance.PlayAudio();

        invokeAtStart.Invoke();

        GameManager.Instance.IsTesting = isTesting;
    }

    private void Update()
    {
        // Though this is in the update method, it should only get invoked once...
        // ...hopefully
        if (m_asheExitDoor != null && m_tinkerExitDoor != null 
            && m_asheExitDoor.OnDoor && m_tinkerExitDoor.OnDoor) OnLevelExit();
    }

    public void OnLevelExit()
    {
        // So that Tinker n' Ashe don't start invoking this a billion times
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

