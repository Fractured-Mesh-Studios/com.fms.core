using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using System.IO;
using Codice.Utils;

namespace CoreEditor
{
    public class ThumbnailWindow : EditorWindow
    {
        private static ThumbnailWindow Window;

        private Object Target;
        private string SelectedPath;

        [MenuItem("Window/Thumbnail")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            Window = (ThumbnailWindow)EditorWindow.GetWindow(typeof(ThumbnailWindow));
            Window.Show();

            GUIContent Content = EditorGUIUtility.IconContent("Camera Gizmo");
            Content.text = "Thumbnail Generator";
            Content.tooltip = "Generate a object thumbnail to use ingame [This class Only Work in The Unity Editor]";

            Window.titleContent = Content;
            Window.SelectedPath = Application.dataPath;
        }

        void OnGUI()
        {

            EditorGUILayout.LabelField("Thumbnail Settings", EditorStyles.boldLabel);

            EditorGUILayout.TextField(SelectedPath);
            Target = EditorGUILayout.ObjectField("Target",Target, typeof(Object), true);

            AssetPreview.SetPreviewTextureCacheSize(256);
            Texture2D Image = AssetPreview.GetAssetPreview(Target);

            EditorGUILayout.Space();
            if (Image)
            {
                EditorGUI.DrawPreviewTexture(new Rect(Window.position.width * 0.2f, Window.position.height * 0.3f, 200, 200), Image, null, ScaleMode.ScaleToFit);
            }
            else
            {
                EditorGUILayout.HelpBox("Select an Object to generate a image preview", MessageType.Warning);
            }

            

            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Generate Texture"))
            {
                if (Image)
                {
                    byte[] bytes = Image.EncodeToPNG();
                    File.WriteAllBytes(SelectedPath + "/" + Target.name + ".png", bytes);
                }
            }

            if(GUILayout.Button("Select Save Path"))
            {
                SelectedPath = EditorUtility.OpenFolderPanel("Save Path", Application.dataPath, "None");
            }

            if(GUILayout.Button("Show In Explorer"))
            {
                EditorUtility.RevealInFinder(SelectedPath);
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
