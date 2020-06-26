using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {
	public bool lookAtMainCamera = true;
	public GameObject lookAt;
	public bool useLocalRoation = false;
	public Vector3 rotationOffset;
	public bool lockX, lockY, lockZ;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (useLocalRoation) {
			Vector3 target = Vector3.zero;
			if (lookAtMainCamera) {
				target = Camera.main.transform.position;
			} else {
				target = lookAt.transform.position;
			}


			Vector3 between = -this.transform.position + target;
			Vector3 localEuler = Quaternion.FromToRotation (Vector3.back, between).eulerAngles;
			localEuler.y = 0f;
			localEuler.z = 0f;
			this.transform.localRotation = Quaternion.Euler (localEuler);


			return;
		}



		if (lookAtMainCamera) {
			this.transform.LookAt (Camera.main.transform.position);
		} else {
			this.transform.LookAt (lookAt.transform.position);
		}

		Vector3 euler = this.transform.rotation.eulerAngles;
		if (lockX)
			euler.x = 0f;
		if (lockY)
			euler.y = 0f;
		if (lockZ)
			euler.z = 0f;

		this.transform.rotation = Quaternion.Euler (euler);
		this.transform.Rotate (rotationOffset);

	}
}
