using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace GameEditor
{
    [CustomPropertyDrawer(typeof(ValueGeneric), true)]
    public class ValueEditor : PropertyDrawer
    {
        private Value<object> Target;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement Element = new VisualElement();

            Element.name = "ASD";

            PropertyField a = new PropertyField(property.FindPropertyRelative("value"));

            Element.Add(a);

            return Element;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var v = property.FindPropertyRelative("_value");
            var a = property.serializedObject.targetObject;
            
            if (v != null)
            {
                Debug.Log(v.GetType());
            }



            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);
            if (property.isExpanded)
            {

            }
        }

        /*public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.
        }*/
    }
}
