using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 m_movingDirection;

    [SerializeField] private float m_speed = 1.5f;
    private SpriteRenderer m_sr;
    private Rigidbody2D m_rb;
    private Collider2D m_cldr;
    private Transform poolerParent;
    public void Awake()
    {
        m_movingDirection = Vector2.zero;
        m_sr = GetComponent<SpriteRenderer>();
        m_rb = GetComponent<Rigidbody2D>();
        m_cldr = GetComponent<Collider2D>();
        poolerParent = transform.parent;
    }

    public void Fire(Vector2 spawnPos, Vector2 direction)
    {
        transform.SetParent(poolerParent, true);
        transform.position = spawnPos;
        m_movingDirection = direction;
        m_rb.velocity = direction * 10f;
        //enabled = true;

        m_sr.flipX = direction == Vector2.left;
        m_cldr.offset = new Vector2(Mathf.Sign(direction.x) * Mathf.Abs(m_cldr.offset.x), m_cldr.offset.y);
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

    public void Update()
    {
        //transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + m_movingDirection, Time.deltaTime * m_speed);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("I was entered trigger!");
        if (collider.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            m_rb.velocity = Vector3.zero;
            //m_movingDirection = Vector3.zero;
            //enabled = false;
            transform.SetParent(collider.transform, true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("I was entered Collision!");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            m_rb.velocity = Vector3.zero;
            //m_movingDirection = Vector3.zero;
            //enabled = false;
            transform.SetParent(collision.transform, true);
        }
    }
}
