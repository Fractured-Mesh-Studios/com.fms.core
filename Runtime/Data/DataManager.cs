using Codice.CM.SEIDInfo;
using log4net.Appender;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine
{
    public class DataManager : MonoBehaviour
    {
        public List<Component> components = new List<Component>();

        private FileDataHandler m_file;
        private Translation.Transform m_transform;

        private void Awake()
        {
            string path = Application.persistentDataPath;
            m_file = new FileDataHandler(path, gameObject.name+".txt");

            m_transform = new Translation.Transform(transform);
        }

        public void Load()
        {
            string path = Application.persistentDataPath;
            m_file = new FileDataHandler(path, gameObject.name + ".txt");


            if (m_file != null)
            {
                object[] array = m_file.LoadArray<object>();
                foreach (object obj in array)
                {
                    Debug.Log(obj.ToString());
                }
            }
        }

        public void Save() 
        {
            string path = Application.persistentDataPath;
            m_file = new FileDataHandler(path, gameObject.name + ".txt");

            if (m_file != null)
            {

                for(int i = 0; i < components.Count; i++)
                {
                    var fields = components[i].GetType().GetFields(
                        System.Reflection.BindingFlags.Public |
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance |
                        System.Reflection.BindingFlags.FlattenHierarchy
                    );

                    var prop = components[i].GetType().GetProperties();
                    foreach(var p in prop)
                    {
                        //Debug.Log(p.Name.Color(Color.red));
                    }


                    List<object> concat = new List<object>();
                    foreach(var field in fields)
                    {
                        object value = field.GetValue(components[i]);

                        Debug.Log(new object[] { value.GetType().Name, value });

                        concat.Add( new[] { value.GetType().Name, value } );
                        //m_file.Save(field.GetValue(components[i]));
                    }

                    m_file.SaveArray(concat.ToArray());

                    /*foreach(var o in concat)
                    {
                        Debug.Log(o.ToString());
                    }*/


                }
                //m_file.Save(m_transform);
            }
        }
    }
}
