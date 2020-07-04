using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportGrabActivator : MonoBehaviour
{
    public float m_AxisToPressThreshold;

    public InputHelpers.Button activationButton = InputHelpers.Button.PrimaryAxis2DUp;

    public XRInteractorLineVisual TeleporterLine;
    
    [SerializeField]
    XRNode m_ControllerNode;

    private InputDevice m_InputDevice;
    private InputDevice inputDevice
    {
        get
        {
            return m_InputDevice.isValid ? m_InputDevice : (m_InputDevice = InputDevices.GetDeviceAtXRNode(m_ControllerNode));
        }
    }

    private void Update()
    {
        bool isPressed = false;
        inputDevice.IsPressed(activationButton, out isPressed, m_AxisToPressThreshold);

        TeleporterLine.enabled = isPressed;
    }
}
