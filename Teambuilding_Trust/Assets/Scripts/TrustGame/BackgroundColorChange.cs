using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorChange : MonoBehaviour
{
    [SerializeField] MeshRenderer MeshRendererLeft;
    [SerializeField] MeshRenderer MeshRendererRight;
    [SerializeField] Material DefaultMaterial;

    private void Start()
    {
        ResetMaterials();
    }
    public void ChangeMaterialLeft(Material material)
    {
        MeshRendererLeft.material = material;
    }

    public void ChangeMaterialRight(Material material)
    {
        MeshRendererRight.material = material;
    }

    public void ResetMaterials()
    {
        MeshRendererLeft.material = DefaultMaterial;
        MeshRendererRight.material = DefaultMaterial;
    }
}
