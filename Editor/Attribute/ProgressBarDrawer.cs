using CoreEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

using ProgressBar = CoreEngine.ProgressBar;

namespace CoreEditor
{
    [CustomPropertyDrawer(typeof(ProgressBar))]
    public class ProgressBarDrawer : PropertyDrawer
    {
        private ProgressBar m_progress;
        private Rect m_rect, m_labelRect;

        private const int POSITON_FIX = 50;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            m_progress = attribute as ProgressBar;
            return base.CreatePropertyGUI(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (m_progress == null)
            {
                m_progress = attribute as ProgressBar;
            }

            float x = position.x;
            float y = position.y;
            float width = position.width - POSITON_FIX;
            float height = position.height;

            if (!m_progress.label)
            {
                EditorGUI.Slider(position, property, m_progress.min, m_progress.max, string.Empty);
                m_rect = new Rect(x - 5, y, width + 5, height);
                EditorGUI.DrawRect(m_rect, EditorExtended.backgroundColor);
                m_rect = new Rect(x, y, width, height);
                string text = $"{property.displayName} [{label.text}/{m_progress.max}]";
                EditorGUI.ProgressBar(m_rect, property.floatValue / m_progress.max, text);
            }
            else
            {
                float widthLabel = position.x + property.displayName.Length * 8;
                m_rect = new Rect(x + widthLabel, y, width, height);
                m_labelRect = new Rect(x, y, width, height);

                EditorGUI.Slider(m_rect, property, m_progress.min, m_progress.max, string.Empty);
                m_rect = new Rect(x + widthLabel - 5, y, width - widthLabel, height);
                EditorGUI.DrawRect(m_rect, EditorExtended.backgroundColor);
                m_rect = new Rect(x + widthLabel, y, width - widthLabel, height);
                EditorGUI.ProgressBar(m_rect, property.floatValue / m_progress.max, label.text);
                EditorGUI.LabelField(m_labelRect, property.displayName);
            }

        }
    }
}
