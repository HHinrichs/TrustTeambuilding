﻿using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRCollection.Handlers
{
    [CreateAssetMenu(fileName = "NewAxisHandler", menuName = "InputHelpers/NewAxisHandler")]
    public class AxisHandler : InputHandler, ISerializationCallbackReceiver
    {
        public enum Axis
        {
            None,
            Trigger,
            Grip
        }

        public delegate void ValueChange(XRController controller, float value);
        public event ValueChange OnValueChanged;

        public Axis axis = Axis.None;

        private InputFeatureUsage<float> inputFeature;
        private float previousValue = 0.0f;

        public void OnAfterDeserialize()
        {
            inputFeature = new InputFeatureUsage<float>(axis.ToString());
        }

        public void OnBeforeSerialize()
        {

        }

        public override void HandlerState(XRController controller)
        {
            float value = GetValue(controller);

            if(value != previousValue)
            {
                previousValue = value;
                OnValueChanged?.Invoke(controller, value);
            }
        }

        public float GetValue(XRController controller)
        {
            if (controller.inputDevice.TryGetFeatureValue(inputFeature, out float value))
                return value;

            return 0.0f;
        }
    }
}