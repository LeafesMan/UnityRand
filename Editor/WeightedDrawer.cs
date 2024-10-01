/*
 *  Author: Ian
 *
 *  Project: LeafRand
 *
 *  Date: 5/13/23
 *      Property Drawer for Weighted Elements 
 *      Displays Element left of its Weight on 
 *      a single line
 *  Date: 9/28/24
 *      Reimplemented with UI Toolkit
*/
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using static UnityEngine.Rendering.DebugUI.MessageBox;

[CustomPropertyDrawer(typeof(Weighted<>))]
public class WeightedDrawer : PropertyDrawer
{
    /*public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);


        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't indent child fields
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var elementRect = new Rect(position.x-100, position.y, 160, position.height);
        var labelRect = new Rect(position.x + 85, position.y, 45, position.height);
        var weightRect = new Rect(position.x + 132, position.y, 30, position.height);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(elementRect, property.FindPropertyRelative("element"), GUIContent.none);
        EditorGUI.LabelField(labelRect, "Weight:");
        EditorGUI.PropertyField(weightRect, property.FindPropertyRelative("weight"), GUIContent.none);


        // Set indent back to what it was
        EditorGUI.indentLevel = indent;


        EditorGUI.EndProperty();
    }*/


    // Ui toolkit implementation of the above
    // better alignment plus respects any editor Attributes applied to the element
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {

        VisualElement root = new VisualElement();
        root.style.flexDirection = FlexDirection.Row;
        root.style.alignItems = Align.Center;

        root.Add(new Label("Item:"));
        root.Add(new PropertyField(property.FindPropertyRelative("element")) { label = "", style = { flexGrow = 1 } });
        root.Add(new Label("Weight:"){ style = { marginLeft = 15} });
        root.Add(new PropertyField(property.FindPropertyRelative("weight")) { style = {width = 50} });



        return root;
    }
}
