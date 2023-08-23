using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paw : MonoBehaviour
{
    private Rigidbody2D m_rb;
    private Chase m_chaser;
    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_chaser = GetComponent<Chase>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Projectile>()) ReactOnNail();
        else if (collision.GetComponent<Gauntlet>()) ReactOnPunch();
    }
    private void ReactOnNail()
    {
        m_rb.velocity = Vector3.zero;
    }
    private void ReactOnPunch()
    {
        m_rb.velocity = Vector3.zero;
        StartCoroutine(ChaseDisableFor(3f));
    }
    private IEnumerator ChaseDisableFor(float seconds)
    {
        m_chaser.StopChase();
        yield return new WaitForSeconds(seconds);
        m_chaser.StartChase();
    }
}
