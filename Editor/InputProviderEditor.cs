using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEditor;

using GameEngine;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.EventSystems;
using CodiceApp.EventTracking;

[CustomEditor(typeof(InputProvider))]
public class InputProviderEditor : Editor
{
    InputProvider Target;
    int i = 0;
    int SelectedIndex = 0;
    string Data;

    private void OnEnable()
    {
        Target = target as InputProvider;

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (EditorApplication.isPlaying)
        {
            if (!EventSystem.current)
            {
                EditorGUILayout.HelpBox("The event system is not setup correctly", MessageType.Warning);
            }
        }

        PopUp();
        if(Target.ActionMapName == string.Empty)
        {
            EditorGUILayout.HelpBox("Action Map Name In Current Target Asset Is <Empty>", MessageType.Warning);
            return;
        }

        InputActionMap Map = null;
        if (Target.Asset)
        {
            Map = Target.Asset.FindActionMap(Target.ActionMapName, true);

            if (Target.MapKeys == null || Target.MapKeys.Length == 0 || Target.MapKeys.Length != Map.actions.Count)
            {
                if (Target.ActionMapName != string.Empty)
                {
                    for(int i = 0; i < Target.Asset.actionMaps.Count; i++)
                    {
                        if(Target.Asset.actionMaps[i] != null && Target.Asset.actionMaps[i].name == Target.ActionMapName)
                        {
                            Target.MapKeys = new bool[Map.actions.Count];
                            for(int j = 0; j < Target.MapKeys.Length; j++)
                            {
                                Target.MapKeys[j] = true;
                            }
                        }
                    }

                }
            }
        }
       
        if(Map != null)
        {
            i = 0;
            EditorGUILayout.Space();

            EditorGUILayout.LabelField(Target.Mode+" To "+Target.gameObject);
            EditorGUILayout.BeginVertical("Box");
            foreach (InputAction action in Map.actions)
            {
                Data = "[Name = " + action.name + "]"; 

                EditorGUILayout.BeginHorizontal();
                if(i >= 0 && i < Target.MapKeys.Length)
                {
                    GUIContent Content = new GUIContent("On" + action.name + "(CallbackContext);", Data);
                    Target.MapKeys[i] = EditorGUILayout.ToggleLeft(Content, Target.MapKeys[i]);
                    i++;
                }
                else
                {
                    EditorGUILayout.LabelField("On" + action.name + "(CallbackContext);");
                }

                Data = action.controls.Count > 0 ? action.controls[0].displayName : "<Unbind>";
                EditorGUILayout.LabelField("Key: ["+Data+"]");
                
                EditorGUILayout.EndHorizontal();    
            }
            EditorGUILayout.EndVertical();
        }
    }

    private void PopUp()
    {
        if (Target.Asset)
        {
            string[] ActionMapNames = new string[Target.Asset.actionMaps.Count];
            for(i = 0; i < ActionMapNames.Length; i++)
            {
                ActionMapNames[i] = Target.Asset.actionMaps[i].name;
                if(Target.ActionMapName == ActionMapNames[i])
                {
                    SelectedIndex = i;
                }
            }

            ActionMapNames = ActionMapNames.Append("None").ToArray();

            GUIContent Content = new GUIContent("Action Map List", "Action map list in current target action asset");
            SelectedIndex = EditorGUILayout.Popup(Content, SelectedIndex, ActionMapNames);

            if (ActionMapNames[SelectedIndex] != "None")
            {
                Target.ActionMapName = ActionMapNames[SelectedIndex];
            }
            else 
                Target.ActionMapName = string.Empty;

        }
    }
}
