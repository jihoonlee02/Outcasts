using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [SerializeField] private float speed;
    private void Update()
    {
        transform.localPosition = Vector2.LerpUnclamped(transform.localPosition, transform.localPosition + Vector3.left, Time.deltaTime * speed);
    }
}
