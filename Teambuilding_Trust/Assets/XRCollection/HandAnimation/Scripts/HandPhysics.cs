using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRCollection.Hand
{
    public class HandPhysics : MonoBehaviour
    {
        public float SmoothingAmount = 15f;
        public Transform Target = null;

        private Rigidbody rigidbody = null;
        private Vector3 objectToMovePosition = Vector3.zero;
        private Quaternion objectToMoveRotation = Quaternion.identity;
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            TeleportToTarget();
        }

        private void Update()
        {
            SetTargetPosition();
            SetTargetRotation();
        }

        private void SetTargetPosition()
        {
            float time = SmoothingAmount * Time.unscaledDeltaTime;
            objectToMovePosition = Vector3.Lerp(objectToMovePosition, Target.position, time);
        }

        private void SetTargetRotation()
        {
            float time = SmoothingAmount * Time.unscaledDeltaTime;
            objectToMoveRotation = Quaternion.Slerp(objectToMoveRotation,Target.rotation, time);

        }

        private void FixedUpdate()
        {
            MoveToController();
            RotateToController();
        }

        private void MoveToController()
        {
            Vector3 positionDelta = objectToMovePosition - transform.position;

            rigidbody.velocity = Vector3.zero;
            rigidbody.MovePosition(transform.position + positionDelta);
        }

        private void RotateToController()
        {
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.MoveRotation(objectToMoveRotation);
        }

        public void TeleportToTarget()
        {
            objectToMovePosition = Target.position;
            objectToMoveRotation = Target.rotation;

            transform.position = objectToMovePosition;
            transform.rotation = objectToMoveRotation;
        }
    }
}