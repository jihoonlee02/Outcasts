using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RegionInvoker : Invoker
{
    [SerializeField] private bool triggerOnce = true;
    [SerializeField] private bool playerTrigger = true;
    [SerializeField] private bool requireSpecficObjects = false;
    [SerializeField] private string[] tags;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (playerTrigger && (collider.gameObject.layer == LayerMask.NameToLayer("Players")))
        {
            Activate();

            if (triggerOnce) 
            { 
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerTrigger && (collision.gameObject.layer == LayerMask.NameToLayer("Players")))
        {
            Activate();

            if (triggerOnce)
            {
                Destroy(gameObject);
            }
        }
    }
}
