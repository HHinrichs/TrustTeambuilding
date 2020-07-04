using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRCollection.Handlers
{
    public class InputManager : MonoBehaviour
    {
        public List<ButtonHandler> allButtonHandlers = new List<ButtonHandler>();
        public List<TriggerHandler> allTriggerHandlers = new List<TriggerHandler>();
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
            if (allButtonHandlers == null)
                return;

            foreach (ButtonHandler handler in allButtonHandlers)
                handler.HandlerState(controller);
        }

        private void HandlerAxis2DEvents()
        {
            if (allAxis2DHandlers == null)
                return;

            foreach (AxisHandler2D handler in allAxis2DHandlers)
                handler.HandlerState(controller);
        }

        private void HandleAxisEvents()
        {
            if (allTriggerHandlers == null)
                return;

            foreach (TriggerHandler handler in allTriggerHandlers)
                handler.HandlerState(controller);
        }
    }
}
