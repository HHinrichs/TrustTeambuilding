using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPointer : MonoBehaviour {


	public static HandPointer Instance;
	public void Awake(){
		Instance = this;
	}


	public Transform camera;
	public Transform recognzedHand;
	public bool currentlyPointing = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (recognzedHand != null) {
			initHandPointing ();

		} else {
			endHandPointing ();
		}
	}

	private void initHandPointing(){
		currentlyPointing = true;
		Ray pointingRay = new Ray (camera.position, -camera.position + recognzedHand.position);
		RaycastHit hit;
		if (Physics.Raycast (pointingRay, out hit)) {
			Debug.DrawLine (camera.position, hit.point);
		}


	}

	private void endHandPointing(){
		currentlyPointing = false;
	}
}
