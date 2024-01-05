using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CoreEditor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class BaseEditor : Editor
    {
        private MonoBehaviour m_monoBehavior;
        protected string m_title;

        protected virtual void OnEnable()
        {
            m_monoBehavior = target as MonoBehaviour;
            m_title = m_monoBehavior.GetType().Name;
        }

        protected virtual void OnDisable() 
        {
            m_monoBehavior = null;
            m_title = string.Empty;
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);
            TitleBox(m_title);
            GUILayout.Space(10);

            DrawInspectorExcept(serializedObject, "m_Script");
        }

        protected void TitleBox(string title, int height = 5)
        {
            EditorGUILayout.BeginVertical("SelectionRect");
            GUILayout.Space(height);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(title, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(height);
            EditorGUILayout.EndVertical();
        }

        #region INTERNAL
        private void DrawInspectorExcept(SerializedObject serializedObject, string fieldToSkip)
        {
            DrawInspectorExcept(serializedObject, new string[1] { fieldToSkip });
        }

        private void DrawInspectorExcept(SerializedObject serializedObject, string[] fieldsToSkip)
        {
            serializedObject.Update();
            SerializedProperty prop = serializedObject.GetIterator();
            if (prop.NextVisible(true))
            {
                do
                {
                    if (fieldsToSkip.Any(prop.name.Contains))
                        continue;

                    EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
                }
                while (prop.NextVisible(false));
            }
            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
