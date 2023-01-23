using UnityEngine;

public interface CollisionRespondent
{
    void InvokeCollisionRespose(Collision2D collision, string from);
    void InvokeTriggerResponse(Collider2D collider, string from);
}
