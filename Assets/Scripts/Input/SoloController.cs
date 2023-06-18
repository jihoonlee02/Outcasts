using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoloController : MonoBehaviour
{
    [SerializeField] private PlayerInput m_playerInput;
    [SerializeField] private TinkerPawn m_tinkerPawn;
    [SerializeField] private AshePawn m_ashePawn;

    private void Awake()
    {
        
    }
}
