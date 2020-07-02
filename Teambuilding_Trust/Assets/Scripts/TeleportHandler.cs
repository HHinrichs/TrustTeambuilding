using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Teleporter : MonoBehaviour
{
    InputDevice inputDevice;
    private void Awake()
    {
        inputDevice = GetComponent<XRController>().inputDevice;
    }
    void Start()
    {
    }

}
