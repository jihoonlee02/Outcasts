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
    [SerializeField] private PawnSelection pawnSelection;
    [SerializeField] private EventAction eventAction;
    [Header("Movement Event")]
    [SerializeField] private float timeDuration;
    [SerializeField] private Direction moveDirection;
    [SerializeField, Range(0.2f, 1f)] private float moveSpeed;
    [SerializeField] private float delay;
    [Header("Jump Event")]
    [SerializeField] private float jumpForce;
    [SerializeField] private Dialogue dialogue;
    public PawnSelection PawnSelection => pawnSelection;
    public EventAction EventAction => eventAction;
    public float TimeDuration => timeDuration;
    public Direction MoveDirection => moveDirection;
    public float MoveSpeed => moveSpeed;
    public float Delay => delay;
    public float JumpForce => jumpForce;    
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
    //bool pawnEventFoldOut;
    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();
    //    PawnEventData pawnEventData = (PawnEventData)target;
    //    EditorGUILayout.Space();
    //    pawnEventFoldOut = EditorGUILayout.Foldout(pawnEventFoldOut, "PawnEvents");
    //    foreach (PawnEvent p in pawnEventData.PawnEvents)
    //    {
    //        EditorGUILayout.BeginHorizontal();
    //        switch (p.EventAction)
    //        {
    //            case EventAction.Move:
    //                EditorGUILayout.LabelField("Moving");
    //                break;
    //            case EventAction.Jump:
    //                EditorGUILayout.LabelField("Jumping");
    //                break;
    //            case EventAction.Punch:
    //                EditorGUILayout.LabelField("Punching");
    //                break;
    //            case EventAction.Shoot:
    //                EditorGUILayout.LabelField("Shooting");
    //                break;
    //            case EventAction.Grab:
    //                EditorGUILayout.LabelField("Grabbing");
    //                break;
    //        }
    //        EditorGUILayout.EndHorizontal();
    //        EditorGUILayout.Space();
    //    }
    //}

    //public override VisualElement CreateInspectorGUI()
    //{
    //    // Create a new VisualElement to be the root of out inspector UI
    //    VisualElement myInspector = new VisualElement();

    //    // Add a simple label
    //    myInspector.Add(new Label("PawnEvents Internal Title"));

    //    // Load and clone a visual tree from UXML
    //    VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/VisualUITrees/PawnEventsCustomInspector.uxml");
    //    visualTree.CloneTree(myInspector);

    //    return myInspector;
    //}
}
#endif
