using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using Transform = GameEngine.Translation.Transform;

namespace GameEditor
{
    [CustomEditor(typeof(Transform))]
    public class TransformEditor : Editor
    {
        Transform Target;
        Vector3 Rotation;

        private void OnEnable()
        {
            Target = Target as Transform;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Position"));
            Rotation = EditorGUILayout.Vector3Field("Rotation", Target.Rotation.eulerAngles);
            Target.Rotation = Quaternion.Euler(Rotation);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Scale"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}