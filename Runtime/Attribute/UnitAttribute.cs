using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreEngine
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class Unit : PropertyAttribute
    {
        private string m_name;
        private GUIStyle m_labelStyle;
        private Color m_color;

        public string name { get { return m_name; } }
        public GUIStyle style { get { return m_labelStyle; } set { m_labelStyle = value; } }
        public Color color { get { return m_color; } }

        public Unit(string name)
        {
            m_name = name;
            m_color = Color.yellow;
            m_labelStyle = new GUIStyle();
        }
        public Unit(string name, Color color)
        {
            m_name = name;
            m_labelStyle = new GUIStyle();
            m_color = color;
        }
    }
}
