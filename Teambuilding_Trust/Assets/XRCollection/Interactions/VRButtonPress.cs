using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRCollection.Interactions
{
    public class VRButtonPress : XRBaseInteractable
    {
        public UnityEvent OnPress = null;

        private float yMin = 0f;
        private float yMax = 0f;
        private bool previousPressed = false;

        private float previousHandHeight = 0.0f;
        private XRBaseInteractor hoverinteractor = null;
        protected override void Awake()
        {
            base.Awake();
            onHoverEnter.AddListener(StartPress);
            onHoverExit.AddListener(EndPress);
        }
        private void OnDestroy()
        {
            onHoverEnter.RemoveListener(StartPress);
            onHoverExit.RemoveListener(EndPress);
        }

        private void StartPress(XRBaseInteractor interactor)
        {
            hoverinteractor = interactor;
            // Gets the local Y Position in realation to the Hand
            previousHandHeight = GetLocalYPosition(hoverinteractor.transform.position);
        }

        private void EndPress(XRBaseInteractor interactor)
        {
            hoverinteractor = null;
            previousHandHeight = 0.0f;

            previousPressed = false;
            SetYPosition(yMax);
        }

        private void Start()
        {
            SetMinMax();
        }

        private void SetMinMax()
        {
            Collider collider = GetComponent<Collider>();
            yMin = transform.localPosition.y - (collider.bounds.size.y * 0.5f);
            yMax = transform.localPosition.y;
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            if (hoverinteractor)
            {
                float newHandHeight = GetLocalYPosition(hoverinteractor.transform.position);
                float handDifference = previousHandHeight - newHandHeight;
                previousHandHeight = newHandHeight;

                float newPosition = transform.localPosition.y - handDifference;
                SetYPosition(newPosition);

                CheckPress();
            }
        }

        private float GetLocalYPosition(Vector3 position)
        {
            Vector3 localPosition = transform.root.InverseTransformPoint(position);
            return localPosition.y;
        }

        private void SetYPosition(float position)
        {
            Vector3 newPosition = transform.localPosition;
            newPosition.y = Mathf.Clamp(position, yMin, yMax);
            transform.localPosition = newPosition;
        }

        private void CheckPress()
        {
            bool inPosition = InPosition();

            if (inPosition && inPosition != previousPressed)
                OnPress.Invoke();

            previousPressed = inPosition;
        }

        private bool InPosition()
        {
            float inRange = Mathf.Clamp(transform.localPosition.y, yMin, yMin + 0.01f);
            return transform.localPosition.y == inRange;
        }
    }
}
