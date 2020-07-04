using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarRenderSettings : MonoBehaviour
{
    public LayersToRender layersToRender;
    Camera cam;
    Component[] allTransforms;

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
    }

    void Start()
    {
        this.gameObject.layer = 9;

        allTransforms = GetComponentsInChildren(typeof(Transform));
        foreach (Transform child in allTransforms)
            child.gameObject.layer = 9;

        if (cam != null)
            cam.cullingMask = layersToRender.CustomLayerMask;
    }
}
