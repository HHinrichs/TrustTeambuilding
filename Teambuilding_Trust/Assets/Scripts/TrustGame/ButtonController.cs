using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] ButtonScriptableObject buttonScriptableObject;
    [SerializeField] MeshRenderer MeshRendererP1;
    [SerializeField] MeshRenderer MeshRendererP2;
    [SerializeField] MeshRenderer MeshRenderHighlight;
    [SerializeField] Material HighlightMaterial;
    [SerializeField] Material NonHighlightHighlightMaterial;
    private Material DefaultMaterial;

    public delegate void ButtonWasPressed(int buttonNumberID);
    public event ButtonWasPressed buttonPressed;

    private void Start()
    {
        MeshRenderHighlight.material = NonHighlightHighlightMaterial;
    }
    public void ButtonPressed()
    {
        buttonPressed.Invoke(buttonScriptableObject.buttonNumber);


    }

    public void HighlightButtonAndSetPressedState()
    {

        // Switch Pressed State and highlight material
        if (buttonScriptableObject.isPressed == false)
        {
            buttonScriptableObject.isPressed = true;
            MeshRenderHighlight.material = HighlightMaterial;
        }
        else
        {
            buttonScriptableObject.isPressed = false;
            MeshRenderHighlight.material = NonHighlightHighlightMaterial;
        }

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
