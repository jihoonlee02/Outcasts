using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class CameraFollow : MonoBehaviour
{
    [Header("Basic Properties")]
    [SerializeField]
    private Transform target;

    [SerializeField]
    private bool following;

    [SerializeField, Range(0,100), Tooltip("The higher the value, the more time it will take for the camera to follow the player")]
    private float smoothness = 5f;

    [SerializeField, Range(0,100)]
    private float rotateSmooth = 4f;

    [Header("Position Offset")]
    [SerializeField]
    private float offX;
    public float OffX
    {
        set
        {
            offX = value;
        }
    }
    [SerializeField]
    private float offY;
    public float OffY
    {
        set
        {
            offY = value;
        }
    }
    [SerializeField]
    private float offZ;
    public float OffZ
    {
        set
        {
            offZ = value;
        }
    }

    [Header("Rotation Offset")]
    [SerializeField]
    private float rotX = 23.45f;
    [SerializeField]
    private float rotY;
    [SerializeField]
    private float rotZ;

    private Vector3 velocity = Vector3.zero;
    private Vector3 offset;
    private float rotationAngle;

    public Transform Target
    {
        set
        {
            target = value;
        }
    }

    public bool FollowTarget
    {
        set
        {
            following = value;
        }
    }

    void FixedUpdate()
    {
        if (following)
        {
            // Yes this is hacky and disgusting. Deal with it >:(
            rotationAngle = offZ < 0 ? 0f : offX < 0 ? 90f : offZ > 0 ? 180f : offX > 0 ? 270f : 0f;
            offset = new Vector3(offX, offY, offZ);
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothness * Time.deltaTime);
            transform.position = smoothedPosition;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotX, rotationAngle, 0f), Time.deltaTime * rotateSmooth);
            //transform.rotation = new Vector3(rotX, rotY, rotZ);
        }
        
        //transform.LookAt(target);
    }
    //For Button Purposes

    public void RotateLeft()
    {
        if (Mathf.Abs(offX) > 0)
        {
            offZ = (-1) * offX;
            offX = 0; 
        } else
        {
            offX = offZ;
            offZ = 0;
        }
    }

    public void RotateRight()
    {
        if (Mathf.Abs(offX) > 0)
        {
            offZ = offX;
            offX = 0;
        }
        else
        {
            offX = (-1) * offZ;
            offZ = 0;
        }
    }

    public void AdjustRotXValue(float x)
    {
        rotX += x;
    }

    public void AdjustXValue(float x)
    {
        offset += new Vector3(x, 0, 0);
    }

    public void AdjustYValue(float y)
    {
        offset += new Vector3(0, y, 0);
    }

    public void AdjustZValue(float z)
    {
        offset += new Vector3(0, 0, z);
    }
}
