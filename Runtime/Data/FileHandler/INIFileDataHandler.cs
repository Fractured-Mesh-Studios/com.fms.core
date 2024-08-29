using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

namespace CoreEngine.Data
{
    public class INIFileDataHandler
    {
        private string m_path = string.Empty;
        private INIData m_data = new INIData();
        private bool m_initialized = false;

        public INIFileDataHandler(string path, string filename)
        {
            string name = filename + ".ini";
            m_path = Path.Combine(path, name);
        }

        #region READ
        public T ReadValue<T>(string section, string key) where T : IConvertible
        {
            if (!m_initialized)
                FirstRead();

            bool contains = m_data.ContainsKey(section);
            contains &= m_data[section].ContainsKey(key);

            T value = default;
            if (contains)
            {
                string data = m_data[section][key];

                if(value.GetType() == typeof(int))
                {
                    return (T)(object)int.Parse(data);  
                }

                if (value.GetType() == typeof(bool))
                {
                    return (T)(object)bool.Parse(data);
                }

                if (value.GetType() == typeof(double))
                {
                    return (T)(object)double.Parse(data);
                }

                if (value.GetType() == typeof(float))
                {
                    return (T)(object)float.Parse(data);
                }

                if (value.GetType() == typeof(short))
                {
                    return (T)(object)short.Parse(data);
                }

                if (value.GetType() == typeof(string))
                {
                    return (T)(object)data;
                }

                if (value.GetType() == typeof(byte))
                {
                    return (T)(object)byte.Parse(data);
                }
            }

            return default;
        }

        private bool FirstRead()
        {
            if (File.Exists(m_path))
            {
                using (StreamReader sr = new StreamReader(m_path))
                {
                    string line;
                    string theSection = "";
                    string theKey = "";
                    string theValue = "";
                    while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                    {
                        line.Trim();
                        if (line.StartsWith("[") && line.EndsWith("]"))
                        {
                            theSection = line.Substring(1, line.Length - 2);
                        }
                        else
                        {
                            string[] ln = line.Split(new char[] { '=' });
                            theKey = ln[0].Trim();
                            theValue = ln[1].Trim();
                        }
                        if (theSection == "" || theKey == "" || theValue == "")
                            continue;
                        Populate(theSection, theKey, theValue);
                    }
                }
            }
            return true;
        }

        private void Populate(string section, string key, string value)
        {
            if (m_data.ContainsKey(section))
            {
                if (m_data[section].ContainsKey(key))
                    m_data[section][key] = value;
                else
                    m_data[section].Add(key, value);
            }
            else
            {
                Dictionary<string, string> neuVal = new Dictionary<string, string>();
                neuVal.Add(key.ToString(), value);
                m_data.Add(section.ToString(), neuVal);
            }
        }
        #endregion

        #region WRITE
        /// <summary>
        /// Write data to INI file. Section and Key no in enum.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void WriteValue<T>(string section, string key, T value)
        {
            if (!m_initialized)
                FirstRead();

            Populate(section, key, value.ToString());
            //write ini
            Write();
        }

        private void Write()
        {
            using (StreamWriter sw = new StreamWriter(m_path))
            {
                foreach (KeyValuePair<string, Dictionary<string, string>> sesion in m_data)
                {
                    sw.WriteLine("[" + sesion.Key.ToString() + "]");
                    foreach (KeyValuePair<string, string> child in sesion.Value)
                    {
                        // value must be in one line
                        string value = child.Value.ToString();
                        value = value.Replace(Environment.NewLine, " ");
                        value = value.Replace("\r\n", " ");
                        sw.WriteLine(child.Key.ToString() + " = " + value);
                    }
                }
            }
        }
        #endregion

        public void Delete()
        {
            if (Directory.Exists(m_path))
            {
                Directory.Delete(m_path, true);
            }
        }

    }

    public class INIData : Dictionary<string, Dictionary<string, string>> { }
}
