using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 
/// </summary>
public class DialogueManager : MonoBehaviour
{
    #region Singleton
    private static DialogueManager instance; 
    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogueManager>().GetComponent<DialogueManager>();
            }

            return instance;
        }
    }
    #endregion

    [SerializeField] private TextProducer m_dialogueProducer;
    [SerializeField] private Image profile;
    [SerializeField] private GameObject dialogueBox;
    private bool inProduction;

    private void Start()
    {
        HideDialogue();
    }

    private void Update()
    {
        
    }

    public void DisplayDialogue(DialogueObject a_dialogueObject)
    {
        if (inProduction) StopAllCoroutines();
        inProduction = true;
        dialogueBox.GetComponent<Animator>().Play("Appear");
        StartCoroutine(RunThroughDialogue(a_dialogueObject));
    }

    public void HideDialogue()
    {
        if (inProduction)
        {
            dialogueBox.GetComponent<Animator>().Play("Disappear");
            inProduction = false;
        }  
    }

    public void StopDialogue()
    {
        StopAllCoroutines();
        HideDialogue();
    }

    private IEnumerator RunThroughDialogue(DialogueObject a_dialogueObject)
    {
        foreach (Dialogue dialogue in a_dialogueObject.Dialogue)
        {
            AdjustProfileSegment(dialogue.Profile, dialogue.Alignment);
            m_dialogueProducer.TypeSound = dialogue.TypeSound;
            m_dialogueProducer.TMP_access.margin = new Vector4(25f, 0, 25f, 0);
            yield return m_dialogueProducer.ReplaceTextWith(dialogue.Text, ProduceEffect.Typewriter, dialogue.Speed, dialogue.Delay);
        }

        HideDialogue();
        yield return null;
    }

    private void AdjustProfileSegment(Sprite a_profile, ProfileAlignment alignment)
    {
        if (profile == null) return;
        profile.gameObject.SetActive(true);
        profile.sprite = null;
        profile.CrossFadeAlpha(0f, 0f, true);
        m_dialogueProducer.TMP_access.alignment = TextAlignmentOptions.Center;
        profile.sprite = a_profile;
        if (profile.sprite != null)
        {
            profile.transform.localPosition = new Vector3(Mathf.Abs(profile.transform.localPosition.x) * (alignment == ProfileAlignment.Right ? 1 : -1) 
                , profile.transform.localPosition.y, profile.transform.localPosition.z);
            m_dialogueProducer.TMP_access.alignment = alignment == ProfileAlignment.Right ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;
            profile.CrossFadeAlpha(1f, 0f, true);
        }
    }
}
