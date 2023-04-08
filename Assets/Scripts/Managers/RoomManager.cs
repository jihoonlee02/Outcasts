using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Locations")]
    [SerializeField] private Vector2 m_tinkerSpawn;
    [SerializeField] private Vector2 m_asheSpawn;
    [SerializeField] private ExitDoor m_tinkerPrevDoor;
    [SerializeField] private ExitDoor m_ashePrevDoor;
    [SerializeField] private ExitDoor m_tinkerNextDoor;
    [SerializeField] private ExitDoor m_asheNextDoor;

    //Fully Version will be scene transition rather than position transition
    [Header("Next RoomEvent")]
    [SerializeField] private Vector2 m_currroomPosition;
    [SerializeField] private Room[] m_rooms;

    [Header("Pawns")]
    [SerializeField] private Pawn m_tinker;
    [SerializeField] private Pawn m_ashe;
    public Pawn Tinker => m_tinker;
    public Pawn Ashe => m_ashe;

    #region Technical
    private int roomIdx = 0;
    #endregion

    public bool ViewingRoom => (Vector2)Camera.Instance.transform.position == m_currroomPosition;

    private void Start()
    {
        m_tinker.transform.position = m_rooms[roomIdx].tinkerSpawn;
        m_ashe.transform.position = m_rooms[roomIdx].asheSpawn;
        m_ashePrevDoor = m_rooms[m_rooms.Length].asheDoor;
        m_tinkerPrevDoor = m_rooms[m_rooms.Length].tinkerDoor;
        m_tinkerNextDoor = m_rooms[roomIdx].tinkerDoor;
        m_asheNextDoor = m_rooms[roomIdx].asheDoor;
        m_currroomPosition = m_rooms[roomIdx].roomPosition;
        Camera.Instance.ShiftTo(m_rooms[roomIdx].roomPosition);
    }

    private void Update()
    {
        if (m_tinkerNextDoor.OnDoor && m_asheNextDoor.OnDoor) NextRoom();
        if (m_tinkerPrevDoor.OnDoor && m_ashePrevDoor.OnDoor) NextRoom();
    }

    public void OnLevelExit()
    {
        //SlideManager.Instance.NextSlide();
    }

    //For Developer Purpose!!!
    public void NextRoom()
    {
        TransitionToRoom((roomIdx + 1) % m_rooms.Length);
    }

    public void PrevRoom()
    {
        TransitionToRoom((roomIdx - 1 < 0 ? (m_rooms.Length - 1) : roomIdx - 1) % m_rooms.Length);   
    }

    public void TransitionToRoom(int roomIdx)
    {
        this.roomIdx = roomIdx;
        m_tinker.transform.position = m_rooms[roomIdx].tinkerSpawn;
        m_ashe.transform.position = m_rooms[roomIdx].asheSpawn;
        m_tinkerPrevDoor = m_tinkerNextDoor;
        m_ashePrevDoor = m_asheNextDoor;

        m_tinkerNextDoor = m_rooms[roomIdx].tinkerDoor;
        m_asheNextDoor = m_rooms[roomIdx].asheDoor;
        m_currroomPosition = m_rooms[roomIdx].roomPosition;
        Camera.Instance.ShiftTo(m_rooms[roomIdx].roomPosition);
    }
}

// Temp Level Struct
// If one will be made it will have its own attachable script to
// a prefab
[Serializable]
public struct Room
{
    public Vector2 tinkerSpawn;
    public Vector2 asheSpawn;
    public Vector2 roomPosition;
    public ExitDoor tinkerDoor;
    public ExitDoor asheDoor;
}


