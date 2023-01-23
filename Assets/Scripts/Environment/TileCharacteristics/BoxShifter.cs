using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;

public class BoxShifter : CollisionRespondent
{
    [SerializeField] private BoxCollider2D m_leftCollider;
    [SerializeField] private BoxCollider2D m_rightCollider;
    [SerializeField] private BoxCollider2D m_upCollider;
    [SerializeField] private BoxCollider2D m_downCollider;

    [SerializeField] private Rigidbody2D m_rb;

    public void InvokeCollisionRespose(Collision2D collision, string colliderImpacted)
    {
        throw new System.NotImplementedException();
    }

    public void InvokeTriggerResponse(Collider2D collider, string colliderImpacted)
    {
        var projectile = collider.gameObject.GetComponent<Projectile>();
        projectile?.Impact();
        switch (colliderImpacted) 
        {
            case "right":
                m_rb.velocity = Vector2.right;
                break;
            case "left":
                m_rb.velocity = Vector2.left;
                break;
            case "up":
                m_rb.velocity = Vector2.up;
                break;
            case "down":
                m_rb.velocity = Vector2.down;
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
