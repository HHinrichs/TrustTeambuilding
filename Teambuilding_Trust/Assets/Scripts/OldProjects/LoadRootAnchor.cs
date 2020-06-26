#if UNITY_WSA_10_0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Persistence;


public class LoadRootAnchor : MonoBehaviour {

    public GameObject positionAnchorObject;
    public GameObject rotationAnchorObject;

    private WorldAnchorStore store;
    // Use this for initialization
    void Start()
    {
        WorldAnchorStore.GetAsync(AnchorStoreLoaded);
    }

    private void AnchorStoreLoaded(WorldAnchorStore store)
    {
        this.store = store;
        LoadAnchors();
    }

    private void LoadAnchors()
    {
        GameObject g = new GameObject();
        this.store.Load(positionAnchorObject.name.ToString(), g);
        this.transform.position = g.transform.position;

        this.store.Load(rotationAnchorObject.name.ToString(), g);
        Vector3 euler = g.transform.rotation.eulerAngles;

        euler = new Vector3(0f,euler.y + 180f, 0f);
        this.transform.rotation = Quaternion.Euler(euler);


    }
}

#endif
