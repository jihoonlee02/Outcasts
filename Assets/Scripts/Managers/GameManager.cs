using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager m_pim;
    [SerializeField] private Pawn[] m_pawnsToControl;

    private int count = 0;
    public void SetPlayerControllerToPawn(PlayerInput pi)
    {
        pi.GetComponent<PlayerController>().ControlPawn(m_pawnsToControl[count]);
        pi.gameObject.name = m_pawnsToControl[count].name + " Player";
        count++;
    }
}
