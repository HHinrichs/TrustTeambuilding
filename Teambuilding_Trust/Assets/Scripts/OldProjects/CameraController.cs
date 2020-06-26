using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform center;
   // public Transform plane;
    
    // Update is called once per frame
    Vector3 previousMousePos = Vector3.zero;
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 mouseDelta = previousMousePos -  Input.mousePosition;

            this.transform.RotateAround(center.position, Vector3.up, -mouseDelta.x);
            this.transform.RotateAround(center.position, this.transform.right, mouseDelta.y);

        }

        //if (Input.GetMouseButton(2))
        //{
        //    Vector3 mouseDelta = previousMousePos - Input.mousePosition;
        //    Vector3 offset = new Vector3(mouseDelta.x, mouseDelta.y, 0f) / 10f;
        //    plane.transform.Translate(offset);


        //}

        Vector2 scrollDelta = Input.mouseScrollDelta;
        this.transform.Translate(0f, 0f, scrollDelta.y * (Vector3.Distance(this.transform.position, center.position) / 10f));
        previousMousePos = Input.mousePosition;
        this.transform.LookAt(center.position);
    }
}
