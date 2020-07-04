using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRCollection.VisualHelper
{
    public class HoverColorChanger : MonoBehaviour
    {
        public Material SelectedMaterial = null;

        private MeshRenderer meshRenderer = null;
        private XRBaseInteractable interactor = null;
        private Material originalMaterial = null;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            originalMaterial = meshRenderer.material;

            interactor = GetComponent<XRBaseInteractable>();
            interactor.onHoverEnter.AddListener(SetSelectedMaterial);
            interactor.onHoverExit.AddListener(SetOriginalMaterial);
        }

        private void OnDestroy()
        {
            interactor.onHoverEnter.RemoveListener(SetSelectedMaterial);
            interactor.onHoverExit.RemoveListener(SetOriginalMaterial);
        }

        private void SetSelectedMaterial(XRBaseInteractor interactable)
        {
            meshRenderer.material = SelectedMaterial;
        }

        private void SetOriginalMaterial(XRBaseInteractor interactable)
        {
            meshRenderer.material = originalMaterial;
        }
    }
}
