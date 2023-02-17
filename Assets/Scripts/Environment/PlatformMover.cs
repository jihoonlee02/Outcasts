using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [Header("Platform Points")]
    [SerializeField] private Vector2 m_pointA;
    [SerializeField] private Vector2 m_pointB;
    [SerializeField] private Vector2 m_pointC;

    [Header("Modifications")]
    [SerializeField, Range(0f, 1f)] private float speed;

    private bool shouldMove = false;
    private bool secretMove = false;

    private void Start()
    {
        transform.position = m_pointA;
    }

    private void Update()
    {
        if (secretMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, m_pointC, Time.deltaTime * 3f * speed);
        }
        else if (shouldMove)
        {
            transform.position =  Vector2.MoveTowards(transform.position, m_pointB, Time.deltaTime * 3f * speed);
        }
        else
        {
            transform.position =  Vector2.MoveTowards(transform.position, m_pointA, Time.deltaTime * 3f * speed);
        }
    }

    public void MoveToPointB()
    {
        shouldMove = true;
        secretMove = false;
    }

    public void MoveToPointA()
    {
        shouldMove = false;
        secretMove = false;
    }

    public void MoveToPointC()
    {
        shouldMove = false;
        secretMove = true;
    }
}
