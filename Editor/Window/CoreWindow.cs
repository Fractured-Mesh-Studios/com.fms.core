using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CoreEditor.Window
{
    public class CoreWindow : EditorWindow
    {
        protected static CoreWindow s_window;
        protected static SerializedObject s_windowObject;
    }
}
