using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Camera : MonoBehaviour
{
    #region Singleton
    private static Camera instance;
    public static Camera Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Camera>();
                if (instance == null)
                {
                    Debug.LogError("Camera Prefab is required in level scene!");
                }
            }

            return instance;
        }
    }
    #endregion

    [Header("Modifiers")]
    [SerializeField, Range(1f, 30f)] private float m_moveSpeed = 1f;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] public bool m_followEnabled = false;
    [SerializeField] public bool m_smooth = false;

    [Header("Playful")]
    [SerializeField] private CameraShake m_cameraShaker;
    [SerializeField] private TnACameraFollow m_tnaCameraFollow;

    public CameraShake CameraShaker => m_cameraShaker;
    public TnACameraFollow TnACameraFollow => m_tnaCameraFollow;

    #region Technical

    #endregion
    private void Awake()
    {
        m_tnaCameraFollow = GetComponent<TnACameraFollow>();
    }
    public void ShiftTo(Vector3 targetPosition)
    {
        this.targetPosition = new Vector3(targetPosition.x, targetPosition.y, -10f);
    }
    public void ShiftToX(float x)
    {
        targetPosition = new Vector3(x, targetPosition.y, -10f);
    }
    public void ShiftToY(float y)
    {
        targetPosition = new Vector3(targetPosition.x, y, -10f);
    }

    private void Start()
    {
        this.targetPosition = new Vector3(targetPosition.x, targetPosition.y, -10f);
    }

    private void Update()
    {
        if (m_followEnabled && transform.position != targetPosition && (m_cameraShaker == null || !m_cameraShaker.IsShaking))
        {
            transform.position = m_smooth ? Vector3.LerpUnclamped(transform.position, targetPosition, Time.deltaTime * m_moveSpeed * 0.1f) :
                Vector3.MoveTowards(transform.position, targetPosition, m_moveSpeed * Time.deltaTime);
        }
    }
    public void FollowEnable(bool follow)
    {
        m_followEnabled = follow;
    }
    public void FollowSmooth(bool smooth)
    {
        m_smooth = smooth;
    }
    public void ChangeMoveSpeed(float speed)
    {
        m_moveSpeed = speed;
    }
}
