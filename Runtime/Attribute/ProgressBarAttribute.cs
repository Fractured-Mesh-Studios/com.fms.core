using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreEngine
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ProgressBar : PropertyAttribute
    {
        private float m_min, m_max;
        private bool m_label;

        public float min {  get { return m_min; } }
        public float max { get { return m_max; } }
        public bool label { get { return m_label; } }

        public ProgressBar(float min, float max, bool label = false)
        {
            m_min = min;
            m_max = max;
            m_label = label;
        }
    }
}
