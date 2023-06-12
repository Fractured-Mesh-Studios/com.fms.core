using UnityEngine;
using GameEngine;

namespace DebugEngine
{
    public class Debug
    {
        public static bool EnableAll = false;
        public static Color ContextColor = new Color(0.0f, 0.63f, 0.90f);

        //Logs
        public static void Log(object message)
        {
            Log(message, null, true);
        }

        public static void Log(object message, Object context, bool Enable = true)
        {
            if (Enable || EnableAll)
            {
                if (context)
                {
                    message = "<color=#" + ColorUtility.ToHtmlStringRGBA(ContextColor) + ">" +
                        "[" + context.name + "]" + "</color>" + " " + message;
                }
                UnityEngine.Debug.Log(message, context);
            }
        }

        public static void Log(object message, Color color, Object context, bool Enable)
        {
            if (Enable || EnableAll)
            {
                string Context = context ? ("["+context.name+"]").Color(ContextColor) : string.Empty;
                string Message = message.ToString().Color(color);
                message = Context + " " + Message;
                UnityEngine.Debug.Log(message, context);
            }
        }

        public static void LogWarning(object message)
        {
            LogWarning(message, null, true);
        }

        public static void LogWarning(object message, Object context, bool Enable = true)
        {
            if (Enable || EnableAll)
            {
                if (context)
                {
                    message = "<color=#" + ColorUtility.ToHtmlStringRGBA(ContextColor) + ">" +
                        "[" + context.name + "]" + "</color>" + " " + message;
                }
                UnityEngine.Debug.LogWarning(message, context);
            }
        }

        public static void LogError(object message)
        {
            LogError(message, null, true);
        }

        public static void LogError(object message, Object context, bool Enable = true)
        {
            if (Enable || EnableAll)
            {
                if (context)
                {
                    message = "<color=#" + ColorUtility.ToHtmlStringRGBA(ContextColor) + ">" +
                        "[" + context.name + "]" + "</color>" + " " + message;
                }
                UnityEngine.Debug.LogError(message, context);
            }
        }

        public static void LogAssertion(object message)
        {
            LogAssertion(message, null, true);
        }

        public static void LogAssertion(object message, Object context, bool Enable)
        {
            if (Enable)
            {
                if (context)
                {
                    message = "<color=#" + ColorUtility.ToHtmlStringRGBA(ContextColor) + ">" +
                        "[" + context.name + "]" + "</color>" + " " + message;
                }
                UnityEngine.Debug.LogAssertion(message, context);
            }
        }

        public static void LogExeption(System.Exception exception)
        {
            LogExeption(exception, null, true);
        }

        public static void LogExeption(System.Exception exception, Object context, bool Enable)
        {
            if (Enable || EnableAll)
            {
                UnityEngine.Debug.LogException(exception, context);
            }
        }

        public static void Clear()
        {
            UnityEngine.Debug.ClearDeveloperConsole();
        }

    }
}