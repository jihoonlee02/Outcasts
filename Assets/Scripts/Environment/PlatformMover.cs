using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class PlatformMover : Invokee
{
    [Header("Options")]
    [SerializeField] private Vector2[] m_waypoints;
    [SerializeField, Range(0f, 10f)] private float speed = 1f;
    [SerializeField] private bool autoMove;

    # if UNITY_EDITOR
    private bool isSelected => UnityEditor.Selection.transforms.Contains(transform);
    #endif
    private int idx = 0;
    private Vector2 moveDifference;
    private Dictionary<Transform, Transform> objectsOnPlatform;
    public int moveDifferenceMultiplier;
    private bool blocked = false;
    private int count_collisions;

    private void Start()
    {
        transform.localPosition = m_waypoints[idx];
        objectsOnPlatform = new Dictionary<Transform, Transform>();
        moveDifference = transform.localPosition;
    }

    
    private void Update()
    {
        

        //foreach(var objectOnPlatform in objectsOnPlatform)
       // {
            //moveDifference *= -1;
            //objectOnPlatform.Translate(moveDifference);
       // }
    }
    private void FixedUpdate()
    {
        if (!blocked)
        {
            if (autoMove && (Vector2)transform.localPosition == m_waypoints[idx]) idx = (idx + 1) % m_waypoints.Length;
            moveDifference = transform.localPosition;
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, m_waypoints[idx], Time.deltaTime * 3f * speed);
            moveDifference -= (Vector2)transform.localPosition;
        }
    }

    public void MoveToWaypoint(int idx)
    {
        this.idx = idx;
        blocked = false;
    }
    protected override void OnActivate()
    {
        MoveToWaypoint(1);
    }
    protected override void OnDeactivate()
    {
        MoveToWaypoint(0);
    }

    public void ChangePlatformSpeed(float speed)
    {
        this.speed = speed;
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
        if (objectOnPlatform != null && !objectOnPlatform.GetComponent<Chase>())
        {
            objectsOnPlatform.Add(objectOnPlatform.transform, objectOnPlatform.transform.parent);
            objectOnPlatform.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var objectOnPlatform = collision.GetComponent<Rigidbody2D>();
        if (objectOnPlatform != null && objectsOnPlatform.ContainsKey(objectOnPlatform.transform))
        {
            objectOnPlatform.transform.SetParent(objectsOnPlatform[objectOnPlatform.transform]);
            objectsOnPlatform.Remove(objectOnPlatform.transform);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Players") && collision.gameObject.tag != "physical")
        {
            count_collisions++;
            blocked = true;
            Debug.Log("BLOCKED BY " + collision.gameObject.name);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Players") && collision.gameObject.tag != "physical")
        {
            count_collisions--;
        }

        if (count_collisions <= 0)
        {
            blocked = false;
            Debug.Log("UN-BLOCKED");
        }
    }
}
