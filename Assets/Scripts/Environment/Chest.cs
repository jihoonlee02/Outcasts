using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpen && collision.GetComponent<Pawn>())
        {
            animator.Play("ChestOpen");
            isOpen = true;
        }
    }
}
