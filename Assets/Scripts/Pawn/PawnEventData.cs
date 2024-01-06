using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

[CreateAssetMenu(menuName = "Data/PawnEventData")]
// Restricted to tinker n Ashe only
public class PawnEventData : ScriptableObject
{
    [SerializeField] private PawnEvent[] pawnEvents;
    [SerializeField] private bool hideDialogueAfterEvent;
    [SerializeField] private bool skippable;
    public PawnEvent[] PawnEvents => pawnEvents;
    public bool HideDialogueAfterEvent => hideDialogueAfterEvent;
    public bool Skippable => skippable;
}

[System.Serializable]
public struct PawnEvent
{
    // TODO: Be able to adjust both Tinker and Actions Actions in the Same Event
    // Include Dialogue to be ran in Dialogue Manager!
    [Header("Level General")] // More Efficent in a new editor
    [SerializeField] private string pawnEventTitle;
    [SerializeField] private bool notCinematic;
    [SerializeField] private bool pausePawnControl;
    [SerializeField] private bool resumePawnControl;
    [SerializeField] private float transitionTime;

    [Header("Pawn Specific")]
    [SerializeField] private PawnSelection pawnSelection;
    [SerializeField] private EventAction eventAction;
    [Header("Movement Event")]
    [SerializeField] private Direction moveDirection;
    [SerializeField, Range(0.2f, 1f)] private float moveSpeed;
    [SerializeField] private float timeDuration;

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

    // Properties
    public string Title => pawnEventTitle;
    public bool NotCinematic => notCinematic;
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
    Grab,
    MoveTo
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
    //private List<string> m_eventNames;
    private void OnEnable()
    {
        pawnEventArray = serializedObject.FindProperty("pawnEvents");
        //m_eventNames = new List<string>();
    }

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.LabelField("Pawn Event Data");
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Events: " + pawnEventArray.arraySize, EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        var skippableButton = serializedObject.FindProperty("skippable");
        skippableButton.boolValue = EditorGUILayout.Toggle("Skippable", skippableButton.boolValue);
        // Interface Each PawnEvent by Title
        EditorGUILayout.Space();
        EventTitleInterfacer();
        EditorGUILayout.Space();
        PawnEventAdjustment();
        EditorGUILayout.Space();

        // Interfacing TOP
        HorizontalPagingInterfacer();
        EditorGUILayout.Separator();

        // (Ryan) From here is where you will add your custom implementation of Pawn Events
        if (pawnEventArray.arraySize > 0)
        {
            SerializedProperty selectedElement = pawnEventArray.GetArrayElementAtIndex(currPawnEventIdx);
            //selectedElement.isExpanded = true;
            //EditorGUILayout.PropertyField(selectedElement, true);
            LevelGeneralSpace(selectedElement);
            EditorGUILayout.Space(20f);
            PawnControlSpace(selectedElement);
            EditorGUILayout.Space(20f);
            DialogueSpace(selectedElement);
            EditorGUILayout.Space(20f);
            InvokeGameEventSpace(selectedElement);
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Currently no Pawn Events, Add a new Pawn Event to Begin!", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
        }
        // Interfacing Bot
        EditorGUILayout.Separator();
        HorizontalPagingInterfacer();

        EditorGUILayout.Space();

        PawnEventAdjustment();
        EditorGUILayout.Space();

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

