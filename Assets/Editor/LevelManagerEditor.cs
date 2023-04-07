// using UnityEditor;
// using UnityEditorInternal;
// using UnityEngine;

// [CustomEditor(typeof(LevelManager))]
// public class LevelManagerEditor : Editor {
//     SerializedProperty totalAirVentPower;
//     ReorderableList list;
//     private void OnEnable() {
//         totalAirVentPower = serializedObject.FindProperty("totalAirVentPower");
//         list = new ReorderableList(serializedObject, totalAirVentPower, true, true, true, true);

//         list.drawElementCallback = DrawListItems;
//         list.drawHeaderCallback = DrawHeader;
//     }

//     void DrawListItems(Rect rect, int index, bool isActive, bool isFocused) {
//         SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

//         EditorGUI.PropertyField(
//             new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight),
//             element.FindPropertyRelative("airVentGroup"),
//             GUIContent.none);

//         EditorGUI.PropertyField(
//             new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight),
//             element.FindPropertyRelative("airVentPower"),
//             GUIContent.none);
//     }

//     void DrawHeader(Rect rect) {
//         string name = "Total Air Vent Power";
//         EditorGUI.LabelField(rect, name);
//     }

//     public override void OnInspectorGUI() {
//         base.OnInspectorGUI();
//         serializedObject.Update();
//         list.DoLayoutList();
//         serializedObject.ApplyModifiedProperties();
//     }
// }