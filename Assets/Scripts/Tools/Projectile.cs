using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 m_movingDirection;

    [SerializeField] private float m_speed = 1.5f;
    [SerializeField] private SpriteRenderer m_sr;

    public void Awake()
    {
        m_movingDirection = Vector2.zero;
        m_sr = GetComponent<SpriteRenderer>();
    }

    public void Fire(Vector2 spawnPos, Vector2 direction)
    {
        transform.position = spawnPos;
        m_movingDirection = direction;

        m_sr.flipX = direction == Vector2.left;
        if (direction == new Vector2(0.5f, 0.5f))
        {
            transform.eulerAngles = new Vector3(0f, 0f, 45);
            return;
        }
        if (direction == new Vector2(0.5f, -0.5f))
        {
            transform.eulerAngles = new Vector3(0f, 0f, -45);
            return;
        }
        if (direction == new Vector2(-0.5f, 0.5f))
        {
            transform.eulerAngles = new Vector3(0f, 0f, 135);
            return;
        }
        if (direction == new Vector2(-0.5f, -0.5f))
        {
            transform.eulerAngles = new Vector3(0f, 0f, -135);
            return;
        }
        transform.eulerAngles = new Vector3(0, 0, direction == Vector2.up ? 90 : direction == Vector2.down ? -90 : 0);
    }

    public void OnImpact()
    {
        gameObject.SetActive(false);
    }

    public void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + m_movingDirection, Time.deltaTime * m_speed);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<PlayerPawn>()) return;
        OnImpact();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<PlayerPawn>()) return;
        OnImpact();
    }


}
