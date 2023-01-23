using UnityEngine;

public class CollisionReporter : MonoBehaviour
{
    [SerializeField] private CollisionRespondent m_cr;
    [SerializeField] private string m_name;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_cr.InvokeCollisionRespose(collision, m_name);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        m_cr.InvokeTriggerResponse(collider, m_name);
    }

}
