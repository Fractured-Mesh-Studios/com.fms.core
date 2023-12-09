using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace GameEngine.Data
{
    public class DataLoader : MonoBehaviour
    {
        public List<Component> components = new List<Component>();

        private FileDataHandler m_file;
        private SObject m_object;

        private void Awake()
        {
            InitializeFileHandler();
        }

        public void Load()
        {
            InitializeFileHandler();

            if (m_file != null)
            {
                SObject loadedObject = m_file.Load<SObject>();
                Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();

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

            Rigidbody rigidbody = GetComponent<Rigidbody>();

            if (m_file != null)
            {
                CreateObjectData(rigidbody);

                m_object.component = CreateComponentContainer();

                m_file.Save(m_object);
            }
        }

        #region PRIVATE

        private void InitializeFileHandler()
        {
            string path = Application.persistentDataPath;
            m_file = new FileDataHandler(path, $"{gameObject.name}.data");
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
                UpdateFieldValues(index, loadedComponent, type, fieldInfo, f);
            }
        }

        private System.Reflection.FieldInfo[] GetFieldInfo(Type type)
        {
            return type.GetFields(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.FlattenHierarchy
            );
        }

        private void UpdateFieldValues(int index, SComponent loadedComponent, Type type, System.Reflection.FieldInfo[] fieldInfo, int f)
        {
            if (fieldInfo[f].Name == loadedComponent.field[f].name)
            {
                object fieldValue = loadedComponent.field[f].value;

                ConvertTo(fieldInfo, f, ref fieldValue);

                fieldInfo[f].SetValue(components[index], fieldValue);
            }
        }

        private void ConvertTo(System.Reflection.FieldInfo[] fieldInfo, int f, ref object fieldValue)
        {
            if (fieldValue is JObject)
            {
                JToken token = fieldValue as JToken;
                fieldValue = token.ToObject(fieldInfo[f].FieldType, JsonSerializer.CreateDefault());
            }

            ConvertDoubleValue(ref fieldValue);
            ConvertLongValue(ref fieldValue);
        }

        private void ConvertDoubleValue(ref object fieldValue)
        {
            if (fieldValue is double)
            {
                double value = (double)fieldValue;
                fieldValue = Convert.ToSingle(value);
            }
        }

        private void ConvertLongValue(ref object fieldValue)
        {
            if (fieldValue is long)
            {
                long value = (long)fieldValue;
                fieldValue = Convert.ToInt32(value);
            }
        }

        private void CreateObjectData(Rigidbody rigidbody)
        {
            m_object.id = gameObject.GetHashCode();
            m_object.name = gameObject.name;
            m_object.transform = new STransform(transform);
            m_object.rigidbody = new SRigidbody(rigidbody);
        }

        private SComponent[] CreateComponentContainer()
        {
            List<SComponent> componentContainer = new List<SComponent>();
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

                componentContainer.Add(component);
            }
            return componentContainer.ToArray();
        }

        #endregion
    }
}
