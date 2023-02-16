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
    [SerializeField] private PlayerPawn[] m_pawnsToControl;

    [Header("Level Management")]
    [SerializeField] private LevelManager m_currentLevelManager;

    public LevelManager CurrLevelManager => m_currentLevelManager;

    private void Start()
    {
        foreach (Pawn pawn in m_pawnsToControl)
            pawn.gameObject.SetActive(false);
    }

    private int count = 0;
    public void SetPlayerControllerToPawn(PlayerInput pi)
    {
        pi.GetComponent<PlayerController>().ControlPawn(m_pawnsToControl[count]);
        pi.gameObject.name = m_pawnsToControl[count].name + " Player";
        m_pawnsToControl[count].gameObject.SetActive(true);
        count++;
    }
}
