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
    }

    public void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + m_movingDirection, Time.deltaTime * m_speed);
    }
}
