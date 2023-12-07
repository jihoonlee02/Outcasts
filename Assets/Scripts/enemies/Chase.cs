using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
// Dependent on the existence of TnA since we will NOT have any other player pawns
public class Chase : MonoBehaviour
{
    [SerializeField] private bool m_chaseTNA;
    [SerializeField] private float m_speed = 2f;
    [SerializeField] private float m_angleOffset = -90f;
    [SerializeField] private Transform[] targets;
    [SerializeField] private bool isChasing = false;
    private Rigidbody2D m_rb;
    private Vector3 moveDirection;
    private bool isGrabbing = false;
    private Transform grabbedTarget = null;
    private int arrSize;
    
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        arrSize = targets.Length;
        if (m_chaseTNA)
        {
            ChangeTargetsToTna();
        }
    }

    private void FixedUpdate()
    {
        ChaseTargets(); // Slower as method, but easier to read and clearer if we had more components
    }
    private void Update()
    {
        if (isGrabbing) grabbedTarget.position = transform.position;
    }
    private void ChaseTargets()
    {
        // Figure out whos the closest
        if (arrSize == 0) return;
        Transform closestTarget = targets[0];
        float closestDistance = Mathf.Infinity;
        for (int i = 0; i < arrSize; i++)
        {
            if (targets[i].position.magnitude < closestDistance)
            {
                closestDistance = targets[i].position.magnitude;
                closestTarget = targets[i];
            }
        }

        // Calculate that movement
        moveDirection = (closestTarget.position - transform.position).normalized;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        m_rb.rotation = Mathf.LerpAngle(m_rb.rotation, angle + m_angleOffset, Time.deltaTime * m_speed);
        if (isChasing) m_rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * m_speed;
        if (!isChasing) m_rb.velocity = Vector2.zero;
    }
    public void StartChase()
    {
        isChasing = true;
    }
    public void StopChase() 
    {
        isChasing = false;
    }
    // When using the ChangeTargets Methods, all original targets get replaced
    // Would be fine, but they should prioritize one or the other really.
    public void ChangeTargetsToTna()
    {
        targets = new Transform[] { GameManager.Instance.Tinker.transform, GameManager.Instance.Ashe.transform };
        arrSize = 2;
    }
    public void ChangeTargets(Transform transform)
    {
        targets = new Transform[5];
        targets[0] = transform;
        arrSize = 1;
    }
    public void ChangeTargets(Transform[] transform)
    {
        targets = new Transform[transform.Length + 5];
        transform.CopyTo(targets, 0);
        arrSize = transform.Length;
    }
    public void AddTarget(Transform transform)
    {
        if (arrSize >= targets.Length)
        {
            // Resize Array
            var temp = targets;
            targets = new Transform[temp.Length * 2];
            temp.CopyTo(targets, 0);
        }
        targets[arrSize++] = transform;
    }
    public void ChangeSpeed(float speed)
    {
        m_speed = speed;
    }
    public void GrabTarget(Transform target)
    {
        if (!targets.Contains(target)) return;
        if (target.tag == "Tinker")
        {
            target.GetComponent<TinkerPawn>().IsHeld = false;
            GameManager.Instance.Ashe.IsLifting = false;
        }
        grabbedTarget = target;
        isGrabbing = true;
    }
}
