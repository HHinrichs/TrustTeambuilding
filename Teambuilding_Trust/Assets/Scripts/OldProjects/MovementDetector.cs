using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementDetector : MonoBehaviour {
    public UnityEvent invokedWhenMovementIsDetected;
	// Use this for initialization
	void Start () {
		
	}
	private Vector3 previousPos;
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(previousPos, this.transform.position) < 0.001f)
        {
            Debug.Log("Movement Detected: " + this.name);
            if(invokedWhenMovementIsDetected != null)
                invokedWhenMovementIsDetected.Invoke();
            previousPos = this.transform.position;
        }
	}
}
