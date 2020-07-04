using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLog : MonoBehaviour
{
    public void ShowMessage()
    {
        Debug.Log("[DebugLog] " + gameObject.name + " called ");
    }
}
