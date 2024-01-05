using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreEngine
{
    public static class Extension
    {
        #region String
        public static string Color(this string msg, Color color) { return "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + msg + "</color>"; }
        public static string Bold(this string msg) { return "<b>" + msg + "</b>"; }
        public static string Italic(this string msg) { return "<i>" + msg + "</i>"; }
        public static string Size(this string msg, int size) { return "<size=" + size + ">" + msg + "</size>"; }
        public static string CopyToClipboard(this string msg) { GUIUtility.systemCopyBuffer = msg; return GUIUtility.systemCopyBuffer; }
        #endregion

        #region Color
        /*
        BlendModeClear,           R = 0 
        BlendModeCopy,            R = S 
        BlendModeSourceIn,        R = S*Da 
        BlendModeSourceOut,       R = S*(1 - Da) 
        BlendModeSourceAtop,      R = S*Da + D*(1 - Sa) 
        BlendModeDestinationOver,     R = S*(1 - Da) + D 
        BlendModeDestinationIn,       R = D*Sa 
        BlendModeDestinationOut,      R = D*(1 - Sa) 
        BlendModeDestinationAtop,     R = S*(1 - Da) + D*Sa 
        BlendModeXOR,             R = S*(1 - Da) + D*(1 - Sa) 
        BlendModePlusDarker,      R = MAX(0, (1 - D) + (1 - S)) 
        BlendModePlusLighter      R = MIN(1, S + D) 
        */

        public static Color NormalBlend(this Color src, Color dst)
        {
            float sAlpha = src.a;
            float dAlpha = (1 - src.a) * dst.a;
            return dst * dAlpha + src * sAlpha;
        }
        
        public static Color MultiplyBlend(this Color src, Color dst)
        {
            return (src * dst);
        }

        public static bool Compare(this Color src, Color dst, float tolerance = 0.1f)
        {
            bool vr = Mathf.Abs(src.r - dst.r)/1.0f <= Mathf.Clamp01(tolerance);
            bool vg = Mathf.Abs(src.g - dst.g)/1.0f <= Mathf.Clamp01(tolerance);
            bool vb = Mathf.Abs(src.b - dst.b)/1.0f <= Mathf.Clamp01(tolerance);
            bool va = Mathf.Abs(src.a - dst.a)/1.0f <= Mathf.Clamp01(tolerance);

            return (vr && vg && vb && va);
        }

        public static bool CompareRGB(this Color src, Color dst, float tolerance = 0.1f)
        {
            src.a = dst.a = 0.0f;
            return Compare(src, dst, tolerance);
        }
        #endregion

        #region Vector3
        public static bool IsZero(this Vector3 vector3)
        {
            return vector3.sqrMagnitude < 9.99999943962493E-11;
        }

        public static Vector3 RelativeTo(this Vector3 vector3, Transform relativeToThis, bool isPlanar = true)
        {
            Vector3 forward = relativeToThis.forward;

            if (isPlanar)
            {
                forward = Vector3.ProjectOnPlane(forward, Vector3.up);

                if (forward.IsZero())
                    forward = Vector3.ProjectOnPlane(relativeToThis.up, Vector3.up);
            }

            Quaternion q = Quaternion.LookRotation(forward);

            return q * vector3;
        }

        public static Quaternion ToRotation(this Vector3 vector3)
        {
            return Quaternion.Euler(vector3);
        }

        public static Vector3 Clamp(this Vector3 v, float min, float max)
        {
            double sm = v.sqrMagnitude;
            if (sm > (double)max * (double)max) return v.normalized * max;
            else if (sm < (double)min * (double)min) return v.normalized * min;
            return v;
        }
        #endregion

        #region GameObject
        public static Vector3[] GetColliderVertexPositions(this GameObject obj)
        {
            BoxCollider b = obj.GetComponent<BoxCollider>(); //retrieves the Box Collider of the GameObject called obj
            Vector3[] vertices = new Vector3[8];
            vertices[0] = obj.transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f);
            vertices[1] = obj.transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f);
            vertices[2] = obj.transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f);
            vertices[3] = obj.transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f);
            vertices[4] = obj.transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, -b.size.z) * 0.5f);
            vertices[5] = obj.transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, -b.size.z) * 0.5f);
            vertices[6] = obj.transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, b.size.z) * 0.5f);
            vertices[7] = obj.transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, b.size.z) * 0.5f);

            return vertices;
        }
        #endregion
    }
}