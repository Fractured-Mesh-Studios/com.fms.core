using CoreEngine.Occlusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Occludee))]
public class OccludeeEditor : Editor
{
    private Occludee m_target;

    private void OnEnable()
    {
        m_target = (Occludee)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (m_target.objects.Contains(m_target.gameObject))
        {
            EditorGUILayout.HelpBox("The object itself cannot be deactivated by the occlusion system since the events are executed by the aforementioned object.", MessageType.Error);
        }
    }
}
