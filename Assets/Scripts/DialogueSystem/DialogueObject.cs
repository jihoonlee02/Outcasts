using UnityEngine;
using UnityEngine.Events;

//#if UNITY_EDITOR
//using UnityEditor;
//using UnityEditorInternal;
//#endif

[CreateAssetMenu(menuName = "Data/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    //[Header("Overrides")]
    //[SerializeField] private Sprite profile;
    //[SerializeField] private float speed;
    //[SerializeField] private float delay;
    //[SerializeField] private AudioClip typeSound;

    [SerializeField] private Dialogue[] dialogue;
    public Dialogue[] Dialogue => dialogue;

    private void Awake()
    {
        //foreach (var dialogue in dialogue)
        //{
        //    if (profile != null) dialogue.Profile = profile;
        //}
    }
}

[System.Serializable]
public struct Dialogue
{
    [Header("Dialogue")]
    [SerializeField] private Sprite profile;
    [SerializeField] private ProfileAlignment alignment;
    [SerializeField] private int externalID;
    [SerializeField, Tooltip("None uses default PawnData Sound, otherwise plays this once")] 
    private AudioClip typeSound;
    [SerializeField][TextArea] private string text;

    [Header("Transition Details")]
    [SerializeField] private float speed;
    [SerializeField, Tooltip("Waits delay time after dialogue runs. Waiting on input delays after it")]
    private float delay;
    [SerializeField] private bool waitOnInput;
    [SerializeField, Tooltip("Skips Delay and Wait On Input. Works best if next transition is different producer")] 
    private bool noWaiting;
    [SerializeField] private bool hideProfileAfterDialogue;
    [SerializeField] private bool keepTextAfterDialogue;

    // Unecessary, handled by PawnEventData now
    //[SerializeField] private UnityEvent onDialogue; 
    
    // Dialogue related //
    public Sprite Profile => profile;
    public ProfileAlignment Alignment => alignment;
    public int ExternalID => externalID;
    public AudioClip TypeSound => typeSound;
    public string Text => text;

    // Transition Related //
    public float Speed => speed;
    public bool WaitOnInput => waitOnInput;
    public float Delay => delay;
    public bool NoWaiting => noWaiting;
    public bool HideProfileAfterDialogue => hideProfileAfterDialogue;
    public bool KeepTextAfterDialogue => keepTextAfterDialogue;

    // Unecessary, handled by PawnEventData now
    //public UnityEvent OnDialogue => onDialogue;
}

public enum ProfileAlignment
{
    Tinker,
    Ashe,
    External
}

//#if UNITY_EDITOR
//[CustomEditor(typeof(DialogueObject)), CanEditMultipleObjects]
//public class DialogueObjectEditor : Editor
//{
//    int size;
//    bool dialogueFoldOut;
//    public override void OnInspectorGUI()
//    {
//        DialogueObject dialogueObject = (DialogueObject)target;

//        if (dialogueObject == null) { return; }

//        size = EditorGUILayout.IntField("Size", Mathf.Clamp(dialogueObject.dialogue.Length,0,100));
//        Array.Resize<string>(ref dialogueObject.dialogue, size);
//        Array.Resize<Sprite>(ref dialogueObject.profile, size);
//        Array.Resize<AudioClip>(ref dialogueObject.typeSound, size);
//        EditorGUILayout.Space();

//        dialogueFoldOut = EditorGUILayout.Foldout(dialogueFoldOut, "Dialogues");
//        for (int n = 0; n < dialogueObject.dialogue.Length; n++)
//        {
//            EditorGUILayout.LabelField("Dialogue " + (n + 1));
//            EditorGUILayout.BeginHorizontal();
//            dialogueObject.profile[n] = (Sprite) EditorGUILayout.ObjectField(dialogueObject.profile[n], typeof(Sprite), true);
//            dialogueObject.typeSound[n] = (AudioClip)EditorGUILayout.ObjectField(dialogueObject.typeSound[n], typeof(AudioClip), true);
//            EditorGUILayout.EndHorizontal();
//            dialogueObject.dialogue[n] = EditorGUILayout.TextArea(dialogueObject.dialogue[n], GUILayout.Height(50f));
//            EditorGUILayout.Space();
//        }
//    }
//}
//#endif

