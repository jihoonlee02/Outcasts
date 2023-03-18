using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Can be destroyed by GameObjects that invoke break on collision
[RequireComponent(typeof(Collider2D), typeof(MeshRenderer), typeof(AudioSource))]
public class Breakable : MonoBehaviour
{
    [SerializeField] private bool m_requiresAshe = true;
    [SerializeField, Tooltip("No Implementation with this one yet")] 
    private bool m_requiresTinker = false;
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private Collider2D m_collider2D;
    [SerializeField] private Renderer m_renderer;

    public void Break()
    {
        m_collider2D.enabled = false;
        m_renderer.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_requiresAshe && m_requiresTinker) { Debug.Log("If and only if tinekr n ash lol"); }
        else if (m_requiresAshe && collision.gameObject.tag == "Gauntlet")
        {
            m_AudioSource.Play();
            Break();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_requiresAshe && m_requiresTinker) { Debug.Log("If and only if tinekr n ash lol"); }
        else if (m_requiresAshe && collider.gameObject.tag == "Gauntlet")
        {
            m_AudioSource.Play();
            Break();
        }
    }
}
