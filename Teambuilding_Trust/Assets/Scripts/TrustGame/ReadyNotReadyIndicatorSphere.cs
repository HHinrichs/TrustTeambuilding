using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyNotReadyIndicatorSphere : MonoBehaviour
{
    Renderer rend;
    public Material NotReadyMaterial;
    public Material ReadyMaterial;
    public void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = NotReadyMaterial;
    }

    public void SetNotReadyMaterial()
    {
        rend.material = NotReadyMaterial;
    }

    public void SetReadyMaterial()
    {
        rend.material = ReadyMaterial;
    }
}
