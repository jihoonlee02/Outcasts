using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private DialogueObject m_tinkerDeath;
    [SerializeField] private DialogueObject m_asheDeath;
    private bool enteredAlready = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var tag = collision.gameObject.tag;
        if ((tag == "Tinker" || tag == "Ashe") && !enteredAlready)
        {
            enteredAlready = true;
            StartCoroutine(OnCharacterDeath(tag));
        }
    }

    private IEnumerator OnCharacterDeath(string tag)
    {
        DialogueManager.Instance.DisplayDialogue(tag == "Tinker" ? m_tinkerDeath : m_asheDeath);
        yield return new WaitForSeconds(0.4f);
        GameManager.Instance.ReloadLevel();
    }
}
