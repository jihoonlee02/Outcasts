using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Locations")]
    [SerializeField] private Vector2 m_tinkerSpawn;
    [SerializeField] private Vector2 m_asheSpawn;
    [SerializeField] private ExitDoor m_tinkerExitDoor;
    [SerializeField] private ExitDoor m_asheExitDoor;

    //Fully Version will be scene transition rather than position transition
    [Header("Next Level Event")]
    [SerializeField] private Vector2 m_currLevelPosition;
    [SerializeField] private Level[] m_levels;

    [Header("Pawns")]
    [SerializeField] private Pawn m_tinker;
    [SerializeField] private Pawn m_ashe;
    public Pawn Tinker => m_tinker;
    public Pawn Ashe => m_ashe;

    #region Technical
    private int levelNum = 0;
    #endregion

    public bool ViewingLevel => (Vector2)Camera.Instance.transform.position == m_currLevelPosition;

    private void Start()
    {
        m_tinker.transform.position = m_levels[levelNum].tinkerSpawn;
        m_ashe.transform.position = m_levels[levelNum].asheSpawn;
        m_tinkerExitDoor = m_levels[levelNum].tinkerDoor;
        m_asheExitDoor = m_levels[levelNum].asheDoor;
        m_currLevelPosition = m_levels[levelNum].levelPosition;
        Camera.Instance.ShiftTo(m_levels[levelNum].levelPosition);
    }

    private void Update()
    {
        if (m_tinkerExitDoor.OnDoor && m_asheExitDoor.OnDoor) OnLevelExit();
    }

    public void OnLevelExit()
    {
        SlideManager.Instance.NextSlide();
    }

    //For Developer Purpose!!!
    public void NextLevel()
    {
        levelNum = (levelNum + 1) % m_levels.Length;
        m_tinker.transform.position = m_levels[levelNum].tinkerSpawn;
        m_ashe.transform.position = m_levels[levelNum].asheSpawn;
        m_tinkerExitDoor = m_levels[levelNum].tinkerDoor;
        m_asheExitDoor = m_levels[levelNum].asheDoor;
        m_currLevelPosition = m_levels[levelNum].levelPosition;
        Camera.Instance.ShiftTo(m_levels[levelNum].levelPosition);
    }

    public void PrevLevel()
    {
        levelNum = (levelNum - 1 < 0 ? (m_levels.Length - 1) : levelNum - 1) % m_levels.Length;
        m_tinker.transform.position = m_levels[levelNum].tinkerSpawn;
        m_ashe.transform.position = m_levels[levelNum].asheSpawn;
        m_tinkerExitDoor = m_levels[levelNum].tinkerDoor;
        m_asheExitDoor = m_levels[levelNum].asheDoor;
        m_currLevelPosition = m_levels[levelNum].levelPosition;
        Camera.Instance.ShiftTo(m_levels[levelNum].levelPosition);
    }
}

// Temp Level Struct
// If one will be made it will have its own attachable script to
// a prefab
[Serializable]
public struct Level
{
    public Vector2 tinkerSpawn;
    public Vector2 asheSpawn;
    public Vector2 levelPosition;
    public ExitDoor tinkerDoor;
    public ExitDoor asheDoor;
}


