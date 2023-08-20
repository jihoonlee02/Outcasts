using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class Chase : MonoBehaviour
{
    [SerializeField] private string m_pawnName;
    [SerializeField] private float m_speed = 2f;
    [SerializeField] private float m_angleOffset = -90f;
    [SerializeField] private Transform target;
    [SerializeField] private bool isChasing = false;
    private Rigidbody2D m_rb;
    private Vector3 moveDirection;
    
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        target = m_pawnName == "Tinker" ? GameManager.Instance.Tinker.transform
            : m_pawnName == "Ashe" ? GameManager.Instance.Ashe.transform : target;
    }

    private void Update()
    {
        moveDirection = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        m_rb.rotation = angle + m_angleOffset;
        if (isChasing) m_rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * m_speed;
    }

    public void StartChase()
    {
        isChasing = true;
    }
    public void StopChase()
    {
        isChasing = false;
    }
    public void ChangeTarget(string name)
    {
        target = name == "Tinker" ? GameManager.Instance.Tinker.transform
            : name == "Ashe" ? GameManager.Instance.Ashe.transform : target;
    }
    public void ChangeTarget(Transform transform)
    {
        target = transform;
    }
}
