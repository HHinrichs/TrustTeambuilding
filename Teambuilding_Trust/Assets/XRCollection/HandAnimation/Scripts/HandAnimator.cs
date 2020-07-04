using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using XRCollection.Handlers;

namespace XRCollection.Hand
{
    public class HandAnimator : MonoBehaviour
    {

        public float animationSpeed = 5.0f;
        public TriggerHandler gripHandler;
        public TriggerHandler triggerHandler;
        private Animator animator = null;

        private readonly List<Finger> gripFingers = new List<Finger>()
        {
            new Finger(FingerType.Middle),
            new Finger(FingerType.Ring),
            new Finger(FingerType.Pinky)
        };
        private readonly List<Finger> pointFingers = new List<Finger>()
        {
            new Finger(FingerType.Index),
            new Finger(FingerType.Thumb),
        };

        public void OnEnable()
        {
            if (gripHandler == null || triggerHandler == null)
            {
                Debug.LogWarning("Grip or Trigger Handler is not Set!");
                return;
            }
            gripHandler.OnValueChanged += CheckGrip;
            triggerHandler.OnValueChanged += CheckPointer;
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (gripHandler == null || triggerHandler == null)
                return;
            // Store input
                // Already dont via Axis Handlers!
            // Smooth input Values
            SmoothFinger(pointFingers);
            SmoothFinger(gripFingers);
            // Apply smoothed values
            AnimateFinger(pointFingers);
            AnimateFinger(gripFingers);
        }

        private void CheckGrip(XRController controller, float gripValue)
        {
            SetFingerTargets(gripFingers, gripValue);
        }

        private void CheckPointer(XRController controller, float triggerHandler)
        {
            SetFingerTargets(pointFingers, triggerHandler);
        }

        private void SetFingerTargets(List<Finger> fingers, float value)
        {
            foreach (Finger finger in fingers)
                finger.target = value;
        }

        private void SmoothFinger(List<Finger> fingers)
        {
            foreach (Finger finger in fingers)
            {
                float time = animationSpeed * Time.unscaledDeltaTime;
                finger.current = Mathf.MoveTowards(finger.current, finger.target, time);
            }
        }

        private void AnimateFinger(List<Finger> fingers)
        {
            foreach(Finger finger in fingers)
            {
                AnimateFinger(finger.type.ToString(), finger.current);
            }
        }

        private void AnimateFinger(string finger, float blend)
        {
            animator.SetFloat(finger, blend);
        }
    }
}