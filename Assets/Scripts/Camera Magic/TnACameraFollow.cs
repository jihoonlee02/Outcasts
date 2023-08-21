using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TnACameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = .5f;
    [SerializeField] private bool isLocked = true;
    [SerializeField] private bool lockVertical = true;
    [SerializeField] private bool lockHorizontal = false;
    #region technical
    private Vector3 velocity;
    private void Start()
    {
        Physics2D.IgnoreLayerCollision(30, 8, true);
        Physics2D.IgnoreLayerCollision(30, 0, true);
    }
    #endregion
    void LateUpdate()
    {
        if (isLocked)
        {
            return;
        }
        if (GameManager.Instance.Tinker != null && GameManager.Instance.Ashe != null)
        {
            var centerPoint = GetCenterPoint();
            Vector3 newPosition = new Vector3(lockHorizontal ? transform.position.x : centerPoint.x, 
                lockVertical ? transform.position.y : centerPoint.y, transform.position.z) + offset;

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
    public void UnLock()
    {
        isLocked = false;
    }
    public void Lock()
    {
        isLocked = true;
    }
}
