using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEditor;

using CoreEngine;
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
                EditorGUILayout.HelpBox("The interface event system is not setup correctly", MessageType.Warning);
            }
        }

        PopUp();
        if(Target.actionMapName == string.Empty)
        {
            EditorGUILayout.HelpBox("Action Map Name In Current Target Asset Is <Empty>", MessageType.Warning);
            return;
        }

        InputActionMap Map = null;
        if (Target.asset)
        {
            Map = Target.asset.FindActionMap(Target.actionMapName, true);

            if (Target.mapKeys == null || Target.mapKeys.Length == 0 || Target.mapKeys.Length != Map.actions.Count)
            {
                if (Target.actionMapName != string.Empty)
                {
                    for(int i = 0; i < Target.asset.actionMaps.Count; i++)
                    {
                        if(Target.asset.actionMaps[i] != null && Target.asset.actionMaps[i].name == Target.actionMapName)
                        {
                            Target.mapKeys = new bool[Map.actions.Count];
                            for(int j = 0; j < Target.mapKeys.Length; j++)
                            {
                                Target.mapKeys[j] = true;
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

            EditorGUILayout.LabelField(Target.mode+" To "+Target.gameObject);
            EditorGUILayout.BeginVertical("Box");
            foreach (InputAction action in Map.actions)
            {
                Data = "[Name = " + action.name + "]"; 

                EditorGUILayout.BeginHorizontal();
                if(i >= 0 && i < Target.mapKeys.Length)
                {
                    GUIContent Content = new GUIContent("On" + action.name + "(InputValue);", Data);
                    Target.mapKeys[i] = EditorGUILayout.ToggleLeft(Content, Target.mapKeys[i]);
                    i++;
                }
                else
                {
                    EditorGUILayout.LabelField("On" + action.name + "(InputValue);");
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
        if (Target.asset)
        {
            string[] ActionMapNames = new string[Target.asset.actionMaps.Count];
            for(i = 0; i < ActionMapNames.Length; i++)
            {
                ActionMapNames[i] = Target.asset.actionMaps[i].name;
                if(Target.actionMapName == ActionMapNames[i])
                {
                    SelectedIndex = i;
                }
            }

            ActionMapNames = ActionMapNames.Append("None").ToArray();

            GUIContent Content = new GUIContent("Action Map List", "Action map list in current target action asset");
            SelectedIndex = EditorGUILayout.Popup(Content, SelectedIndex, ActionMapNames);

            if (ActionMapNames[SelectedIndex] != "None")
            {
                Target.actionMapName = ActionMapNames[SelectedIndex];
            }
            else 
                Target.actionMapName = string.Empty;

        }
    }
}
