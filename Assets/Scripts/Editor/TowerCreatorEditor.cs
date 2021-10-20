using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

[CustomEditor(typeof(TowerCreator))]
public class TowerCreatorEditor : Editor
{
    SerializedProperty m_BuildableArea;

    int m_Width;
    int m_Height;

    bool m_MouseDown = false;
    bool m_SetBuildable = true;

    void OnEnable()
    {
        m_BuildableArea = serializedObject.FindProperty("buildableArea");

        m_Width = m_BuildableArea.arraySize;

        if (m_Width == 0)
        {
            m_BuildableArea.InsertArrayElementAtIndex(0);
            m_Width++;
        }

        m_Height = m_BuildableArea.GetArrayElementAtIndex(0).FindPropertyRelative("Array").arraySize;

        if (m_Height == 0)
        {
            m_BuildableArea.GetArrayElementAtIndex(0).FindPropertyRelative("Array").InsertArrayElementAtIndex(0);
            m_Height++;
        }

        SceneView.duringSceneGui += DuringSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        int _NewWidth = EditorGUILayout.IntField("Buildable Area Width", m_Width);
        int _NewHeight = EditorGUILayout.IntField("Buildable Area Height", m_Height);

        if (_NewWidth < 1)
        {
            _NewWidth = 1;
        }

        if (_NewHeight < 1)
        {
            _NewHeight = 1;
        }

        while (_NewWidth > m_Width)
        {
            m_BuildableArea.InsertArrayElementAtIndex(m_Width - 1);
            m_Width++;
        }

        while (_NewWidth < m_Width)
        {
            m_BuildableArea.DeleteArrayElementAtIndex(m_Width - 1);
            m_Width--;
        }

        while (_NewHeight > m_Height)
        {
            for (int i = 0; i < m_Width; i++)
            {
                m_BuildableArea.GetArrayElementAtIndex(i).FindPropertyRelative("Array").InsertArrayElementAtIndex(m_Height - 1);
            }

            m_Height++;
        }

        while (_NewHeight < m_Height)
        {
            for (int i = 0; i < m_Width; i++)
            {
                m_BuildableArea.GetArrayElementAtIndex(i).FindPropertyRelative("Array").DeleteArrayElementAtIndex(m_Height - 1);
            }

            m_Height--;
        }

        serializedObject.ApplyModifiedProperties();
    }

    void DuringSceneGUI(SceneView a_SceneView)
    {
        Event _Current = Event.current;

        Plane _Plane = new Plane(new Vector3(0, 0, 1), Vector3.zero);

        float _HitDistance;

        Ray _MouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        _Plane.Raycast(_MouseRay, out _HitDistance);

        Vector3 _WorldMousePos = _MouseRay.GetPoint(_HitDistance);

        int _XIndex = Mathf.FloorToInt(_WorldMousePos.x + m_Width / 2.0f);
        int _YIndex = Mathf.FloorToInt(_WorldMousePos.y + m_Height / 2.0f);

        if (Mathf.Abs(_WorldMousePos.x) < m_Width / 2.0f &&
            Mathf.Abs(_WorldMousePos.y) < m_Height / 2.0f)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            if (_Current.type == EventType.MouseDown)
            {
                m_MouseDown = true;

                m_SetBuildable = !m_BuildableArea.GetArrayElementAtIndex(_XIndex).FindPropertyRelative("Array").GetArrayElementAtIndex(_YIndex).boolValue;

                m_BuildableArea.GetArrayElementAtIndex(_XIndex).FindPropertyRelative("Array").GetArrayElementAtIndex(_YIndex).boolValue = m_SetBuildable;

                _Current.Use();
            }

            if (_Current.type == EventType.MouseDrag &&
                m_MouseDown)
            {
                _Current.Use();

                m_BuildableArea.GetArrayElementAtIndex(_XIndex).FindPropertyRelative("Array").GetArrayElementAtIndex(_YIndex).boolValue = m_SetBuildable;
            }

            if (_Current.type == EventType.MouseUp &&
                m_MouseDown)
            {
                m_MouseDown = false;
                _Current.Use();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
