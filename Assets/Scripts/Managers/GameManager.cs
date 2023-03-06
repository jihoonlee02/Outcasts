using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    Debug.LogError("GameManager Prefab is required in level scene!");
                }
            }

            return instance;
        }
    }
    #endregion 

    [Header("Player Pawn Management")]
    [SerializeField] private PlayerInputManager m_pim;
    [SerializeField] private Pawn[] m_pawnsToControl;

    [Header("Level Management")]
    [SerializeField] private RoomManager m_roomManager;
    [SerializeField] private LevelManager m_levelManager;

    public RoomManager CurrRoomManager => m_roomManager;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        foreach (Pawn pawn in m_pawnsToControl)
            pawn.gameObject.SetActive(false);
    }

    private int count = 0;
    public void SetPlayerControllerToPawn(PlayerInput pi)
    {
        //Coupled
        if (count > 1) return;
        pi.GetComponent<PlayerController>().ControlPawn(m_pawnsToControl[count]);
        pi.gameObject.name = m_pawnsToControl[count].name + " Player";
        m_pawnsToControl[count].gameObject.SetActive(true);
        count++;
    }
}
