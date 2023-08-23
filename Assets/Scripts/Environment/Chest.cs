using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private int chestID;
    [SerializeField] private Sprite openChestSprite;
    private Animator animator;
    private bool isOpen = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (ChestTracker.Instance.IsChestFound(chestID))
        {
            GetComponent<SpriteRenderer>().sprite = openChestSprite;
            isOpen = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpen && collision.GetComponent<Pawn>())
        {
            animator.Play("ChestOpen");
            ChestTracker.Instance.FoundNewChest(chestID);
            isOpen = true;
        }
    }
}
