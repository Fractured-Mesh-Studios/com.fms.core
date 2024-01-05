using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CoreEditor
{
    public static class EditorExtended
    {
        //Box
        public const string MiniBox = "OL box";
        public const string GroupBox = "GroupBox";
        public const string FrameBox = "FrameBox";
        public const string BlackBox = "LODBlackBox";
        public const string WizardBox = "Wizard Box";
        public const string CreateBox = "U2D.createRect";
        public const string OutlineBox = "EyeDropperPickedPixel";
        public const string GeneralBox = "ChannelStripBg";
        public const string FadeBox = "AnimationEventBackground";

        //DropDown
        public static string BlackDropDown = "PreviewPackageInUse";
        public static string GizmoDropDown = "GV Gizmo DropDown";

        //Color
        private static Color s_defaultBackgroundColor;
        public static Color backgroundColor
        {
            get
            {
                if (s_defaultBackgroundColor.a == 0)
                {
                    var method = typeof(EditorGUIUtility)
                        .GetMethod("GetDefaultBackgroundColor", BindingFlags.NonPublic | BindingFlags.Static);
                    s_defaultBackgroundColor = (Color)method.Invoke(null, null);
                }
                return s_defaultBackgroundColor;
            }
        }
    }
}
