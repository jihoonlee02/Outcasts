using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawn : ToolUser
{
    [Header("Player Component References")]
    [SerializeField] private PlayerController m_pc;

    public PlayerController PC
    {
        get { return m_pc; }
        set { m_pc = value; }
    }
}
