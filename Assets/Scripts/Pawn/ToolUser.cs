using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolUser : Pawn
{
    #region Selection Node Logic
    private class SelectionNode
    {
        public SelectionNode prev;
        public SelectionNode next;
        public Tool tool;

        public SelectionNode(Tool tool) { this.tool = tool; }

        public SelectionNode(Tool tool, SelectionNode next, SelectionNode prev)
        {
            this.tool = tool;
            this.next = next;
            this.prev = prev;
        }
    }
    //Node Logic
    private SelectionNode head;
    private SelectionNode tail;
    #endregion

    private SelectionNode currToolNode;
    private int size;

    //Used to initizlize tools in inspector
    [SerializeField] private Tool[] m_tools;

    protected new void Start()
    {
        base.Start();
        head = null;
        tail = null;
        size = 0;

        foreach (var tool in m_tools)
        {
            AddTool(tool);
        }
    }

    public void UseToolPrimaryAction() 
    {
        Animator.speed = 1;
        currToolNode.tool.UsePrimaryAction();
    }

    //Out of Scope
    public void UseToolSecondaryAction()
    {
        currToolNode.tool.UseSecondaryAction();
    }

    //Was important, not really necessary now
    public void AddTool(Tool tool)
    {
        tool.User = this;

        if (head == null)
        {
            currToolNode = new SelectionNode(tool);
            currToolNode.next = currToolNode;
            currToolNode.prev = currToolNode;
            head = currToolNode;
            tail = currToolNode;
        }
        else 
        {
            SelectionNode newNode = new SelectionNode(tool, head, tail);
            tail.next = newNode;
            head.prev = newNode;
        }

        size++;
    }

    //Sad but nothing
    public void RemoveTool(Tool tool)
    {
        //Remains nothing
    }

    //No point
    public void RemoveCurrTool()
    {
        if (currToolNode == null) 
        {
            return;
        }

        if (size == 1)
        {
            currToolNode = null;
            head = null;
            tail = null;
            size = 0;
            return;
        }

        currToolNode.next.prev = currToolNode.prev;
        currToolNode.prev.next = currToolNode.next;
        currToolNode = currToolNode.next;
        size--;
    }

    // Out of Scope -> Won't be in use, but will remain for future reason o.O
    public void NextTool()
    {
        currToolNode = currToolNode.next;
    }

    public void PrevTool()
    {
        currToolNode = currToolNode.prev;
    }
}


