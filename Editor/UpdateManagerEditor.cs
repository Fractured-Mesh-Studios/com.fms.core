using CoreEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System;

namespace CoreEditor
{
    [CustomEditor(typeof(UpdateManager))]
    public class UpdateManagerEditor : Editor
    {
        private UpdateManager m_target;

        private BindingFlags m_flags = BindingFlags.NonPublic | BindingFlags.Static;

        private void OnEnable()
        {
            m_target = (UpdateManager)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Type type = target.GetType();

            var updateField = type.GetField("s_update", m_flags);
            var fixedUpdateField = type.GetField("s_fixedUpdate", m_flags);
            var lateUpdateField = type.GetField("s_lateUpdate", m_flags);

            List<Action> updateList = updateField.GetValue(null) as List<Action>;
            List<Action> fixedUpdateList = fixedUpdateField.GetValue(null) as List<Action>;
            List<Action> lateUpdateList = lateUpdateField.GetValue(null) as List<Action>;

            foreach ( var update in updateList ) { GUILayout.Label(update.GetType().Name); }
        }
    }
}
