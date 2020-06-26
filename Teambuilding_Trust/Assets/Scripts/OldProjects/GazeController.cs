using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeController : MonoBehaviour {
	public LayerMask gazeLayerMask;
	[Range(0f,0.1f)]
	public float gazeMarjerOffset = 0.05f;
	public Transform gazeMarker;
	// Use this for initialization
	private LineRenderer lr;
	void Start () {
		lr= this.GetComponent<LineRenderer> ();
	}
	public Vector2 scaler;
	// Update is called once per frame
	float offsetAnimation = 0f;
	void Update () {
		RaycastHit hit;
		if (Physics.Raycast (this.transform.position, this.transform.forward, out hit, 10f, gazeLayerMask)) {
			gazeMarker.gameObject.SetActive (true);
			lr.enabled = true;
			gazeMarker.transform.position = hit.point + hit.normal * gazeMarjerOffset;
			gazeMarker.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
			gazeMarker.transform.Rotate (90f, 0f, 0f);

			lr.SetPosition (0, this.transform.position + this.transform.forward * 0.16f);
			lr.SetPosition (1, hit.point);
			//float dist = Vector3.Distance (this.transform.position, hit.point);
			lr.materials [0].mainTextureOffset = new Vector2(offsetAnimation,0f);
			offsetAnimation -= Time.deltaTime *0.1f;

		} else {
			gazeMarker.gameObject.SetActive (false);
			lr.enabled = false;
		}
	}
}
