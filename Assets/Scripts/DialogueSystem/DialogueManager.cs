using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Yarn;

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
    [SerializeField] private GameObject m_inputRequiredSprite;
    private bool inProduction;
    private Dialogue lastDialogue;
    private Animator tinkerAnimator;
    private Animator asheAnimator;
    private bool asheHidden = true;
    private bool tinkerHidden = true;

    private void Start()
    {
        // Coupling
        m_dialogueProducer_left.TypeSound = tinkerData.Voice;
        m_dialogueProducer_right.TypeSound = asheData.Voice;
        m_dialogueProducer_left.TMP_access.margin = new Vector4(25f, 0, 25f, 0);
        m_dialogueProducer_right.TMP_access.margin = new Vector4(25f, 0, 25f, 0);
        m_inputRequiredSprite.SetActive(false);
        tinkerAnimator = profile_left.GetComponent<Animator>();
        asheAnimator = profile_right.GetComponent<Animator>();
        //HideDialogue();
    }
    private void Update()
    {
        
    }
    public Coroutine DisplayDialogue(Dialogue[] a_dialogues)
    {
        StopDialogue();
        inProduction = true;
        //dialogueBox.GetComponent<Animator>().Play("Appear");
        //ShowTinkerProfile();
        //ShowAsheProfile();
        return StartCoroutine(RunThroughDialogue(a_dialogues));
    }
    public Coroutine DisplayDialogue(DialogueObject a_dialogueObject)
    {
        StopDialogue();
        inProduction = true;
        //dialogueBox.GetComponent<Animator>().Play("Appear");
        //ShowTinkerProfile();
        //ShowAsheProfile();
        return StartCoroutine(RunThroughDialogue(a_dialogueObject));
    }
    public void HideDialogue()
    {
        //dialogueBox.GetComponent<Animator>().Play("Disappear");
        HideAsheProfile();
        HideTinkerProfile();
        inProduction = false;
    }
    public void HideAsheProfile()
    {
        if (asheHidden) return;
        asheHidden = true;
        asheAnimator?.Play("HideAshe");
    }
    public void HideTinkerProfile()
    {
        if (tinkerHidden) return;
        tinkerHidden = true;
        tinkerAnimator?.Play("HideTinker");
    }
    public void ShowAsheProfile()
    {
        if (!asheHidden) return;
        asheHidden = false;
        asheAnimator?.Play("ShowAshe");
    }
    public void ShowTinkerProfile()
    {
        if (!tinkerHidden) return;
        tinkerHidden = false;
        tinkerAnimator?.Play("ShowTinker");
    }
    public void StopDialogue()
    {
        if (inProduction)
        {
            StopAllCoroutines();
            m_dialogueProducer_left.StopProduction();
            m_dialogueProducer_right.StopProduction();
            m_inputRequiredSprite.SetActive(false);
            //HideDialogue(); // Hiding all by default
        }        
    }
    private IEnumerator RunThroughDialogue(DialogueObject a_dialogueObject)
    {
        yield return RunThroughDialogue(a_dialogueObject.Dialogue);
        //HideDialogue();
    }

    private IEnumerator RunThroughDialogue(Dialogue[] dialogues)
    {
        lastDialogue = dialogues[dialogues.Length - 1];
        foreach (Dialogue dialogue in dialogues)
        {
            //dialogue.OnDialogue.Invoke(); // Any Events that the Dialogue has
            //AdjustProfileSegment(dialogue.Profile, dialogue.Alignment);
            TextProducer dialogueProducer;
            Image profile;
            if (dialogue.Alignment == ProfileAlignment.Tinker)
            {
                dialogueProducer = m_dialogueProducer_left;
                profile = profile_left;
                ShowTinkerProfile();
            }
            else if (dialogue.Alignment == ProfileAlignment.Ashe)
            {
                dialogueProducer = m_dialogueProducer_right;
                profile = profile_right;
                ShowAsheProfile();
            }
            else if (dialogue.Alignment == ProfileAlignment.External)
            {
                dialogueProducer = GameManager.Instance.LevelManager.ExternalDialogues[dialogue.ExternalID];
                if (dialogue.CharSound != null) dialogueProducer.TypeSound = dialogue.CharSound;
                profile = null;
            }
            else
            {
                break;
            }

            if (profile != null && dialogue.Profile != null) profile.sprite = dialogue.Profile;
            if (dialogue.NoWaiting)
            {
                // Will just run without waiting on dialogue to finish
                // Intended for Async Dialogue!
                dialogueProducer.ReplaceTextWith(dialogue.Text, ProduceEffect.Typewriter, dialogue.Speed, 0);
            }
            else if (dialogue.WaitOnInput)
            {
                yield return dialogueProducer.ReplaceTextWith(dialogue.Text, ProduceEffect.Typewriter, dialogue.Speed, 0);
                m_inputRequiredSprite.SetActive(true);
                yield return new WaitUntil(() =>
                {
                    // Scuffed -> could be more modular!
                    if (Gamepad.current != null) return Gamepad.current.buttonSouth.wasPressedThisFrame;
                    if (Keyboard.current != null) return Keyboard.current.anyKey.wasPressedThisFrame;
                    return false;
                });
                m_inputRequiredSprite.SetActive(false);
            }
            else
            {
                yield return dialogueProducer.ReplaceTextWith(dialogue.Text, ProduceEffect.Typewriter, dialogue.Speed, dialogue.Delay);
            }

            if (dialogue.HideProfileAfterDialogue)
            {
                if (dialogue.Alignment == ProfileAlignment.Ashe)
                    HideAsheProfile();
                if (dialogue.Alignment == ProfileAlignment.Tinker)
                    HideTinkerProfile();
            }
            

            if (!dialogue.KeepTextAfterDialogue) 
                dialogueProducer.ReplaceTextWith("", ProduceEffect.None, 1f, 0f);

            // Ordered so that text can clear then we wait
            if (dialogue.WaitOnInput && !dialogue.NoWaiting)
            {
                yield return new WaitForSeconds(dialogue.Delay);
            }

            //dialogueProducer.TypeSound = dialogue.TypeSound;
            // dialogueProducer.TMP_access.margin = new Vector4(25f, 0, 25f, 0);
            //dialogue.OnDialogue.Invoke();
        }

        // Writer Should mainly hide it
        inProduction = false;
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
