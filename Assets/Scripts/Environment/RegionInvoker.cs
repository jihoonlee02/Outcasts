using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RegionInvoker : Invoker
{
    [SerializeField] private bool triggerOnce = true;
    
    [Header("Player Specific")]
    [SerializeField] private bool playerTrigger = true;
    [SerializeField] private bool tinkerOnlyTrigger = false;
    [SerializeField] private bool asheOnlyTrigger = false;

    [Header("Unique ID Activation")]
    [SerializeField] private bool useUniqueID = false;
    [SerializeField] private int tinkerSpecificID;
    [SerializeField] private int asheSpecificID;
    [SerializeField] private float stayTimeToTrigger = 0f;

    [Header("Object Specific")]
    [SerializeField] private bool requireSpecficObjects = false;
    [SerializeField] private string[] tags;

    private float TimeDuration;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        TimeDuration = Time.time + stayTimeToTrigger;
        if ((tinkerOnlyTrigger && collider.gameObject.tag != "Tinker")
                    || (asheOnlyTrigger && collider.gameObject.tag != "Ashe"))
        {
            return;
        }

        if (Time.time >= TimeDuration && playerTrigger && (collider.gameObject.layer == LayerMask.NameToLayer("Players")))
        {
            if (useUniqueID)
            {
                if (collider.gameObject.tag == "Tinker")
                {
                    Activate(tinkerSpecificID);
                }
                else if (collider.gameObject.tag == "Ashe")
                {
                    Activate(asheSpecificID);
                }
            }
            else
            { 
                Activate();
            }
            

            if (triggerOnce) 
            { 
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((tinkerOnlyTrigger && collision.gameObject.tag != "Tinker")
                    || (asheOnlyTrigger && collision.gameObject.tag != "Ashe"))
        {
            return;
        }
        if (Time.time >= TimeDuration && playerTrigger && (collision.gameObject.layer == LayerMask.NameToLayer("Players")))
        {
            if (useUniqueID)
            {
                if (collision.gameObject.tag == "Tinker")
                {
                    Activate(tinkerSpecificID);
                }
                else if (collision.gameObject.tag == "Ashe")
                {
                    Activate(asheSpecificID);
                }
            }
            else
            {
                Activate();
            }


            if (triggerOnce)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((tinkerOnlyTrigger && collision.gameObject.tag != "Tinker")
                    || (asheOnlyTrigger && collision.gameObject.tag != "Ashe"))
        {
            return;
        }
        if (Time.time >= TimeDuration && playerTrigger && (collision.gameObject.layer == LayerMask.NameToLayer("Players")))
        {
            if (useUniqueID)
            {
                if (collision.gameObject.tag == "Tinker")
                {
                    Activate(tinkerSpecificID);
                }
                else if (collision.gameObject.tag == "Ashe")
                {
                    Activate(asheSpecificID);
                }
            }
            else
            {
                Activate();
            }


            if (triggerOnce)
            {
                Destroy(gameObject);
            }
        }
    }
}
