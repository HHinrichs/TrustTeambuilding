using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexAnimator : MonoBehaviour {

	// Use this for initialization
	Mesh mesh;
	void Start () {
		mesh = GetComponent<MeshFilter> ().mesh;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3[] vertices = mesh.vertices;

		for (int i = 0; i <4;i++) {
			vertices [i] += new Vector3 (Mathf.Sin (Time.realtimeSinceStartup * 0.01f) * 0.1f,0f,0f);
		}
		mesh.vertices = vertices;
		mesh.RecalculateBounds ();
	}

	
}
