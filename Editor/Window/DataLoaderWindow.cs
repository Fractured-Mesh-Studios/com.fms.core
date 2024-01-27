using CoreEngine.Data;
using PlasticPipe.PlasticProtocol.Messages;
using System;
using System.IO;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace CoreEditor.Window
{
    public class DataLoaderWindow : CoreWindow
    {
        [MenuItem("[FMS] - Utility/Data Loader")]
        private static void ShowWindow()
        {
            GUIContent content = EditorGUIUtility.IconContent("SaveActive");
            content.text = "Data Loader";
            content.tooltip = "Show properties of the local data persistent system";

            if (s_window == null)
            {
                s_window = GetWindow<DataLoaderWindow>();
                s_window.titleContent = content;
                ScriptableObject target = s_window;
                s_windowObject = new SerializedObject(target);
                s_window.Show();
            }
            else
            {
                s_window.titleContent = content;
                ScriptableObject target = s_window;
                s_windowObject = new SerializedObject(target);

                if (s_window.docked)
                    s_window.ShowTab();
                else
                    s_window.Show();
            }
        }

        private int m_keySizeFactor = 2;

        private void OnGUI()
        {
            //GENERAL
            GUIStyle style = EditorStyles.boldLabel;
            style.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.BeginVertical("Box");
            GUILayout.Label("General Settings", style);
            DataLoader.autoSave = EditorGUILayout.Toggle("Auto Save", DataLoader.autoSave);
            DataLoader.encryption = EditorGUILayout.Toggle("Encryption", DataLoader.encryption);
            DataLoader.key = EditorGUILayout.TextField("Key", DataLoader.key);

            int keySize = AesOperation.ALGORITHM_BASE * m_keySizeFactor;
            m_keySizeFactor = EditorGUILayout.IntSlider($"Key Size [{keySize}]", m_keySizeFactor, 2, 4);

            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Generate Key"))
            {
                var aes = Aes.Create();
                aes.KeySize = AesOperation.ALGORITHM_BASE * m_keySizeFactor;
                aes.GenerateKey();
                DataLoader.key = Convert.ToBase64String(aes.Key);
                aes.Dispose();
            }
            if (GUILayout.Button("Reset Key"))
            {
                DataLoader.key = AesOperation.KEY;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            //PATH
            EditorGUILayout.BeginVertical("Box");
            GUILayout.Label("Path Settings", style);
            DataLoader.pathType = (DataLoader.DataLoaderPath)EditorGUILayout.EnumPopup("Path Type",DataLoader.pathType);
            EditorGUILayout.BeginHorizontal();
            DataLoader.path = EditorGUILayout.TextField("Path", DataLoader.path);
            if (GUILayout.Button("[...]", GUILayout.MaxWidth(55)))
            {
                DataLoader.path = EditorUtility.OpenFolderPanel("Select Data Path", string.Empty, string.Empty);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("Current Path", DataLoader.currentPath);
            if(GUILayout.Button("[...]", GUILayout.MaxWidth(55)))
            {
                System.Diagnostics.Process.Start(DataLoader.currentPath);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}