using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[CustomEditor(typeof(GameManager))]
public class EditorEventButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameManager myScript = (GameManager)target;

        if(GUILayout.Button("Do Function!"))
        {
            myScript.unityEvent.Invoke();
        }
    }
}
