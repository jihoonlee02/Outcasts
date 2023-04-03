using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class NailActivator : Invoker
{
    private SpriteRenderer m_spriteRenderer;

    #region Technical
    private bool isActive = false;
    private int count = 0;
    #endregion

    private void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.color = Color.gray;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        count++;
        if (!isActive)
        {
            isActive = true;
            Activate();
            m_spriteRenderer.color = Color.green;
        }  
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        count--;
        if (count == 0) 
        {
            isActive = false;
            Deactivate();
            m_spriteRenderer.color = Color.gray;
        }
    }
}
