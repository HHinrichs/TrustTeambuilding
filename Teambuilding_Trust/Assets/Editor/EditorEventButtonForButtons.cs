using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[CustomEditor(typeof(ButtonController))]
public class EditorEventButtonForButtons : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ButtonController myScript = (ButtonController)target;

        if(GUILayout.Button("Do Function!"))
        {
            myScript.unityEvent.Invoke();
        }
    }
}
