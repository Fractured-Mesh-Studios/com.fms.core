using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreEngine.Procedural
{
    public class UnquieGenerator
    {
        protected long m_currentValue;
        protected readonly long m_startValue;
        protected HashSet<long> m_generatedValues;
        protected Queue<long> m_reusableValues;

        public UnquieGenerator(long startValue = int.MinValue)
        {
            m_startValue = startValue;
            m_currentValue = startValue;
            m_generatedValues = new HashSet<long>();
            m_reusableValues = new Queue<long>();
        }

        public long GenerateValue()
        {
            long valueToReturn;

            if (m_reusableValues.Count > 0)
            {
                // Reuse a value from the queue of reusable values
                valueToReturn = m_reusableValues.Dequeue();
            }
            else
            {
                // Generate a new value
                valueToReturn = m_currentValue++;
            }

            AddValue(valueToReturn);
            return valueToReturn;
        }

        public void AddValue(long value)
        {
            m_generatedValues.Add(value);
        }

        public bool RemoveValue(long value)
        {
            if (m_generatedValues.Contains(value))
            {
                m_generatedValues.Remove(value);
                m_reusableValues.Enqueue(value);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            m_currentValue = m_startValue;
            m_generatedValues.Clear();
            m_reusableValues.Clear();
        }

        public bool IsValueGenerated(long value)
        {
            return m_generatedValues.Contains(value);
        }
    }
}
