using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class PlatformMover : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private Vector2[] m_waypoints;
    [SerializeField, Range(0f, 1f)] private float speed;

    private Vector2 currWaypoint;
    private bool isSelected => Selection.transforms.Contains(transform);

    private void Start()
    {
        currWaypoint = m_waypoints[0];
        transform.position = currWaypoint;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, currWaypoint, Time.deltaTime * 3f * speed);
    }

    public void MoveToWaypoint(int idx)
    {
        currWaypoint = m_waypoints[idx];
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
