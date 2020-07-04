using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorPosition : MonoBehaviour
{
    [SerializeField] Transform mirroredObject;

    void Update()
    {
        mirroredObject.SetPositionAndRotation(transform.position, transform.rotation);    
    }
}
