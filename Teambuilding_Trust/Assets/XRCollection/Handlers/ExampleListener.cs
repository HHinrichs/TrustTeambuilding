using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using XRCollection.Handlers;

namespace XRCollection.Handlers
{
    public class ExampleListener : MonoBehaviour
    {

        public ButtonHandler primaryAxisClickHandler = null;
        public AxisHandler2D primaryAxisHandler = null;
        public TriggerHandler triggerHandler = null;

        public void OnEnable()
        {
            if(primaryAxisClickHandler != null)
            {
                primaryAxisClickHandler.OnButtonDown += PrintPrimaryButtonDown;
                primaryAxisClickHandler.OnButtonUp += PrintPrimaryButtonUp;

            }
            if (primaryAxisHandler != null)
                primaryAxisHandler.OnValueChanged += PrintPrimaryAxis;
            if (triggerHandler != null)
                triggerHandler.OnValueChanged += PrintTrigger;
        }

        public void OnDisable()
        {
            if (primaryAxisClickHandler != null)
            {
                primaryAxisClickHandler.OnButtonDown -= PrintPrimaryButtonDown;
                primaryAxisClickHandler.OnButtonUp -= PrintPrimaryButtonUp;

            }
            if (primaryAxisHandler != null)
                primaryAxisHandler.OnValueChanged -= PrintPrimaryAxis;
            if (triggerHandler != null)
                triggerHandler.OnValueChanged -= PrintTrigger;
        }

        public void PrintPrimaryButtonDown(XRController controller)
        {
            print("Primary Button Down");
        }

        public void PrintPrimaryButtonUp(XRController controller)
        {
            print("Primary Button Up");
        }

        public void PrintPrimaryAxis(XRController controller, Vector2 value)
        {
            print("Primary Axis " + value);
        }

        public void PrintTrigger(XRController controller, float value)
        {
            print("Primary Axis2D " + value);
        
        }

    }
}
