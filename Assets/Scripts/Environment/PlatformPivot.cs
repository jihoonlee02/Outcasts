using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPivot : MonoBehaviour
{
    [SerializeField] private Collider2D m_triggerBox;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerPawn>() != null)
            collision.transform.SetParent(transform, false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerPawn>() != null)
            collision.transform.parent = null;
    }
}
