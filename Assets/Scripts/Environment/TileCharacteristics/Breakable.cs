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
    private bool broken = false;

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
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Projectile>())
            {
                child.SetParent(Pooler.Instance.transform, false);
                child.gameObject.SetActive(false);
            }
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_requiresAshe && m_requiresTinker) { Debug.Log("If and only if tinekr n ash lol"); }
        else if (!broken && (m_requiresAshe && collision.gameObject.tag == "Gauntlet"))
        {
            m_AudioSource.Play();
            StartCoroutine(Break());
            broken = true;
        }
    }
    // && (collision.gameObject.GetComponent<Rigidbody2D>().velocity.x >= 2f 
    //        || collision.gameObject.GetComponent<Rigidbody2D>().velocity.x >= 2f))
    private void OnTriggerEnter2D(Collider2D collider)
    {
        var rb = collider.gameObject.GetComponent<Rigidbody2D>();
        var vel = Vector2.zero;
        if (rb != null) vel = rb.velocity;

        if (!broken && ((m_requiresAshe && collider.gameObject.tag == "Gauntlet") 
            || ((collider.gameObject.tag == "physical") && (Mathf.Abs(vel.x) >= 4f || Mathf.Abs(vel.y) >= 4f))))
        {
            m_AudioSource.Play();
            StartCoroutine(Break());
            broken = true;
        }
    }
    // && (collider.gameObject.GetComponent<Rigidbody2D>().velocity.x >= 2f
    //        || collider.gameObject.GetComponent<Rigidbody2D>().velocity.x >= 2f))
}
