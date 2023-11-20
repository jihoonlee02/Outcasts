using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class NailActivator : Invoker
{
    [Header("NailActivator Specific")]
    [SerializeField] private Sprite inactive_sprite;
    [SerializeField] private Sprite active_sprite;


    private SpriteRenderer m_spriteRenderer;
    private AudioSource m_audioSource;

    #region Technical
    private bool isActive = false;
    //private int count = 0;
    #endregion

    private void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.sprite = inactive_sprite;
        m_audioSource = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //count++;
        if (collision.GetComponent<Projectile>() != null)
        {
            m_audioSource.Play();
            if (!isActive)
            {
                isActive = true;
                Activate();
                m_spriteRenderer.sprite = active_sprite;
                // Play Aniation of Squish State
            }     
        }  
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Will Not Deactivate for the time being
        //count--;
        //if (count == 0 && collision.GetComponent<Projectile>() != null) 
        //{
        //    isActive = false;
        //    Deactivate();
        //    m_spriteRenderer.sprite = inactive_sprite;
        //}
    }
}
