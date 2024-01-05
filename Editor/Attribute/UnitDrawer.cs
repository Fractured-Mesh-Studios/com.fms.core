using Codice.Client.BaseCommands;
using Codice.Client.BaseCommands.BranchExplorer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

using CoreEngine;

namespace CoreEditor
{
    [CustomPropertyDrawer(typeof(Unit))]
    public class UnitDrawer : PropertyDrawer
    {
        private Unit m_unit;
        private Rect m_rect;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            m_unit = attribute as Unit;
            return base.CreatePropertyGUI(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);

            if(m_unit == null)
            {
                m_unit = attribute as Unit;
            }

            m_unit.style = GUI.skin.GetStyle("miniLabel");

            float y = position.y;
            float w = (8 * m_unit.name.Length);
            float h = position.height;
            float x = position.x + position.width - w;
            m_rect = new Rect(x, y, position.width, h);
            m_unit.style.normal.textColor = m_unit.color;
            EditorGUI.LabelField(m_rect, m_unit.name, m_unit.style);
        }
    }
}
