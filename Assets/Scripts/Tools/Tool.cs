using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    [SerializeField] protected ToolData m_data;
    [SerializeField] protected ToolUser m_user;

    public ToolUser User { set { m_user = value; } }

    public virtual void Use()
    {
        Debug.Log(m_user + " used Tool:" + m_data.toolName);
    }
}
