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
    [Header("Level Details")]
    [SerializeField] private bool airVentsInLevel = false;
    //[SerializeField] private int numberOfAirVentGroups = 1;
    [SerializeField] private AirVentGroupStruct[] totalAirVentPower;
    private AirVent[] airVents;
    private Hashtable airVentsHashtable;
    public bool AirVentsInLevel {
        get => airVentsInLevel;
    }


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
            GameManager.Instance.LoadToScene("Hub");
            return;
        }

        GameManager.Instance.DoorTransition.OpenDoors();
        AudioManager.Instance.PlayAudio();

        invokeAtStart.Invoke();

        GameManager.Instance.IsTesting = isTesting;
        if (airVentsInLevel) {
            InitializeAirVents();
        }
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

    private void InitializeAirVents() {
        //totalAirVentPower = new Tuple<int, float>[numberOfAirVentGroups];
        airVents = FindObjectsOfType<AirVent>();
        airVentsHashtable = new Hashtable();
        foreach (AirVent airVent in airVents) {
            GameObject airVentGO = airVent.gameObject;
            if (!airVentsHashtable.ContainsKey(airVent.AirVentGroup)) {
                ArrayList[] tempAL = new ArrayList[2];
                tempAL[0] = new ArrayList();
                tempAL[1] = new ArrayList();
                airVentsHashtable.Add(airVent.AirVentGroup, tempAL);
            }
            ArrayList[] airVentAL = (ArrayList[])(airVentsHashtable[airVent.AirVentGroup]);
            if (airVent.Activated) {
                airVentAL[0].Add(airVent);
            } else {
                airVentAL[1].Add(airVent);
            }
        }
        foreach (AirVentGroupStruct airVentGroup in totalAirVentPower) {
            int airVentGroupNum = airVentGroup.airVentGroupNum;
            ArrayList[] tempAL = (ArrayList[])(airVentsHashtable[airVentGroupNum]);
            float partialPower = airVentGroup.airVentPower / tempAL[0].Count;
            foreach (AirVent airVent in tempAL[0]) {
                Transform airPivot = airVent.gameObject.transform.parent;
                airPivot.localScale = new Vector3(airPivot.localScale.x, partialPower, airPivot.localScale.z);
                //airVent.gameObject.transform.parent.localScale.y = partialPower;
            }
        }
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

