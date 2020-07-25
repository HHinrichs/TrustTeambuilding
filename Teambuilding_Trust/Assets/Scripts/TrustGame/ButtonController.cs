using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ButtonController : MonoBehaviour
{
    [SerializeField] MeshRenderer MeshRendererP1;
    [SerializeField] MeshRenderer MeshRendererP2;
    [SerializeField] MeshRenderer MeshRenderHighlight;
    [SerializeField] Material HighlightMaterial;
    [SerializeField] Material NonHighlightHighlightMaterial;
    private Material DefaultMaterial;
    public bool isPressed = false;
    public int buttonNumber;
    public delegate void ButtonWasPressed(int buttonNumberID);
    public event ButtonWasPressed buttonPressed;

    public UnityEvent unityEvent;
    public bool IsPressed { get { return isPressed; } set { isPressed = value; } }

    private void Start()
    {
        MeshRenderHighlight.material = NonHighlightHighlightMaterial;
    }
    public void ButtonPressed()
    {
        buttonPressed.Invoke(buttonNumber);
    }

    public void SetNonHighlightMaterial()
    {
        MeshRenderHighlight.material = NonHighlightHighlightMaterial;
    }

    public void DeselectButton()
    {
        if (!this.isPressed)
            return;

        IsPressed = false;
        MeshRenderHighlight.material = NonHighlightHighlightMaterial;
    }

    public void SelectButton()
    {
        if (this.isPressed)
            return;

        IsPressed = true;
        MeshRenderHighlight.material = HighlightMaterial;
    }


    public void ChangeMaterialP1(Material material)
    {
        MeshRendererP1.material = material;
    }

    public void ChangeMaterialP2(Material material)
    {
        MeshRendererP2.material = material;
    }

    public void ResetMaterials(Material defaultMaterial)
    {
        MeshRendererP1.material = defaultMaterial;
        MeshRendererP2.material = defaultMaterial;
    }

}
