using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Newtonsoft
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoreEngine.Data
{
    public class ObjectLoader : MonoBehaviour
    {
        public enum ObjectLoaderFileName
        {
            UseId,
            UseName,
            UseTypeName,
            UseCustom,
        }

        private class SerializedData : Dictionary<string, Dictionary<string, object>> { }

        private const string m_key = "ZzP5rMHiMkWzGzh8fHP9JQ==";

        public bool encryption = false;
        public long id;
        public string folder = "Save";
        [HideInInspector] public string fileName = string.Empty;

        public ObjectLoaderFileName fileNameMode = ObjectLoaderFileName.UseId;

        public List<Component> components = new List<Component>();

        private FileDataHandler m_file = null;
        private SerializedData m_data = new SerializedData();
        

        public void Load()
        {
            Initialize();

            OnLoad();
        }

        public void Save()
        {
            Initialize();

            OnSave();
        }

        #region PRIVATE
        private void Initialize()
        {
            string path = Application.persistentDataPath;

            string folder = $"/{this.folder}";

            string name;
            switch (fileNameMode)
            {
                case ObjectLoaderFileName.UseName: name = $"{gameObject.name}.data"; break; 
                case ObjectLoaderFileName.UseTypeName: name = $"{GetType().Name}.data"; break;
                case ObjectLoaderFileName.UseCustom: name = $"{fileName}.data"; break;
                default: name = $"{id}.data"; break;
            }
            
            m_file = new FileDataHandler(path + folder, name, m_key);
            m_data = new SerializedData();
        }

        private void OnLoad()
        {
            m_data = m_file.Load<SerializedData>();

            foreach (var Key in m_data.Keys)
            {
                for (int i = 0; i < components.Count; i++)
                {
                    var element = components[i];
                    var type = components[i].GetType();

                    if (type.Name == Key)
                    {
                        //Fields
                        var field = GetFieldInfo(type);
                        for (int f = 0; f < field.Length; f++)
                        {
                            var name = field[f].Name;
                            var ftype = field[f].FieldType;

                            if (m_data[type.Name].ContainsKey(name))
                            {
                                var value = m_data[type.Name][name];
                                ConvertTo(ftype, ref value);
                                field[f].SetValue(element, value);
                            }
                            else
                            {
                                Debug.Log($"Ignored Key: {name}".Color(Color.yellow), gameObject);
                            }
                        }

                        //Properties
                        var property = GetPropertyInfo(type);
                        for (int p = 0; p < property.Length; p++)
                        {
                            var pType = property[p].PropertyType;
                            var canWrite = property[p].CanWrite;
                            var canRead = property[p].CanRead;

                            if (pType.IsSerializable && canWrite && canRead)
                            {
                                var name = property[p].Name;
                                var value = m_data[type.Name][name];

                                ConvertTo(ref value);

                                property[p].SetValue(element, value, null);
                            }

                        }

                        LoadTransform(type, transform);
                    }
                }
            }
        }

        private void OnSave()
        {
            m_data.Clear();

            for (int i = 0; i < components.Count; i++)
            {
                var element = components[i];
                var type = element.GetType();

                if (!m_data.ContainsKey(type.Name))
                    m_data.Add(type.Name, new Dictionary<string, object>());
                else
                {
                    Debug.LogWarning("Repited Component!");
                    continue;
                }

                var field = GetFieldInfo(type);
                for (int f = 0; f < field.Length; f++)
                {
                    var name = field[f].Name;
                    var value = field[f].GetValue(element);

                    if (!IsBackingField(field[f]))
                    {
                        m_data[type.Name].Add(name, value);
                    }
                }

                var property = GetPropertyInfo(type);
                for (int p = 0; p < property.Length; p++)
                {
                    var pType = property[p].PropertyType;
                    var canWrite = property[p].CanWrite;
                    var canRead = property[p].CanRead;


                    if (pType.IsSerializable && canWrite && canRead)
                    {
                        var name = property[p].Name;
                        var value = property[p].GetValue(element);

                        m_data[type.Name].Add(name, value);
                    }

                    SaveTransform(type, pType, transform);
                }

            }

            m_file.Save(m_data, encryption);
        }
        #endregion

        #region REFLECTION
        private FieldInfo[] GetFieldInfo(Type type)
        {
            return type.GetFields(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.FlattenHierarchy
            );
        }

        private PropertyInfo[] GetPropertyInfo(Type type)
        {
            return type.GetProperties(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance
            );
        }

        private bool IsBackingField(FieldInfo field)
        {
            return field.Name.StartsWith("<") && field.Name.EndsWith(">k__BackingField");
        }

        private void ConvertTo(ref object propValue)
        {
            if (propValue is long)
            {
                long value = (long)propValue;
                propValue = Convert.ToInt32(value);
            }

            if (propValue is double)
            {
                double value = (double)propValue;
                propValue = Convert.ToSingle(value);
            }
        }

        private void ConvertTo(Type type, ref object fieldValue)
        {
            if (fieldValue is JObject)
            {
                JToken token = fieldValue as JToken;
                fieldValue = token.ToObject(type, JsonSerializer.CreateDefault());
            }

            if (fieldValue is long)
            {
                long value = (long)fieldValue;
                fieldValue = Convert.ToInt32(value);
            }

            if (fieldValue is double)
            {
                double value = (double)fieldValue;
                fieldValue = Convert.ToSingle(value);
            }
        }
        #endregion

        #region TRANSFORM
        private void LoadTransform(Type element, Transform tr)
        {
            if (element == typeof(Transform))
            {
                object position = m_data[element.Name]["position"];
                object rotation = m_data[element.Name]["rotation"];
                object scale = m_data[element.Name]["scale"];

                ConvertTo(typeof(Vector3), ref position);
                ConvertTo(typeof(Quaternion), ref rotation);
                ConvertTo(typeof(Vector3), ref scale);

                if (m_data[element.Name].ContainsKey("position"))
                    tr.position = (Vector3)position;
                if (m_data[element.Name].ContainsKey("rotation"))
                    tr.rotation = (Quaternion)rotation;
                if (m_data[element.Name].ContainsKey("scale"))
                    tr.localScale = (Vector3)scale;
            }
        }

        private void SaveTransform(Type element, Type prop, Transform tr)
        {
            if (prop == typeof(Transform) && element == typeof(Transform))
            {
                var position = prop.GetProperty("position");
                var rotation = prop.GetProperty("rotation");
                var scale = prop.GetProperty("lossyScale");

                if (!m_data[element.Name].ContainsKey("position"))
                    m_data[element.Name].Add("position", position.GetValue(tr));
                if (!m_data[element.Name].ContainsKey("rotation"))
                    m_data[element.Name].Add("rotation", rotation.GetValue(tr));
                if (!m_data[element.Name].ContainsKey("scale"))
                    m_data[element.Name].Add("scale", scale.GetValue(tr));
            }
        }
        #endregion
    }

}