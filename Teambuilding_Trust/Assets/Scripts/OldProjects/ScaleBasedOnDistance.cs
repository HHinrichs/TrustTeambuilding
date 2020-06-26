using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBasedOnDistance : MonoBehaviour {
	
	[Range(0.5f,1.5f)]
	public float scaleFactor = 1f;
	private LineRenderer lineRenderer;
	// Use this for initialization
	void Start () {
		lineRenderer = this.GetComponentInChildren<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance (Camera.main.transform.position, this.transform.position);

		float currentScale = this.transform.localScale.x;
		float newScale = distance * scaleFactor * 0.2f; //0.2 Hololens Factor
		lineRenderer.startWidth = distance * scaleFactor * 0.005f *  0.2f;


		this.transform.localScale = new Vector3 (newScale,newScale,newScale);
	
		
	}
}
