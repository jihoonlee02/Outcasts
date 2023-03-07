using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class NailActivator : MonoBehaviour
{
    [SerializeField] private UnityEvent m_hitActiveEvent;
    [SerializeField] private UnityEvent m_hitInactiveEvent;

    private SpriteRenderer m_spriteRenderer;
    private bool isActive = false;

    private void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.color = Color.gray;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive)
        {
            m_hitInactiveEvent.Invoke();
            m_spriteRenderer.color = Color.gray;
        }  
        else
        {
            m_hitActiveEvent.Invoke();
            m_spriteRenderer.color = Color.green;
        }
        isActive = !isActive;
    }

    public void Activate()
    {
        isActive = true;
    }

    public void InActivate()
    {
        isActive = false;
    }
}
