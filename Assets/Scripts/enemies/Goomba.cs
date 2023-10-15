using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    [SerializeField] private Vector3 dir = Vector2.right;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float threshold = 1f;
    [SerializeField] private float duration_dir_change = 2f;
    private Vector3 prev_pos;
    private float cooldown;
    private void Start()
    {
        prev_pos = transform.position;
        cooldown = Time.time + duration_dir_change;
    }
    private void FixedUpdate()
    {
        transform.position += dir * speed * Time.deltaTime;
    }

    private void Update()
    {
        if (Time.time > cooldown)
        {          
            if (transform.position.x <= prev_pos.x + threshold && transform.position.x >= prev_pos.x - threshold)
            {
                Debug.Log("Direction Switch!!!");
                dir *= -1;
            }

            prev_pos = transform.position;
            cooldown = Time.time + duration_dir_change;
        }
        
    }
}
