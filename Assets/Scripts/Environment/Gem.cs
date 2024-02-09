using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Gem : MonoBehaviour
{
    [SerializeField] private bool isTinkerGem;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTinkerGem && collision.tag == "Tinker")
        {
            GemTracker.Instance.TinkerCollectsGem();
            Destroy(gameObject);
        }
        else if (!isTinkerGem && collision.tag == "Ashe")
        {
            GemTracker.Instance.AsheCollectsGem();
            Destroy(gameObject);
        }
    }
}
