using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GameObjectTagFinder : MonoBehaviour {
    public GameObject[] objectList;
    public string tagName;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        objectList = GameObject.FindGameObjectsWithTag(tagName);
    }
}
