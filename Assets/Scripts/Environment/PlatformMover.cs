using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class PlatformMover : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private int id;
    [SerializeField] private Vector2[] m_waypoints;
    [SerializeField, Range(0f, 1f)] private float speed = 1f;
    private bool isSelected => Selection.transforms.Contains(transform);
    private int idx = 0;

    private void Start()
    {
        transform.localPosition = m_waypoints[idx];

        EventManager.GetEventManager.Activated += MoveToA;
        EventManager.GetEventManager.Deactivated += MoveToB;
    }

    private void Update()
    {
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, m_waypoints[idx], Time.deltaTime * 3f * speed);
    }

    public void MoveToWaypoint(int idx)
    {
        this.idx = idx;
    }

    private void MoveToA(int id)
    {
        if (this.id == id)
            MoveToWaypoint(1);
    }

    private void MoveToB(int id)
    {
        if (this.id == id)
            MoveToWaypoint(0);
    }

    private void OnDrawGizmos()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        if (m_waypoints == null || !isSelected) return;
        for (int i = 0; i < m_waypoints.Length; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(m_waypoints[i], 0.2f);

            for (int j = i + 1; j < m_waypoints.Length; j++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(m_waypoints[i], m_waypoints[j]);
            }
        }
    }
}
