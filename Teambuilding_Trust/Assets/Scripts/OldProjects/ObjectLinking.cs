using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLinking : MonoBehaviour {
	public Transform target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			this.transform.position = target.position;
			this.transform.rotation = target.rotation;
		}
	}
}
