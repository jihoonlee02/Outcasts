using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Can be destroyed by GameObjects that invoke break on collision
public class Breakable : MonoBehaviour
{
    [SerializeField] private bool m_requiresAshe = true;
    [SerializeField, Tooltip("No Implementation with this one yet")] 
    private bool m_requiresTinker = false;
    public void Break()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_requiresAshe && m_requiresTinker) { Debug.Log("If and only if tinekr n ash lol"); }
        else if (m_requiresAshe && collision.gameObject.tag == "Gauntlet")
        {
            Break();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_requiresAshe && m_requiresTinker) { Debug.Log("If and only if tinekr n ash lol"); }
        else if (m_requiresAshe && collider.gameObject.tag == "Gauntlet")
        {
            Break();
        }
    }
}
