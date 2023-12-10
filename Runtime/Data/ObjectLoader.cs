using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Newtonsoft
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameEngine.Data
{
    public class ObjectLoader : MonoBehaviour
    {
        public bool m_encryption = false;
        public List<Component> components = new List<Component>();

        private SObject m_object;
        private FileDataHandler m_file;
        private const string m_key = "ZzP5rMHiMkWzGzh8fHP9JQ==";

        public void Load()
        {
            InitializeFileHandler();

            if (m_file != null)
            {
                SObject loadedObject = m_file.Load<SObject>(m_encryption);
                Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();

                if(loadedObject == null)
                {
                    Debug.LogError($"Error on load file data corruption or invalid encryption key");
                    return;
                }

                ApplyTransformData(loadedObject.transform);
                ApplyRigidbodyData(rigidbody, loadedObject.rigidbody);

                SComponent[] container = loadedObject.component;

                ValidateComponentCount(container.Length);

                for (int i = 0; i < container.Length; i++)
                {
                    UpdateComponentFields(i, container[i]);
                }
            }
        }

        public void Save()
        {
            InitializeFileHandler();

            if (m_file != null)
            {
                CreateObjectData();

                m_object.component = CreateComponentContainer();

                m_file.Save(m_object, m_encryption);
            }
        }

        #region PRIVATE

        private void InitializeFileHandler()
        {
            string path = Application.persistentDataPath;
            string name = $"{gameObject.name}.data";

            m_file = new FileDataHandler(path, name, m_key);
        }

        private void ApplyTransformData(STransform transformData)
        {
            transform.position = transformData.position;
            transform.rotation = transformData.rotation;
            transform.localScale = transformData.scale;
        }

        private void ApplyRigidbodyData(Rigidbody rigidbody, SRigidbody loadedRigidbody)
        {
            if (rigidbody != null && loadedRigidbody != null)
            {
                rigidbody.mass = loadedRigidbody.mass;
                rigidbody.drag = loadedRigidbody.drag;
                rigidbody.angularDrag = loadedRigidbody.angularDrag;
                rigidbody.useGravity = loadedRigidbody.useGravity;
                rigidbody.isKinematic = loadedRigidbody.isKinematic;
                rigidbody.automaticCenterOfMass = loadedRigidbody.automaticCenterOfMass;
                rigidbody.centerOfMass = loadedRigidbody.centerOfMass;
                rigidbody.inertiaTensor = loadedRigidbody.ínertiaTensor;
                rigidbody.inertiaTensorRotation = loadedRigidbody.inertiaTensorRotation;
                rigidbody.interpolation = loadedRigidbody.interpolation;
                rigidbody.collisionDetectionMode = loadedRigidbody.collisionDetectionMode;
                rigidbody.maxLinearVelocity = loadedRigidbody.maxLinearVelocity;
                rigidbody.maxAngularVelocity = loadedRigidbody.maxAngularVelocity;
            }
        }

        private int ValidateComponentCount(int loadedComponentCount)
        {
            if (loadedComponentCount != components.Count)
            {
                Debug.LogWarning("The number of saved components and the number of loaded components are different", gameObject);
                return components.Count;
            }

            return 0;
        }

        private void UpdateComponentFields(int index, SComponent loadedComponent)
        {
            var type = components[index].GetType();
            var fieldInfo = GetFieldInfo(type);

            for (int f = 0; f < fieldInfo.Length; f++)
            {
                UpdateFieldValues(index, loadedComponent, fieldInfo, f);
            }
        }

        private FieldInfo[] GetFieldInfo(Type type)
        {
            return type.GetFields(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.FlattenHierarchy
            );
        }

        private void UpdateFieldValues(int index, SComponent loadedComponent,FieldInfo[] fieldInfo, int f)
        {
            if (fieldInfo[f].Name == loadedComponent.field[f].name)
            {
                object fieldValue = loadedComponent.field[f].value;

                ConvertTo(fieldInfo, f, ref fieldValue);

                fieldInfo[f].SetValue(components[index], fieldValue);
            }
        }

        private void ConvertTo(FieldInfo[] fieldInfo, int f, ref object fieldValue)
        {
            if (fieldValue is JObject)
            {
                JToken token = fieldValue as JToken;
                fieldValue = token.ToObject(fieldInfo[f].FieldType, JsonSerializer.CreateDefault());
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

        private void CreateObjectData()
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();

            m_object = new SObject();
            m_object.id = gameObject.GetHashCode();
            m_object.name = gameObject.name;
            m_object.transform = new STransform(transform);
            m_object.rigidbody = new SRigidbody(rigidbody);
        }

        private SComponent[] CreateComponentContainer()
        {
            List<SComponent> container = new List<SComponent>();
            for (int i = 0; i < components.Count; i++)
            {
                var type = components[i].GetType();
                var IsComponent = type.IsSubclassOf(typeof(Component));
            
                var fieldInfo = GetFieldInfo(type);

                List<SField> Fields = new List<SField>();
                foreach (var field in fieldInfo)
                {
                    object Value = field.GetValue(components[i]);

                    if (field.FieldType.IsSerializable)
                        Fields.Add(new SField(field.Name, Value));
                }

                SComponent component = new SComponent(
                    type.Name,
                    IsComponent,
                    Fields.ToArray()
                );

                container.Add(component);
            }
            return container.ToArray();
        }

        #endregion
    }
}
