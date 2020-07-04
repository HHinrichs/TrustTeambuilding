using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRCollection.Handlers
{
    public class InputManager : MonoBehaviour
    {
        public List<ButtonHandler> allButtonHandlers = new List<ButtonHandler>();
        public List<AxisHandler> allAxisHandlers = new List<AxisHandler>();
        public List<AxisHandler2D> allAxis2DHandlers = new List<AxisHandler2D>();

        private XRController controller = null;

        private void Awake()
        {
            controller = GetComponent<XRController>();
        }

        private void Update()
        {
            HandleButtonEvents();
            HandlerAxis2DEvents();
            HandleAxisEvents();
        }

        private void HandleButtonEvents()
        {
            foreach (ButtonHandler handler in allButtonHandlers)
                handler.HandlerState(controller);
        }

        private void HandlerAxis2DEvents()
        {
            foreach (AxisHandler2D handler in allAxis2DHandlers)
                handler.HandlerState(controller);
        }

        private void HandleAxisEvents()
        {
            foreach (AxisHandler handler in allAxisHandlers)
                handler.HandlerState(controller);
        }
    }
}
