using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class AvatarRenderSettings : MonoBehaviour
{
    //public LayersToRender layersToRender;
    //Camera cam;
    //Component[] allTransforms;

    //private void Awake()
    //{
    //    cam = FindObjectOfType<Camera>();
    //}

    //void Start()
    //{
    //    this.gameObject.layer = 9;

    //    allTransforms = GetComponentsInChildren(typeof(Transform));
    //    foreach (Transform child in allTransforms)
    //        child.gameObject.layer = 9;

    //    if (cam != null)
    //        cam.cullingMask = layersToRender.CustomLayerMask;
    //}

    private void Start()
    {
        if (GetComponent<RealtimeView>().isOwnedLocally)
        {
            Component[] renderer = GetComponentsInChildren(typeof(Renderer),true);
            foreach(Renderer rend in renderer)
            {
                rend.enabled = false;
            }
        }
    }

}
