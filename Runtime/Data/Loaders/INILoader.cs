using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreEngine.Data
{
    public class INILoader
    {
        private INIFileDataHandler m_handler;

        public INILoader(string path, string name)
        {
            m_handler = new INIFileDataHandler(path, name);
        }

        public T Load<T>()
        {
            return default;
        }

        public void Save<T>(T data)
        {

        }

        public void Delete()
        {
            m_handler.Delete();
        }
    }
}
