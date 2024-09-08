using UnityEditor;
using CoreEngine.Procedural;
using System.Linq;

namespace CoreEditor.Procedural
{
    public class PersistentUniqueGenerator : UnquieGenerator
    {
        private const string CurrentValueKey = "PUG_CurrentValue";
        private const string GeneratedValuesKey = "PUG_GeneratedValues";
        private const string ReusableValuesKey = "PUG_ReusableValues";

        public PersistentUniqueGenerator() : base(int.MinValue)
        {
            m_currentValue = int.MinValue;
        }

        public void Save()
        {
            // Save the current value (using an int for demonstration, ensure it fits within int range)
            EditorPrefs.SetInt(CurrentValueKey, (int)m_currentValue);

            // Save the generated values
            var generatedValuesArray = m_generatedValues.Select(v => v.ToString()).ToArray();
            EditorPrefs.SetString(GeneratedValuesKey, string.Join(",", generatedValuesArray));

            // Save the reusable values
            var reusableValuesArray = m_reusableValues.Select(v => v.ToString()).ToArray();
            EditorPrefs.SetString(ReusableValuesKey, string.Join(",", reusableValuesArray));
        }

        public void Load()
        {
            // Load the current value
            if (EditorPrefs.HasKey(CurrentValueKey))
            {
                m_currentValue = EditorPrefs.GetInt(CurrentValueKey);
            }

            // Load the generated values
            if (EditorPrefs.HasKey(GeneratedValuesKey))
            {
                var generatedValuesString = EditorPrefs.GetString(GeneratedValuesKey);
                var generatedValuesArray = generatedValuesString.Split(',');
                foreach (var valueString in generatedValuesArray)
                {
                    if (long.TryParse(valueString, out var value))
                    {
                        m_generatedValues.Add(value);
                    }
                }
            }

            // Load the reusable values
            if (EditorPrefs.HasKey(ReusableValuesKey))
            {
                var reusableValuesString = EditorPrefs.GetString(ReusableValuesKey);
                var reusableValuesArray = reusableValuesString.Split(',');
                foreach (var valueString in reusableValuesArray)
                {
                    if (long.TryParse(valueString, out var value))
                    {
                        m_reusableValues.Enqueue(value);
                    }
                }
            }
        }

        public void ClearSavedData()
        {
            // Clear saved data from EditorPrefs
            EditorPrefs.DeleteKey(CurrentValueKey);
            EditorPrefs.DeleteKey(GeneratedValuesKey);
            EditorPrefs.DeleteKey(ReusableValuesKey);
        }
    }
}
