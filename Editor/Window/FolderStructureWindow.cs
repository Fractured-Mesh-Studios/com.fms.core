using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using CoreEngine.Data;


namespace CoreEditor.Window
{
    public class FolderStructureWindow : CoreWindow
    {
        [MenuItem("[FMS] - Utility/Project Structure")]
        private static void ShowWindow()
        {
            GUIContent content = new GUIContent("Project Structure", "Customize Project Folder Structure Helper");

            if (s_window == null)
            {
                s_window = GetWindow<FolderStructureWindow>();
                s_window.titleContent = content;
                s_window.maxSize = new Vector2(300, 550);
                ScriptableObject target = s_window;
                s_windowObject = new SerializedObject(target);
                s_window.Show();
            }
            else
            {
                s_window.titleContent = content;
                s_window.maxSize = new Vector2(300, 550);
                ScriptableObject target = s_window;
                s_windowObject = new SerializedObject(target);

                if (s_window.docked)
                    s_window.ShowTab();
                else
                    s_window.Show();
            }
        }

        //member
        [SerializeField] private string[] m_module = new string[1] 
        {
            "Core" 
        };
        [SerializeField] private string[] m_subModule = new string[] { 
            "Animation",
            "Audio",
            "Code",
            "Materials",
            "Meshes",
            "Prefabs",
            "Scenes",
            "Shaders",
            "Textures",
            "UI"
        };

        private void OnGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.PropertyField(s_windowObject.FindProperty("m_module"));
            EditorGUILayout.PropertyField(s_windowObject.FindProperty("m_subModule"));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Create Module")) { CreateModules(); }
        }

        private void CreateModules()
        {
            foreach(var module in m_module)
            {
                if(!Directory.Exists("Assets/" + module))
                {
                    Directory.CreateDirectory("Assets/" + module);

                    CreateSubModules(module);
                }
            }

            s_window.Close();
        }

        private void CreateSubModules(string module)
        {
            foreach (var subModule in m_subModule)
            {
                if (!Directory.Exists("Assets/" + module + "/" + subModule))
                {
                    Directory.CreateDirectory("Assets/" + module + "/" + subModule);
                }
            }
        }
    }
}
