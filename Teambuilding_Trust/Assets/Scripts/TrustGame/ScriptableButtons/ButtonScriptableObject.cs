using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ScriptableButton",menuName ="ScriptableObjects/ScriptableButton")]
public class ButtonScriptableObject : ScriptableObject
{
    public bool isPressed;
    public int buttonNumber;
}
