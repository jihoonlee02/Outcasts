using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//#if UNITY_EDITOR
//using UnityEditor;
//using UnityEditorInternal;
//#endif

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] private Dialogue[] dialogue;    

    public Dialogue[] Dialogue => dialogue;
}

[System.Serializable]
public struct Dialogue
{ 
    [SerializeField] private Sprite profile;
    [SerializeField] private AudioClip typeSound;
    [SerializeField] [TextArea] private string text;

    public Sprite Profile => profile;
    public AudioClip TypeSound => typeSound;
    public string Text => text;
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

