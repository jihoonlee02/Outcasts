using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpin : MonoBehaviour
{
    [SerializeField] private float m_speed = 2f;
    private void FixedUpdate()
    {
        transform.Rotate(0f, 0f, m_speed * Time.deltaTime, Space.World);
    }
}
