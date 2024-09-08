using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CoreEngine.Data;
using System.Linq;
using System.Reflection;
using CoreEditor.Procedural;

namespace CoreEditor.Data
{
    [CustomEditor(typeof(ObjectLoader))]
    public class ObjectLoaderEditor : Editor
    {
        private const int MIN_BTN_WIDTH = 100; //NOT USED
        private const int MAX_BTN_WIDTH = 200;

        private static PersistentUniqueGenerator s_generator = new PersistentUniqueGenerator();

        private static void OnEditorQuit()
        {
            s_generator.Save();
        }

        private ObjectLoader m_target;
        private bool m_isGeneratedId;
        private GUIContent m_content;

        private void OnEnable()
        {
            m_target = (ObjectLoader)target;
            EditorApplication.quitting += OnEditorQuit;

            s_generator.Load();
        }

        public override void OnInspectorGUI()
        {
            KeyInspector();
            DrawDefaultInspector();

            if(m_target.components.Where(x => x.GetType() == typeof(ObjectLoader)).Any())
            {
                EditorGUILayout.HelpBox("No Self Container Allowed", MessageType.Error);
            }

            GUILayout.Space(10);
            IdInspector();
            LoadSaveInspector();
            ConfigInspector();

            serializedObject.ApplyModifiedProperties();
        }

        private void KeyInspector()
        {
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            var keyField = m_target.GetType().GetField("m_key", flags);
            GUI.enabled = false;
            EditorGUILayout.TextField((string)keyField.GetValue(m_target));
            GUI.enabled = true;
        }

        private void IdInspector()
        {
            EditorGUILayout.BeginHorizontal("Box");
            GUI.enabled = !m_isGeneratedId;

            m_content = new GUIContent("Generate Id", $"Range (Min: <b>{int.MinValue}</b> Max: <b>{int.MaxValue}</b>)");

            if (GUILayout.Button(m_content, GUILayout.MaxWidth(MAX_BTN_WIDTH)))
            {
                m_target.id = s_generator.GenerateValue();
                m_isGeneratedId = true;
            }

            m_content = new GUIContent("Remove Id", $"Range (Min: <b>{int.MinValue}</b> Max: <b>{int.MaxValue}</b>)");

            GUILayout.FlexibleSpace();

            GUI.enabled = m_isGeneratedId;
            if (GUILayout.Button(m_content, GUILayout.MaxWidth(MAX_BTN_WIDTH)))
            {
                s_generator.RemoveValue(m_target.id);
                m_isGeneratedId = false;
                m_target.id = long.MinValue;
            }
            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
        }

        private void LoadSaveInspector()
        {
            EditorGUILayout.BeginHorizontal("Box");
            if (GUILayout.Button("Load", GUILayout.MaxWidth(MAX_BTN_WIDTH)))
            {
                m_target.Load();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Save", GUILayout.MaxWidth(MAX_BTN_WIDTH)))
            {
                m_target.Save();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ConfigInspector()
        {
            EditorGUILayout.BeginHorizontal("Box");
            if (GUILayout.Button("Path", GUILayout.MaxWidth(MAX_BTN_WIDTH)))
            {
                System.Diagnostics.Process.Start(Application.persistentDataPath);
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Setup", GUILayout.MaxWidth(MAX_BTN_WIDTH)))
            {
                var components = m_target.GetComponents<Component>();
                m_target.components = components.Where(x => x != m_target).ToList();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
