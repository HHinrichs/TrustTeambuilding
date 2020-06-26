using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour {

    private Camera mainCam;
    public LayerMask interactionMask;
    private void Start()
    {
        mainCam = Camera.main;
    }

    
    public void Update()
    {
        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit,interactionMask))
        {
            this.transform.position = hit.point;
            this.transform.LookAt(hit.point + hit.normal);
        }
    }

}
