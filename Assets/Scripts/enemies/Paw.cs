using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paw : MonoBehaviour
{
    private Rigidbody2D m_rb;
    private Chase m_chaser;
    private BoxCollider2D m_boxCollider;
    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_chaser = GetComponent<Chase>();
        m_boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Projectile>()) ReactOnNail();
        else if (collision.GetComponent<Gauntlet>()) ReactOnPunch();
    }
    private void ReactOnNail()
    {
        StartCoroutine(ChaseDisableFor(0.1f));
    }
    private void ReactOnPunch()
    {
        StartCoroutine(ChaseDisableFor(2f));
    }
    private IEnumerator ChaseDisableFor(float seconds)
    {
        m_chaser.StopChase();
        m_boxCollider.enabled = false;
        yield return new WaitForSeconds(seconds);
        m_chaser.StartChase();
        m_boxCollider.enabled = true;
    }
}
