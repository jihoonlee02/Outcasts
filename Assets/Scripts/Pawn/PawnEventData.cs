using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

[CreateAssetMenu(menuName = "Data/PawnEventData")]
// Restricted to tinker n Ashe only
public class PawnEventData : ScriptableObject
{
    [SerializeField] private PawnEvent[] pawnEvents;
    public PawnEvent[] PawnEvents => pawnEvents;
}

[System.Serializable]
public struct PawnEvent
{
    // TODO: Be able to adjust both Tinker and Actions Actions in the Same Event
    // Include Dialogue to be ran in Dialogue Manager!
    [Header("Level General")] // More Efficent in a new editor
    [SerializeField] private string pawnEventTitle;
    [SerializeField] private bool pausePawnControl;
    [SerializeField] private bool resumePawnControl;
    [SerializeField] private float transitionTime;
    [Header("Pawn Specific")]
    [SerializeField] private PawnSelection pawnSelection;
    [SerializeField] private EventAction eventAction;
    [Header("Movement Event")]
    [SerializeField] private float timeDuration;
    [SerializeField] private Direction moveDirection;
    [SerializeField, Range(0.2f, 1f)] private float moveSpeed;

    [Header("Jump Event")]
    [SerializeField] private float jumpForce;

    [Header("Dialogue")]
    [SerializeField] private bool activeDialogueAtTime;
    [SerializeField, Tooltip("Transitions till dialogue finished as apposed to Transition Time")] private bool waitOnDialogue;
    [SerializeField] private Dialogue[] dialogues; // Cleaner than using a DialogueObject seperatly

    [Header("Invoke Event")]
    [SerializeField] private bool invoke;
    [SerializeField] private bool activate;
    [SerializeField] private int id;
    public string Title => pawnEventTitle;
    public bool PausePawnControl => pausePawnControl;
    public bool ResumePawnControl => resumePawnControl;
    public PawnSelection PawnSelection => pawnSelection;
    public EventAction EventAction => eventAction;
    public float TimeDuration => timeDuration;
    public Direction MoveDirection => moveDirection;
    public float MoveSpeed => moveSpeed;
    public float Delay => transitionTime;
    public float JumpForce => jumpForce;  
    public bool ActiveDialogueAtTime => activeDialogueAtTime;
    public bool WaitOnDialogue => waitOnDialogue;
    public Dialogue[] Dialogues => dialogues;
    public bool Invoke => invoke;
    public bool Activate => activate;
    public int Id => id;    
}

public enum PawnSelection
{
    Tinker,
    Ashe
}

public enum EventAction
{
    None,
    Move,
    Jump,
    Punch,
    Shoot,
    Grab
}

public enum Direction
{
    Right,
    Left,
}

#if UNITY_EDITOR
[CustomEditor(typeof(PawnEventData)), CanEditMultipleObjects]
public class PawnEventDataEditor : Editor
{
    SerializedProperty pawnEventArray;
    int currPawnEventIdx;
    private bool areYouSure = false;
    private Vector2 eventScroll;
    private void OnEnable()
    {
        pawnEventArray = serializedObject.FindProperty("pawnEvents");
    }

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.LabelField("Pawn Event Data");
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Events: " + pawnEventArray.arraySize, EditorStyles.boldLabel);
        // Interface Each PawnEvent by Title
        //EditorGUILayout.BeginHorizontal();
        //for (int i = 0;  i < pawnEventArray.arraySize; i++)
        //{
        //    EditorGUILayout.SelectableLabel(pawnEventArray.GetArrayElementAtIndex(i).FindPropertyRelative("pawnEventTitle").stringValue);
        //}
        //EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("+ New Pawn Event To End", GUILayout.MaxHeight(20f)))
        {
            currPawnEventIdx = pawnEventArray.arraySize++;
            SerializedProperty newElementProperty = pawnEventArray.GetArrayElementAtIndex(pawnEventArray.arraySize - 1);
            InitializePawnEvent(newElementProperty);
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("+ New Pawn Event at " + currPawnEventIdx, GUILayout.MaxHeight(20f)))
        {
            pawnEventArray.InsertArrayElementAtIndex(currPawnEventIdx);
            SerializedProperty newElementProperty = pawnEventArray.GetArrayElementAtIndex(currPawnEventIdx);
            InitializePawnEvent(newElementProperty);
        }
        GUILayout.FlexibleSpace();
        if (!areYouSure)
        {
            EditorGUI.BeginDisabledGroup(pawnEventArray.arraySize <= 0);
            areYouSure = GUILayout.Button("- Delete Pawn Event " + currPawnEventIdx, GUILayout.MaxHeight(20f));
            EditorGUI.EndDisabledGroup();
        }
        else 
        {
            EditorGUILayout.LabelField("Are you sure? ", GUILayout.MaxWidth(80f));
            if (GUILayout.Button("Yes", GUILayout.MaxHeight(20f), GUILayout.ExpandWidth(true)))
            {
                pawnEventArray.DeleteArrayElementAtIndex(currPawnEventIdx);
                currPawnEventIdx--;
                areYouSure = false;
            }
            else if (GUILayout.Button("No", GUILayout.MaxHeight(20f), GUILayout.ExpandWidth(true)))
            {
                areYouSure = false;
            }
        }
        
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        // Interfacing TOP
        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        EditorGUI.BeginDisabledGroup(currPawnEventIdx <= 0);
        if (GUILayout.Button("<<", EditorStyles.miniButtonLeft, GUILayout.MaxWidth(50f))) currPawnEventIdx--;
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(20);

