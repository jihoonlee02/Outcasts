using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Can be destroyed by GameObjects that invoke break on collision
[RequireComponent(typeof(Collider2D), typeof(MeshRenderer), typeof(AudioSource))]
public class Breakable : MonoBehaviour
{
    private ParticleSystem particle;
    private BoxCollider2D collider;
    private TilemapRenderer renderer;
    private AudioSource m_AudioSource;
    [SerializeField] private bool m_requiresAshe = true;
    [SerializeField, Tooltip("No Implementation with this one yet")] 
    private bool m_requiresTinker = false; 

    private void Start() 
    {
        particle = gameObject.GetComponent<ParticleSystem>();
        collider = gameObject.GetComponent<BoxCollider2D>();
        renderer = gameObject.GetComponentInChildren<TilemapRenderer>();
        m_AudioSource = gameObject.GetComponent<AudioSource>();
    }

    public IEnumerator Break()
    {
        particle.Play();
        collider.enabled = false;
        renderer.enabled = false;

        yield return new WaitForSeconds(particle.main.startLifetime.constantMax);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_requiresAshe && m_requiresTinker) { Debug.Log("If and only if tinekr n ash lol"); }
        else if (m_requiresAshe && collision.gameObject.tag == "Gauntlet")
        {
            m_AudioSource.Play();
            StartCoroutine(Break());
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_requiresAshe && m_requiresTinker) { Debug.Log("If and only if tinekr n ash lol"); }
        else if (m_requiresAshe && collider.gameObject.tag == "Gauntlet")
        {
            m_AudioSource.Play();
            StartCoroutine(Break());
        }
    }
}
