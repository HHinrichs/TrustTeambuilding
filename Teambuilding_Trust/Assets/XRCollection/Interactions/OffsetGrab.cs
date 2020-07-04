using Normal.Realtime;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRCollection.Interactions
{
    public class OffsetGrab : XRGrabInteractable
    {
        // Networking Stuff
        [Header("Networking")]
        [SerializeField] private bool enableNetworking = false;
        RealtimeTransform realtimeTransform;

        //
        private Vector3 interactorPosition = Vector3.zero;
        private Quaternion interactorRotation = Quaternion.identity;

        protected override void Awake()
        {
            base.Awake();

            if (enableNetworking)
            {
                realtimeTransform = GetComponent<RealtimeTransform>();
                if(realtimeTransform == null)
                {
                    Debug.LogWarning("Network Transform is Null on " + gameObject.name);
                    return;
                }
                onSelectEnter.AddListener(delegate { realtimeTransform.RequestOwnership(); }) ;
                onHoverEnter.AddListener(delegate { realtimeTransform.RequestOwnership(); });
                //onHoverExit.AddListener(delegate { realtimeTransform.ClearOwnership(); });
            }

        }

        protected override void OnSelectEnter(XRBaseInteractor interactor)
        {
            base.OnSelectEnter(interactor);
            StoreInteractor(interactor);
            MatchAttachmentPoints(interactor);
        }

        private void StoreInteractor(XRBaseInteractor interactor)
        {
            interactorPosition = interactor.attachTransform.localPosition;
            interactorRotation = interactor.attachTransform.localRotation;
        }

        private void MatchAttachmentPoints(XRBaseInteractor interactor)
        {
            bool hasAttach = attachTransform != null;
            interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;
            interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
        }

        protected override void OnSelectExit(XRBaseInteractor interactor)
        {
            base.OnSelectExit(interactor);
            ResetAttachmentPoint(interactor);
            ClearInteractor(interactor);
        }

        private void ResetAttachmentPoint(XRBaseInteractor interactor)
        {
            interactor.attachTransform.localPosition = interactorPosition;
            interactor.attachTransform.localRotation = interactorRotation;
        }

        private void ClearInteractor(XRBaseInteractor interactor)
        {
            interactorPosition = Vector3.zero;
            interactorRotation = Quaternion.identity;
        }
    }

}
