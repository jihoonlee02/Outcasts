using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class PlatformMover : Invokee
{
    [Header("Options")]
    [SerializeField] private Vector2[] m_waypoints;
    [SerializeField, Range(0f, 1f)] private float speed = 1f;

    # if UNITY_EDITOR
    private bool isSelected => Selection.transforms.Contains(transform);
    #endif
    private int idx = 0;
    private Vector2 moveDifference;
    private List<Transform> objectsOnPlatform;
    public int moveDifferenceMultiplier;

    private void Start()
    {
        transform.localPosition = m_waypoints[idx];
        objectsOnPlatform = new List<Transform>();
        moveDifference = transform.localPosition;
    }

    
    private void Update()
    {
        moveDifference = transform.localPosition;
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, m_waypoints[idx], Time.deltaTime * 3f * speed);
        moveDifference -= (Vector2) transform.localPosition;

        foreach(var objectOnPlatform in objectsOnPlatform)
        {
            moveDifference *= -1;
            objectOnPlatform.Translate(moveDifference);
        }
    }

    public void MoveToWaypoint(int idx)
    {
        this.idx = idx;
    }
    protected override void OnActivate()
    {
        MoveToWaypoint(1);
    }
    protected override void OnDeactivate()
    {
        MoveToWaypoint(0);
    }

#if UNITY_EDITOR
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
#endif

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var objectOnPlatform = collision.GetComponent<Rigidbody2D>();
        if (objectOnPlatform != null)
        {
            objectsOnPlatform.Add(objectOnPlatform.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        var objectOnPlatform = collision.GetComponent<Rigidbody2D>();
        if (objectOnPlatform != null)
        {
            objectsOnPlatform.Remove(objectOnPlatform.transform);
        }
    }
}
