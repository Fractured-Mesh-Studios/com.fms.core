using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;

// IngredientDrawerUIE

namespace GameEditor
{ 
    [CustomPropertyDrawer(typeof(GameEngine.Grid))]
    public class GridEditor : PropertyDrawer
    {

        Vector3 Length = Vector3.zero;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect NewPos;
            EditorGUI.DrawRect(position, new Color(66 / 255f, 66 / 255f, 66 / 255f));

            Length = property.FindPropertyRelative("Length").vector3Value;

            NewPos = new Rect(position.x + position.width - 40, position.y, position.width - 40, 25);
            EditorGUI.LabelField(NewPos, "["+Mathf.Clamp(Length.x*Length.y*Length.z, 0, 999)+"]");
            NewPos = new Rect(position.x + 20, position.y, position.width - 20, 25);
            property.isExpanded = EditorGUI.Foldout(NewPos, property.isExpanded, new GUIContent(property.displayName), true);
            if (property.isExpanded)
            {
                //Size
                NewPos = new Rect(position.x + 20, position.y + 20, position.width - 20, 25);
                EditorGUI.LabelField(NewPos, new GUIContent("Size:", "The size of the cells on their respective coordinate axis. [2.5,1.1,0.3] (World Space)"));
                NewPos = new Rect(position.x + 70, position.y + 24, position.width - 75, 25);
                property.FindPropertyRelative("Size").vector3Value = EditorGUI.Vector3Field(NewPos, string.Empty, property.FindPropertyRelative("Size").vector3Value);
                //Length
                NewPos = new Rect(position.x + 20, position.y + 40, position.width - 20, 25);
                EditorGUI.LabelField(NewPos, new GUIContent("Length:", "The maximum number of rows and columns displayed by the grid. [1x2] (Int)"));
                NewPos = new Rect(position.x + 70, position.y + 44, position.width - 75, 25);
                property.FindPropertyRelative("Length").vector3Value = EditorGUI.Vector3Field(NewPos, string.Empty, property.FindPropertyRelative("Length").vector3Value);
                //
                NewPos = new Rect(position.x + 20, position.y + 60, position.width - 20, 25);
                EditorGUI.LabelField(NewPos, new GUIContent("Space:", "Space between each of the rows and columns of the grid. [0.1] (World Space)"));
                NewPos = new Rect(position.x + 70, position.y + 64, position.width - 75, 25);
                property.FindPropertyRelative("Space").vector3Value = EditorGUI.Vector3Field(NewPos, string.Empty, property.FindPropertyRelative("Space").vector3Value);
            
            }
        
        
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.isExpanded ? 90 : 25;
        }
    }
}