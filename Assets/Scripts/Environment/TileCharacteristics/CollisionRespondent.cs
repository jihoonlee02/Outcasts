using System;
using UnityEngine;

public interface CollisionRespondent
{
    void InvokeCollisionRespose(Collision2D collision, string from)
    {
        Debug.Log("I was Collided");
    }
    void InvokeTriggerResponse(Collider2D collider, string from)
    {
        Debug.Log("I was triggered");
    }
}
