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

    [Header("Bounds")]
    [SerializeField] public float leftBound = 0;
    [SerializeField] public bool boundTheLeft = false;
    [SerializeField] public float rightBound;
    [SerializeField] public bool boundTheRight = false;
    [SerializeField] public float topBound;
    [SerializeField] public bool boundTheTop = false;
    [SerializeField] public float botBound;
    [SerializeField] public bool boundTheBot = false;
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
                lockVertical ? transform.position.y : centerPoint.y, transform.position.z);

            bool inBound = false;
            Vector3 boundedPosition = newPosition;
            if (boundTheLeft && newPosition.x < leftBound)
            {
                boundedPosition = new Vector3(leftBound, boundedPosition.y);
                inBound = true;
            }
            else if (boundTheRight && newPosition.x > rightBound)
            {
                boundedPosition = new Vector3(rightBound, boundedPosition.y);
                inBound = true;
            }
            if (boundTheBot && newPosition.y < botBound)
            {
                boundedPosition = new Vector3(boundedPosition.x, botBound);
                inBound = true;
            }
            else if (boundTheTop && newPosition.y > topBound)
            {
                boundedPosition = new Vector3(boundedPosition.x, topBound);
                inBound = true;
            }

            transform.position = Vector3.SmoothDamp(transform.position, (inBound ? boundedPosition : newPosition) + offset, ref velocity, smoothTime);
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
    public void SetSmoothTime(float time)
    {
        smoothTime = time;
    }
}
