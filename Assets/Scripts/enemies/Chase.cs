using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
// Dependent on the existence of TnA since we will NOT have any other player pawns
public class Chase : MonoBehaviour
{
    [SerializeField] private float m_speed = 2f;
    [SerializeField] private float m_angleOffset = -90f;
    [SerializeField] private Transform[] targets;
    public Transform[] Targets => targets;
    [SerializeField] private bool isChasing = false;
    [SerializeField] private bool prioritizeFirstTarget;
    [SerializeField, Tooltip("Targets that are not 0 are now likely to be hit in this range")] private float m_anyTargetRadius = 3f;
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
    }

    private void FixedUpdate()
    {
        if (isChasing) ChaseTargets(); // Slower as method, but easier to read and clearer if we had more components
        else m_rb.velocity = Vector2.zero;
    }
    private void Update()
    {
        if (isGrabbing) grabbedTarget.position = transform.position;
    }
    private void ChaseTargets()
    {
        // Figure out whos the closest and priority
        if (arrSize == 0) return;
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;
        for (int i = 0; i < arrSize; i++)
        {
            if (targets[i].GetComponent<Grabbed>()) continue;
            var mag = (targets[i].position - transform.position).magnitude;
            if (mag < closestDistance && (!prioritizeFirstTarget || (i == 0 || mag <= m_anyTargetRadius)))
            {
                closestDistance = targets[i].position.magnitude;
                closestTarget = targets[i];
            }
        }

        // No Targets found then return
        if (closestTarget == null) return;

        // Calculate that movement
        moveDirection = (closestTarget.position - transform.position).normalized;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        m_rb.rotation = Mathf.LerpAngle(m_rb.rotation, angle + m_angleOffset, Time.deltaTime * m_speed);
        m_rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * m_speed;
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
    public void ChangeTargetsToTna(bool prioritizeAshe)
    {
        targets = prioritizeAshe ? new Transform[] { GameManager.Instance.Ashe.transform, GameManager.Instance.Tinker.transform } 
        : new Transform[] { GameManager.Instance.Tinker.transform, GameManager.Instance.Ashe.transform };
        arrSize = 2;
    }
    public void ChangeTargetToTinker()
    {
        targets = new Transform[] { GameManager.Instance.Tinker.transform };
        arrSize = 1;
    }
    public void ChangeTargetToAshe()
    {
        targets = new Transform[] { GameManager.Instance.Ashe.transform };
        arrSize = 1;
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
        if (target == null || target.GetComponent<Grabbed>()) return;
        if (target.tag == "Tinker")
        {
            target.GetComponent<TinkerPawn>().IsHeld = false;
            GameManager.Instance.Ashe.IsLifting = false;
        }
        grabbedTarget = target;
        isGrabbing = true;
        target.gameObject.AddComponent<Grabbed>();
    }

    public void UnGrabTarget()
    {
        if (grabbedTarget == null) return;
        isGrabbing = false;
        Destroy(grabbedTarget.GetComponent<Grabbed>());
        grabbedTarget = null;
    }
}
