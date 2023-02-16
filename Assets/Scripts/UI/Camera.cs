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
    [SerializeField, Range(1f, 10f)] private float m_moveSpeed = 1f;
    [SerializeField] private Vector3 targetPosition;

    [Header("Playful")]
    [SerializeField] private CameraShake m_cameraShaker;

    public CameraShake CamShaker => m_cameraShaker;

    #region Technical

    #endregion

    public void ShiftTo(Vector3 targetPosition)
    {
        this.targetPosition = new Vector3(targetPosition.x, targetPosition.y, -10f);
    }

    private void Start()
    {
        StartCoroutine(something());
    }

    private void Update()
    {
        if (transform.position != targetPosition && !m_cameraShaker.IsShaking)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, m_moveSpeed * Time.deltaTime);
        }
    }

    private IEnumerator something()
    {
        yield return new WaitForSeconds(2.15f);
        m_cameraShaker.StartShaking();
        yield return new WaitForSeconds(0.35f);
        m_cameraShaker.StopShaking();
    }
}
