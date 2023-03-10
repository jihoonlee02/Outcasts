using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    [SerializeField] protected ToolData m_data;
    [SerializeField] protected Pawn m_user;

    public ToolUser User { set { m_user = value; } }

    public virtual void UsePrimaryAction()
    {
        Debug.Log(m_user + " used Tool:" + m_data.toolName + " (Primary Action)");
    }

    public virtual void UseSecondaryAction()
    {
        Debug.Log(m_user + " used Tool:" + m_data.toolName + " (Secondary Action)");
    }
}
