using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

using CoreEngine;

namespace CoreEditor
{
    [CustomPropertyDrawer(typeof(RequiredObject))]
    public class RequiredObjectDrawer : PropertyDrawer
    {
        private Color m_color;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return base.CreatePropertyGUI(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            m_color = GUI.backgroundColor;

            if (property.objectReferenceValue == null)
                GUI.backgroundColor = Color.red;
            else
                GUI.backgroundColor = m_color;

            EditorGUI.PropertyField(position, property, label);
            GUI.backgroundColor = m_color;
        }
    }
}