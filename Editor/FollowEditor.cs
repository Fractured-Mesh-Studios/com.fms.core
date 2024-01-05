using CoreEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CoreEditor
{
    [CustomEditor(typeof(Follow))]
    public class FollowEditor : Editor
    {
        private Follow m_target;

        private void OnEnable()
        {
            m_target = (Follow)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal("Box");
            GUILayout.FlexibleSpace();
            GUILayout.Label(m_target.target ? "[Following]" : "[Disabled]");
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            DrawDefaultInspector();

            if (!m_target.target)
            {
                EditorGUILayout.HelpBox("Target to follow is <null>", MessageType.Warning);
            }

        }
    }
}
