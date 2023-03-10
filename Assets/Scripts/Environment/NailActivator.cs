using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class NailActivator : Invoker
{
    private SpriteRenderer m_spriteRenderer;
    private bool isActive = false;

    private void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.color = Color.gray;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive)
        {
            Activate();
            m_spriteRenderer.color = Color.green;
        }  
        else
        {
            Deactivate();
            m_spriteRenderer.color = Color.gray;
        }
        isActive = !isActive;
    }
}
