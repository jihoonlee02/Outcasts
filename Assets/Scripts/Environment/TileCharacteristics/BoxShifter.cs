using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxShifter : MonoBehaviour, CollisionRespondent
{
    [SerializeField] private BoxCollider2D m_leftCollider;
    [SerializeField] private BoxCollider2D m_rightCollider;
    [SerializeField] private BoxCollider2D m_upCollider;
    [SerializeField] private BoxCollider2D m_downCollider;

    [SerializeField] private Rigidbody2D m_rb;

    public void InvokeTriggerResponse(Collider2D collider, string colliderImpacted)
    {
        var impacter = collider.gameObject.GetComponent<Projectile>();

        if (impacter == null || impacter.GetType() != typeof(Projectile)) return;

        switch (colliderImpacted) 
        {
            case "right":
                m_rb.velocity += Vector2.left;
                break;
            case "left":
                m_rb.velocity += Vector2.right;
                break;
            case "up":
                m_rb.velocity += Vector2.down;
                break;
            case "down":
                m_rb.velocity += Vector2.up;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.GetMask("Platforms")) 
        {
            m_rb.velocity = Vector2.zero;
        }
    }
}
