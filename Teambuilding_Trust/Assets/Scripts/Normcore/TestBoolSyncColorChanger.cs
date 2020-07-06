using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoolSyncColorChanger : MonoBehaviour
{
    public Material matTrue;
    public Material matFalse;
    private MeshRenderer meshRenderer;
    public BoolSync boolSync;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void OnEnable()
    {
        boolSync.boolValueChanged += UpdateMaterial;
    }
    private void OnDestroy()
    {
        boolSync.boolValueChanged -= UpdateMaterial;
    }

    private void UpdateMaterial()
    {
        if (boolSync.GetBoolValue == true)
            meshRenderer.material = matTrue;
        else
            meshRenderer.material = matFalse;
    }
}
