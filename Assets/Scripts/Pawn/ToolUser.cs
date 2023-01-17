using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolUser : Pawn
{
    [SerializeField] private Tool[] m_tools;

    protected void Start()
    {
        base.Start();
        foreach (var tool in m_tools)
        {
            tool.User = this;
        }
    }

    public void UseTool(int index) 
    {
        m_tools[index].Use();
    }
}