        EditorGUILayout.IntField(currPawnEventIdx, GUILayout.MaxWidth(50f));

        GUILayout.Space(20);

        EditorGUI.BeginDisabledGroup(currPawnEventIdx >= pawnEventArray.arraySize - 1);
        if (GUILayout.Button(">>", EditorStyles.miniButtonRight, GUILayout.MaxWidth(50f))) currPawnEventIdx++;
        EditorGUI.EndDisabledGroup();
        // Bound the index
        currPawnEventIdx = pawnEventArray.arraySize > 0 
            ? (currPawnEventIdx % pawnEventArray.arraySize + pawnEventArray.arraySize) % pawnEventArray.arraySize : 0;

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        // (Ryan) From here is where you will add your custom implementation of Pawn Events
        if (pawnEventArray.arraySize > 0)
        {
            SerializedProperty selectedElement = pawnEventArray.GetArrayElementAtIndex(currPawnEventIdx);
            selectedElement.isExpanded = true;
            EditorGUILayout.PropertyField(selectedElement, true);
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Currently no Pawn Events, Add a new Pawn Event to Begin!", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
        }
        GUILayout.Space(20f);
        // Interfacing Bot
        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();
        EditorGUI.BeginDisabledGroup(currPawnEventIdx <= 0);
        if (GUILayout.Button("<<", EditorStyles.miniButtonLeft, GUILayout.MaxWidth(50f))) currPawnEventIdx--;
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(20);

        EditorGUILayout.IntField(currPawnEventIdx, GUILayout.MaxWidth(50f));

        GUILayout.Space(20);

        EditorGUI.BeginDisabledGroup(currPawnEventIdx >= pawnEventArray.arraySize - 1);
        if (GUILayout.Button(">>", EditorStyles.miniButtonRight, GUILayout.MaxWidth(50f))) currPawnEventIdx++;
        EditorGUI.EndDisabledGroup();
        // Bound the index
        currPawnEventIdx = pawnEventArray.arraySize > 0
            ? (currPawnEventIdx % pawnEventArray.arraySize + pawnEventArray.arraySize) % pawnEventArray.arraySize : 0;

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

    // So that we can just have our struct initializes with wanted defaults
    private void InitializePawnEvent(SerializedProperty pawnEventProperty)
    {
        // Level General
        pawnEventProperty.FindPropertyRelative("pawnEventTitle").stringValue = "New Event " + pawnEventArray.arraySize;
        pawnEventProperty.FindPropertyRelative("pausePawnControl").boolValue = false;
        pawnEventProperty.FindPropertyRelative("resumePawnControl").boolValue = false;
        // Pawn Specific
        pawnEventProperty.FindPropertyRelative("pawnSelection").enumValueIndex = 0;
        pawnEventProperty.FindPropertyRelative("eventAction").enumValueIndex = 0;
        // Movement Event
        pawnEventProperty.FindPropertyRelative("timeDuration").floatValue = 0;
        pawnEventProperty.FindPropertyRelative("moveDirection").enumValueIndex = 0;
        pawnEventProperty.FindPropertyRelative("moveSpeed").floatValue = 0.2f;
        pawnEventProperty.FindPropertyRelative("transitionTime").floatValue = 0;
        // Jump Event
        pawnEventProperty.FindPropertyRelative("jumpForce").floatValue = 0;
        // Dialogue
        pawnEventProperty.FindPropertyRelative("activeDialogueAtTime").boolValue = false;
        pawnEventProperty.FindPropertyRelative("waitOnDialogue").boolValue = false;
        pawnEventProperty.FindPropertyRelative("dialogues").arraySize = 0;
        // Invoke Event
        pawnEventProperty.FindPropertyRelative("invoke").boolValue = false;
        pawnEventProperty.FindPropertyRelative("activate").boolValue = false;
        pawnEventProperty.FindPropertyRelative("id").intValue = -1;
    }
}
#endif
