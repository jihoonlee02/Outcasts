using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Chase : MonoBehaviour
{
    [SerializeField] private string m_pawnName;
    [SerializeField] private float m_speed = 2f;
    [SerializeField] private float m_angleOffset = -90f;

    private Rigidbody2D m_rb;
    private Vector3 moveDirection;
    private Transform target;
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        target = m_pawnName == "Tinker" ? GameManager.Instance.Tinker.transform
            : m_pawnName == "Ashe" ? GameManager.Instance.Ashe.transform : null;
    }

    private void Update()
    {
        moveDirection = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        m_rb.rotation = angle + m_angleOffset;    
    }

    private void FixedUpdate()
    {
        m_rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * m_speed;
    }

    public void StartChase()
    {

    }
    public void StopChase()
    {

    }
}
