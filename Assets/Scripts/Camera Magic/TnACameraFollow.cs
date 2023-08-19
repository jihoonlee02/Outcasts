using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TnACameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = .5f;
    [SerializeField] private Collider2D m_leftBound;

    #region technical
    private Vector3 velocity;
    #endregion
    void LateUpdate()
    {
        if (GameManager.Instance.Tinker != null && GameManager.Instance.Ashe != null)
        {
            Vector3 newPosition = new Vector3(GetCenterPoint().x, transform.position.y, transform.position.z) + offset;

            if (newPosition.x < 0)
            {
                transform.position = Vector3.SmoothDamp(transform.position, Vector3.zero + offset, ref velocity, smoothTime);
                return;
            }

            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        }
        
    }
    Vector3 GetCenterPoint()
    {
        var bounds = new Bounds(GameManager.Instance.Tinker.transform.position, Vector2.zero);
        bounds.Encapsulate(GameManager.Instance.Ashe.transform.position);
        return bounds.center;
    }
}
