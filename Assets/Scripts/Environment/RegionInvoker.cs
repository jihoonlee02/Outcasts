using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider2D))]
public class RegionInvoker : Invoker
{
    [SerializeField] private bool triggerOnce = true;
    [SerializeField] private float stayTimeToTrigger = 0f;
    [SerializeField] private TriggerType triggerType;

    [Header("Player Specific")]
    [SerializeField] private bool useUniqueID = false;
    [SerializeField] private int tinkerSpecificID;
    [SerializeField] private int asheSpecificID;


    [Header("Object Specific")]
    [SerializeField] private string[] tags;

    private float TimeDuration;

    #region Technical
    private bool asheInRegion = false;
    private bool tinkerInRegion = false;
    #endregion 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // This is why level 6 takes forever because objects just keep entering!
        // TODO: Resolve this dependency!
        if (Time.time <= TimeDuration
            || (triggerType == TriggerType.TinkerOnly && collision.gameObject.tag != "Tinker")
            || (triggerType == TriggerType.AsheOnly && collision.gameObject.tag != "Ashe")
            || collision.gameObject.tag == "feet")
        {
            return;
        }
        TimeDuration = Time.time + stayTimeToTrigger;
        if (triggerType != TriggerType.SpecificObjects && collision.gameObject.layer == LayerMask.NameToLayer("Players"))
        {
            if (triggerType == TriggerType.BothPlayersRequired)
            {
                if (collision.gameObject.tag == "Tinker")
                {
                    tinkerInRegion = true;
                    if (!asheInRegion) return;
                }
                else if (collision.gameObject.tag == "Ashe")
                {
                    asheInRegion = true;
                    if (!tinkerInRegion) return;
                }
                else
                {
                    return;
                }
            }
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
                Destroy(this);
            }
        }
        else if (triggerType == TriggerType.SpecificObjects)
        {
            if (triggerOnce)
            {
                Destroy(this);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (stayTimeToTrigger < 0 || Time.time <= TimeDuration
            || (triggerType == TriggerType.TinkerOnly && collision.gameObject.tag != "Tinker")
            || (triggerType == TriggerType.AsheOnly && collision.gameObject.tag != "Ashe")
            || collision.gameObject.tag == "feet")
        {
            return;
        }
        if (triggerType != TriggerType.SpecificObjects && collision.gameObject.layer == LayerMask.NameToLayer("Players"))
        {
            if (triggerType == TriggerType.BothPlayersRequired)
            {
                if (collision.gameObject.tag == "Tinker")
                {
                    tinkerInRegion = true;
                    if (!asheInRegion) return;
                }
                else if (collision.gameObject.tag == "Ashe")
                {
                    asheInRegion = true;
                    if (!tinkerInRegion) return;
                }
                else
                {
                    return;
                }
            }
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
                Destroy(this);
            }
        }
        else if (triggerType == TriggerType.SpecificObjects && tags.Contains(collision.tag))
        {
            Activate();
            if (triggerOnce)
            {
                Destroy(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((triggerType == TriggerType.TinkerOnly && collision.gameObject.tag != "Tinker")
            || (triggerType == TriggerType.AsheOnly && collision.gameObject.tag != "Ashe")
            || collision.gameObject.tag == "feet")
        {
            return;
        } 
        if (triggerType != TriggerType.SpecificObjects && collision.gameObject.layer == LayerMask.NameToLayer("Players"))
        {
            if (triggerType == TriggerType.BothPlayersRequired)
            {
                if (collision.gameObject.tag == "Tinker")
                {
                    tinkerInRegion = false;
                }
                else if (collision.gameObject.tag == "Ashe")
                {
                    asheInRegion = false;
                }
            }
            if (useUniqueID)
            {
                if (collision.gameObject.tag == "Tinker")
                {
                    Deactivate(tinkerSpecificID);
                }
                else if (collision.gameObject.tag == "Ashe")
                {
                    Deactivate(asheSpecificID);
                }
            }
            else
            {
                if (!triggerOnce) Deactivate();
            }
        }
        else if (triggerType == TriggerType.SpecificObjects && tags.Contains(collision.tag))
        {
            if (!triggerOnce) Deactivate();
        }
    }

    private enum TriggerType
    {
        AnyPlayer,
        TinkerOnly,
        AsheOnly,
        BothPlayersRequired,
        SpecificObjects
    }
}
