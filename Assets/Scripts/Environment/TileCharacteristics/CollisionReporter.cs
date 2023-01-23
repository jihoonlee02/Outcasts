using UnityEngine;

public class CollisionReporter : MonoBehaviour
{
    [SerializeField] private GameObject m_cr;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_cr.GetComponent<CollisionRespondent>()?.InvokeCollisionRespose(collision, gameObject.name);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("It did this!");
        m_cr.GetComponent<CollisionRespondent>()?.InvokeTriggerResponse(collider, gameObject.name);
    }

}