    private void PawnEventAdjustment()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("+ New Pawn Event To End", GUILayout.MaxHeight(20f), GUILayout.ExpandWidth(true)))
        {
            currPawnEventIdx = pawnEventArray.arraySize++;
            SerializedProperty newElementProperty = pawnEventArray.GetArrayElementAtIndex(pawnEventArray.arraySize - 1);
            InitializePawnEvent(newElementProperty);
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("+ New Pawn Event at " + currPawnEventIdx, GUILayout.MaxHeight(20f), GUILayout.ExpandWidth(true)))
        {
            pawnEventArray.InsertArrayElementAtIndex(currPawnEventIdx);
            SerializedProperty newElementProperty = pawnEventArray.GetArrayElementAtIndex(currPawnEventIdx);
            InitializePawnEvent(newElementProperty);
        }
        GUILayout.FlexibleSpace();
        if (!areYouSure)
        {
            EditorGUI.BeginDisabledGroup(pawnEventArray.arraySize <= 0);
            areYouSure = GUILayout.Button("- Delete Pawn Event " + currPawnEventIdx, GUILayout.MaxHeight(20f), GUILayout.ExpandWidth(true));
            EditorGUI.EndDisabledGroup();
        }
        else
        {
            EditorGUILayout.LabelField("Are you sure? ", GUILayout.MaxWidth(80f), GUILayout.ExpandWidth(true));
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
    }

    private void HorizontalPagingInterfacer()
    {
        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();
        EditorGUI.BeginDisabledGroup(currPawnEventIdx <= 0);
        if (GUILayout.Button("<<<=", EditorStyles.miniButtonRight, GUILayout.MaxWidth(50f))) currPawnEventIdx = 0;
        if (GUILayout.Button("<<", EditorStyles.miniButtonLeft, GUILayout.MaxWidth(50f))) currPawnEventIdx--;
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(20);

        //EditorGUILayout.IntPopup(currPawnEventIdx, pawnEventArray.);
        EditorGUILayout.IntField(currPawnEventIdx, GUILayout.MaxWidth(50f));

        GUILayout.Space(20);

        EditorGUI.BeginDisabledGroup(currPawnEventIdx >= pawnEventArray.arraySize - 1);
        if (GUILayout.Button(">>", EditorStyles.miniButtonRight, GUILayout.MaxWidth(50f))) currPawnEventIdx++;
        if (GUILayout.Button("=>>>", EditorStyles.miniButtonRight, GUILayout.MaxWidth(50f))) currPawnEventIdx = pawnEventArray.arraySize - 1;
        EditorGUI.EndDisabledGroup();
        // Bound the index
        currPawnEventIdx = pawnEventArray.arraySize > 0
            ? (currPawnEventIdx % pawnEventArray.arraySize + pawnEventArray.arraySize) % pawnEventArray.arraySize : 0;

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();
    }

    private void EventTitleInterfacer()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Title Interfacer Coming Soon");
        //for (int i = 0; i < pawnEventArray.arraySize; i++)
        //{
        //    EditorGUILayout.SelectableLabel(pawnEventArray.GetArrayElementAtIndex(i).FindPropertyRelative("pawnEventTitle").stringValue);
        //}
        EditorGUILayout.EndHorizontal();
    }

    // Details of the Pawn Event Data
    private void LevelGeneralSpace(SerializedProperty pawnEvent)
    {
        EditorGUILayout.LabelField("Level General", EditorStyles.boldLabel);
        var PawnEventTitle = pawnEvent.FindPropertyRelative("pawnEventTitle");
        var TransitionTime = pawnEvent.FindPropertyRelative("transitionTime");
        PawnEventTitle.stringValue = EditorGUILayout.TextField("Pawn Event Title", PawnEventTitle.stringValue);
        TransitionTime.floatValue = EditorGUILayout.FloatField("Transition Time", TransitionTime.floatValue);
        var PausePC = pawnEvent.FindPropertyRelative("pausePawnControl");
        var ResumePC = pawnEvent.FindPropertyRelative("resumePawnControl");
        var NotCinem = pawnEvent.FindPropertyRelative("notCinematic");

        EditorGUI.BeginChangeCheck();
        bool pause = EditorGUILayout.Toggle("Pause Pawn Control", PausePC.boolValue);
        bool resume = EditorGUILayout.Toggle("Resume Pawn Control", ResumePC.boolValue);
        if (EditorGUI.EndChangeCheck())
        {
            if (pause && !PausePC.boolValue)
            {
                PausePC.boolValue = true;
                ResumePC.boolValue = false;
            }
            else if (resume && !ResumePC.boolValue)
            {
                ResumePC.boolValue = true;
                PausePC.boolValue = false;
            }
            else
            {
                ResumePC.boolValue = false;
                PausePC.boolValue = false;
            }
        }
        if (PausePC.boolValue)
        {
            NotCinem.boolValue = EditorGUILayout.Toggle("Not Cinematic", NotCinem.boolValue);
        }
    }

    private void PawnControlSpace(SerializedProperty pawnEvent)
    {
        EditorGUILayout.LabelField("Pawn Control", EditorStyles.boldLabel);
        // Display a dropdown for an enum Pawn
        var pawnSelection = pawnEvent.FindPropertyRelative("pawnSelection");
        var EventAction = pawnEvent.FindPropertyRelative("eventAction");
        pawnSelection.enumValueIndex = EditorGUILayout.Popup("Pawn", pawnSelection.enumValueIndex, pawnSelection.enumDisplayNames);
        EventAction.enumValueIndex = EditorGUILayout.Popup("Action", EventAction.enumValueIndex, EventAction.enumDisplayNames);

        // Select The Action to display
        EditorGUI.indentLevel++;
        switch ((EventAction)EventAction.enumValueIndex) 
        {
            case global::EventAction.Move:
                MovementActionSpace(pawnEvent);
                break;
            case global::EventAction.Jump:
                JumpActionSpace(pawnEvent);
                break;
            default: 
                break;
        }
        EditorGUI.indentLevel--;
    }
    private void DialogueSpace(SerializedProperty pawnEvent)
    {
        EditorGUILayout.LabelField("Dialogue", EditorStyles.boldLabel);
        var DialougeActive = pawnEvent.FindPropertyRelative("activeDialogueAtTime");   
        DialougeActive.boolValue = EditorGUILayout.Toggle("Dialogue Active", DialougeActive.boolValue);

        if (DialougeActive.boolValue)
        {
            var WaitOnDialogue = pawnEvent.FindPropertyRelative("waitOnDialogue");
            WaitOnDialogue.boolValue = EditorGUILayout.Toggle("Wait On Dialogue", WaitOnDialogue.boolValue);
            EditorGUILayout.PropertyField(pawnEvent.FindPropertyRelative("dialogues"), true);
        }
    }

    private void InvokeGameEventSpace(SerializedProperty pawnEvent)
    {
        EditorGUILayout.LabelField("Invoke Game Event", EditorStyles.boldLabel);
        if (EditorGUILayout.IntField("ID", pawnEvent.FindPropertyRelative("id").intValue) >= 0)
        {
            var Invoke = pawnEvent.FindPropertyRelative("invoke");
            var Activate = pawnEvent.FindPropertyRelative("activate");
            Invoke.boolValue = EditorGUILayout.Toggle("Invoke", Invoke.boolValue);
            Activate.boolValue = EditorGUILayout.Toggle("Activate", Activate.boolValue);
        }
        
    }

    #region Pawn Actions
    private void MovementActionSpace(SerializedProperty pawnEvent)
    {
        //EditorGUILayout.LabelField("Movement Action", EditorStyles.boldLabel);
        var Direction = pawnEvent.FindPropertyRelative("moveDirection");
        Direction.enumValueIndex = EditorGUILayout.Popup("Direction", Direction.enumValueIndex, Direction.enumDisplayNames);
        var Speed = pawnEvent.FindPropertyRelative("moveSpeed");
        Speed.floatValue = EditorGUILayout.Slider("Speed", Speed.floatValue, 0.2f, 1f);
    }
    private void JumpActionSpace(SerializedProperty pawnEvent)
    {
        //EditorGUILayout.LabelField("Jump Action", EditorStyles.boldLabel);
        var JumpForce = pawnEvent.FindPropertyRelative("jumpForce");
        JumpForce.floatValue = EditorGUILayout.FloatField("Jump Force", JumpForce.floatValue);
    }
    #endregion
}
#endif
