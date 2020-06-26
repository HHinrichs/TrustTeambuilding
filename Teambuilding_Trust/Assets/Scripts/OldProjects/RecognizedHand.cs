using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecognizedHand : MonoBehaviour {
	public Material outlineMaterial;
	[Range(0f,1f)]
	public float outlineMat_Smoothness = 0f;

	[Range(0.1f,10f)]
	public float speed = 1f;

	[Range(0f,1f)]
	public float minValue = 0.2f;
	[Range(0f,1f)]
	public float maxValue = 0.8f;

	// Use this for initialization
	void Start () {
		outlineMaterial.SetFloat ("_MixColor", 0f);
	}

	private bool fadeIn = true;
	public float fadeValue = 0f;
	// Update is called once per frame
	void FixedUpdate () {
		if (fadeIn) {

			fadeValue += Time.deltaTime;
			outlineMaterial.SetFloat ("_MixColor", fadeValue);
			if (fadeValue >= 1f) {
				fadeIn = false;
			}

		} else {

			outlineMat_Smoothness = (Mathf.Sin (Time.realtimeSinceStartup*speed)+1f)/2f * (maxValue - minValue) + minValue;
			outlineMaterial.SetFloat ("_Smoothness", outlineMat_Smoothness);
		}



	}

}
