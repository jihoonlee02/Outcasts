using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LineRendererSmoother))]
public class LineRendererSmootherEditor : Editor
{
    private LineRendererSmoother Smoother;

    private SerializedProperty Line;
    private SerializedProperty InitialState;
    private SerializedProperty SmoothingSections;

    private GUIContent UpdateInitialStateGUIContent = new GUIContent("Set Initial State");
    private GUIContent SmoothButtonGUIContent = new GUIContent("Smooth Path");
    private GUIContent RestoreDefaultGUIContent = new GUIContent("Restore Default Path");

    //private bool ExpandCurves = false;
    private BezierCurve[] Curves;
    private BezierCurve[] EditorCurves;

    private void OnEnable() {
        Smoother = (LineRendererSmoother)target;

        if (Smoother.Line == null) {
            Smoother.Line = Smoother.GetComponent<LineRenderer>();
        }
        Line = serializedObject.FindProperty("Line");
        InitialState = serializedObject.FindProperty("InitialState");
        SmoothingSections = serializedObject.FindProperty("SmoothingSections");

        EnsureCurvesMatchLineRendererPositions();
    }

    public override void OnInspectorGUI() {
        if (Smoother == null) {
            return;
        }
        EnsureCurvesMatchLineRendererPositions();

        EditorGUILayout.PropertyField(Line);
        EditorGUILayout.PropertyField(InitialState);
        EditorGUILayout.PropertyField(SmoothingSections);

        if (GUILayout.Button(UpdateInitialStateGUIContent)) {
            Smoother.InitialState = new Vector3[Smoother.Line.positionCount];
            Smoother.Line.GetPositions(Smoother.InitialState);
        }

        EditorGUILayout.BeginHorizontal(); {
            GUI.enabled = Smoother.Line.positionCount >= 3;
            if (GUILayout.Button(SmoothButtonGUIContent)) {
                SmoothPath();
            }

            bool lineRendererPathAndInitialStateAreSame = Smoother.Line.positionCount == Smoother.InitialState.Length;

            if (lineRendererPathAndInitialStateAreSame) {
                Vector3[] positions = new Vector3[Smoother.Line.positionCount];
                Smoother.Line.GetPositions(positions);

                lineRendererPathAndInitialStateAreSame = positions.SequenceEqual(Smoother.InitialState);
            }

            GUI.enabled = !lineRendererPathAndInitialStateAreSame;
            if (GUILayout.Button(RestoreDefaultGUIContent)) {
                Smoother.Line.positionCount = Smoother.InitialState.Length;
                Smoother.Line.SetPositions(Smoother.InitialState);

                if (Curves.Length != (Smoother.Line.positionCount - 1) / 3) {
                    Curves = new BezierCurve[(Smoother.Line.positionCount - 1) / 3];
                    EditorCurves = new BezierCurve[(Smoother.Line.positionCount - 1) / 3];
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

    private void SmoothPath() {
        Smoother.Line.positionCount = Curves.Length * SmoothingSections.intValue;
        int index = 0;
        for (int i = 0; i < Curves.Length; i++) {
            Vector3[] segments = Curves[i].GetSegments(SmoothingSections.intValue);
            for (int j = 0; j < segments.Length; j++) {
                Smoother.Line.SetPosition(index, segments[j]);
                index++;
            }
        }

        SmoothingSections.intValue = 1;
        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI() {
        if (Smoother.Line.positionCount <= 3) {
            return;
        }
        EnsureCurvesMatchLineRendererPositions();
        
        for (int i = 0; i < Curves.Length; i++) {
            if (Smoother.Line.useWorldSpace) {
                for (int j = 0; j < 4; j++) {
                    Curves[i].Points[j] = Smoother.Line.GetPosition((i*3) + j);
                    EditorCurves[i].Points[j] = Smoother.Line.GetPosition((i*3) + j);
                }
            } else {
                for (int j = 0; j < 4; j++) {
                    Curves[i].Points[j] = Smoother.Line.GetPosition((i*3) + j);
                    EditorCurves[i].Points[j] = Smoother.Line.GetPosition((i*3) + j) + Smoother.gameObject.transform.parent.parent.position;
                }
            }
        }

        //Apply look ahead
        // {
        //     Vector3 nextDirection = (Curves[1].EndPosition - Curves[1].StartPosition).normalized;
        //     Vector3 lastDirection = (Curves[0].EndPosition - Curves[0].StartPosition).normalized;

        //     Curves[0].Points[2] = Curves[0].Points[3] + (nextDirection + lastDirection) * -1 * SmoothingLength.floatValue;
        // }

        DrawSegments();
    }

    private void DrawSegments() {
        for (int i = 0; i < Curves.Length; i++) {
            Vector3[] segments = EditorCurves[i].GetSegments(SmoothingSections.intValue);
            for (int j = 0; j <segments.Length - 1; j++) {
                Handles.color = Color.white;
                Handles.DrawLine(segments[j], segments[j + 1]);

                float color = (float)j / segments.Length;
                Handles.color = new Color(color, color, color);
                Handles.Label(segments[j], $"C{i} S{j}");
                Handles.DotHandleCap(EditorGUIUtility.GetControlID(FocusType.Passive), segments[j], Quaternion.identity, 0.05f, EventType.Repaint);
            }

            Handles.color = Color.white;
            Handles.Label(segments[segments.Length - 1], $"C{i} S{segments.Length - 1}");
            Handles.DotHandleCap(EditorGUIUtility.GetControlID(FocusType.Passive), segments[segments.Length - 1], Quaternion.identity, 0.05f, EventType.Repaint);

            Handles.DrawLine(segments[segments.Length - 1], EditorCurves[i].EndPosition);
        }
    }

    private void EnsureCurvesMatchLineRendererPositions() {
        if (Curves == null || Curves.Length != (Smoother.Line.positionCount - 1) / 3) {
            Curves = new BezierCurve[(Smoother.Line.positionCount - 1) / 3];
            EditorCurves = new BezierCurve[(Smoother.Line.positionCount - 1) / 3];
            for (int i = 0; i < Curves.Length; i++) {
                Curves[i] = new BezierCurve();
                EditorCurves[i] = new BezierCurve();
            }
        }
    }
}
