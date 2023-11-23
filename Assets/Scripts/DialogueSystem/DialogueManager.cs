using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Profiling;
using static UnityEditor.Searcher.SearcherWindow;

/// <summary>
/// Dialogue Manager 1.0 -> Maybe Let's refactor and change this guy up to be uncoupled from monobehaviour
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

    [SerializeField] private TextProducer m_dialogueProducer_left;
    [SerializeField] private TextProducer m_dialogueProducer_right;
    [SerializeField] private Image profile_left;
    [SerializeField] private Image profile_right;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private PawnData tinkerData;
    [SerializeField] private PawnData asheData;
    private bool inProduction;

    private void Start()
    {
        HideDialogue();
        // Coupling
        m_dialogueProducer_left.TypeSound = tinkerData.Voice;
        m_dialogueProducer_right.TypeSound = asheData.Voice;
        m_dialogueProducer_left.TMP_access.margin = new Vector4(25f, 0, 25f, 0);
        m_dialogueProducer_right.TMP_access.margin = new Vector4(25f, 0, 25f, 0);
    }
    private void Update()
    {
        
    }
    public void DisplayDialogue(Dialogue[] a_dialogues)
    {
        if (inProduction)
        {
            StopAllCoroutines();
            m_dialogueProducer_left.StopProduction();
            m_dialogueProducer_right.StopProduction();
        }
        inProduction = true;
        dialogueBox.GetComponent<Animator>().Play("Appear");
        StartCoroutine(RunThroughDialogue(a_dialogues));
    }
    public void DisplayDialogue(DialogueObject a_dialogueObject)
    {
        if (inProduction)
        {
            StopAllCoroutines();
            m_dialogueProducer_left.StopProduction();
            m_dialogueProducer_right.StopProduction();
        }
        inProduction = true;
        dialogueBox.GetComponent<Animator>().Play("Appear");
        StartCoroutine(RunThroughDialogue(a_dialogueObject));
    }
    public void HideDialogue()
    {
         dialogueBox.GetComponent<Animator>().Play("Disappear");
         inProduction = false;
    }
    public void StopDialogue()
    {
        if (inProduction)
        {
            StopAllCoroutines();
            HideDialogue();
        }        
    }
    private IEnumerator RunThroughDialogue(DialogueObject a_dialogueObject)
    {
        foreach (Dialogue dialogue in a_dialogueObject.Dialogue)
        {
            dialogue.OnDialogue.Invoke(); // Any Events that the Dialogue has
            //AdjustProfileSegment(dialogue.Profile, dialogue.Alignment);
            if (dialogue.Alignment == ProfileAlignment.Left)
            {
                // Tinker
                profile_left.sprite = dialogue.Profile;
                yield return m_dialogueProducer_left.ReplaceTextWith(dialogue.Text, ProduceEffect.Typewriter, dialogue.Speed, dialogue.Delay);

                // Auto Clear after done
                m_dialogueProducer_left.ReplaceTextWith("", ProduceEffect.None, 1f, 0f);
            }
            else
            {
                // Ashe
                profile_right.sprite = dialogue.Profile;
                yield return m_dialogueProducer_right.ReplaceTextWith(dialogue.Text, ProduceEffect.Typewriter, dialogue.Speed, dialogue.Delay);

                // Auto Clear after done
                m_dialogueProducer_right.ReplaceTextWith("", ProduceEffect.None, 1f, 0f);
            }

            //dialogueProducer.TypeSound = dialogue.TypeSound;
            // dialogueProducer.TMP_access.margin = new Vector4(25f, 0, 25f, 0);
            //dialogue.OnDialogue.Invoke();
            

        }

        HideDialogue();
        yield return null;
    }

    private IEnumerator RunThroughDialogue(Dialogue[] dialogues)
    {
        foreach (Dialogue dialogue in dialogues)
        {
            dialogue.OnDialogue.Invoke(); // Any Events that the Dialogue has
            //AdjustProfileSegment(dialogue.Profile, dialogue.Alignment);
            if (dialogue.Alignment == ProfileAlignment.Left)
            {
                // Tinker
                profile_left.sprite = dialogue.Profile;
                yield return m_dialogueProducer_left.ReplaceTextWith(dialogue.Text, ProduceEffect.Typewriter, dialogue.Speed, dialogue.Delay);

                // Auto Clear after done
                m_dialogueProducer_left.ReplaceTextWith("", ProduceEffect.None, 1f, 0f);
            }
            else
            {
                // Ashe
                profile_right.sprite = dialogue.Profile;
                yield return m_dialogueProducer_right.ReplaceTextWith(dialogue.Text, ProduceEffect.Typewriter, dialogue.Speed, dialogue.Delay);

                // Auto Clear after done
                m_dialogueProducer_right.ReplaceTextWith("", ProduceEffect.None, 1f, 0f);
            }

            //dialogueProducer.TypeSound = dialogue.TypeSound;
            // dialogueProducer.TMP_access.margin = new Vector4(25f, 0, 25f, 0);
            //dialogue.OnDialogue.Invoke();


        }

        // Usuer Should mainly hide it
        //HideDialogue();
        yield return null;
    }
    private void AdjustProfileSegment(Sprite a_profile, ProfileAlignment alignment)
    {
        //var profile = alignment == ProfileAlignment.Left ? profile_left : profile_right;

        //profile.gameObject.SetActive(true);
        //profile.sprite = null;
        //profile.CrossFadeAlpha(0f, 0f, true);
        //m_dialogueProducer.TMP_access.alignment = TextAlignmentOptions.Center;
        //profile.sprite = a_profile;

        //// [Old] when Profile's were any character dialogue
        //profile.gameObject.SetActive(true);
        //profile.sprite = null;
        //profile.CrossFadeAlpha(0f, 0f, true);
        //m_dialogueProducer.TMP_access.alignment = TextAlignmentOptions.Center;
        //profile.sprite = a_profile;
        //profile.transform.localPosition = new Vector3(Mathf.Abs(profile.transform.localPosition.x) * (alignment == ProfileAlignment.Right ? 1 : -1)
        //        , profile.transform.localPosition.y, profile.transform.localPosition.z);
        //m_dialogueProducer.TMP_access.alignment = alignment == ProfileAlignment.Right ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;

        //if (a_profile != null)
        //{
        //    profile.CrossFadeAlpha(1f, 0f, true);
        //}      
    }
}
