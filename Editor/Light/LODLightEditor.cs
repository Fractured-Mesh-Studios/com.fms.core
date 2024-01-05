using CoreEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CoreEditor
{
    [CustomEditor(typeof(LODLight))]
    public class LODLightEditor : Editor
    {
        LODLight m_target;
        Camera m_camera;

        private void OnEnable()
        {
            m_target = (LODLight)target;
            m_camera = Camera.main;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        private void OnSceneGUI()
        {
            Handles.color = Color.white;
            Handles.DrawLine(m_target.transform.position, m_camera.transform.position);
        }
    }
}
