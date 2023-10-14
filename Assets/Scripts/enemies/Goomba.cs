using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    [SerializeField] private Vector3 dir = Vector2.right;
    [SerializeField] private float speed = 1f;
    private void FixedUpdate()
    {
        transform.position += dir * speed * Time.deltaTime;
    }
}
