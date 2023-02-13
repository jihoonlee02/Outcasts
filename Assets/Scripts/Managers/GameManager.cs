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

    private int count = 0;
    public void SetPlayerControllerToPawn(PlayerInput pi)
    {
        pi.GetComponent<PlayerController>().ControlPawn(m_pawnsToControl[count]);
        pi.gameObject.name = m_pawnsToControl[count].name + " Player";
        count++;
    }
}
