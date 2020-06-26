using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject : MonoBehaviour {

	public GameObject toggleObject;
	public void toggle(){
		toggleObject.SetActive (!toggleObject.activeSelf);
	}
}
