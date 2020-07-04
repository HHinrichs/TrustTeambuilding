using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRCollection.VisualHelper
{
    public class HoverHandHider : MonoBehaviour
    {
        private SkinnedMeshRenderer meshRenderer = null;
        private XRDirectInteractor interactor = null;

        private void Awake()
        {
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            interactor = GetComponentInChildren<XRDirectInteractor>();

            interactor.onHoverEnter.AddListener(Show);
            interactor.onHoverExit.AddListener(Hide);
        }

        private void OnDestroy()
        {
            interactor.onHoverEnter.RemoveListener(Show);
            interactor.onHoverExit.RemoveListener(Hide);
        }

        private void Show(XRBaseInteractable interactable)
        {
            meshRenderer.enabled = true;
        }

        private void Hide(XRBaseInteractable interactable)
        {
            meshRenderer.enabled = false;
        }
    }
}
