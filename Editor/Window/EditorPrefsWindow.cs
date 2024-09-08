using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CoreEditor
{
    public class EditorPrefsWindow : EditorWindow
    {
        private Vector2 scrollPos;
        private Dictionary<string, string> prefs = new Dictionary<string, string>();

        [MenuItem("Tools/EditorPrefs Inspector")]
        public static void ShowWindow()
        {
            GetWindow<EditorPrefsWindow>("EditorPrefs Inspector");
        }

        private void OnEnable()
        {
            // Populate the dictionary with EditorPrefs keys and values
            LoadEditorPrefs();
        }

        private void OnGUI()
        {
            GUILayout.Label("EditorPrefs Inspector", EditorStyles.boldLabel);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            foreach (var kvp in prefs)
            {
                EditorGUILayout.LabelField(kvp.Key, kvp.Value);
            }

            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Refresh"))
            {
                LoadEditorPrefs();
            }
        }

        private void LoadEditorPrefs()
        {
            // Clear existing dictionary
            prefs.Clear();

            // Define keys you want to check or consider a pattern
            var keys = new[]
            {
            "PUG_CurrentValue",
            "PUG_GeneratedValues",
            "PUG_ReusableValues",
            // Add more known keys here
        };

            foreach (var key in keys)
            {
                if (EditorPrefs.HasKey(key))
                {
                    // Store values in dictionary
                    prefs[key] = EditorPrefs.GetString(key);
                }
                else
                {
                    prefs[key] = "Key does not exist";
                }
            }
        }
    }

}
