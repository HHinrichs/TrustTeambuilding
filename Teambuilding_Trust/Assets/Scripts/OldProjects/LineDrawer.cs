using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour {
    public LineRenderer lr;
    public Transform pointA;
    public Transform pointB;
    // Use this for initialization
    void Start () {
        lr.positionCount = 2;
	}
	
	// Update is called once per frame
	void Update () {
        lr.SetPosition(0, pointA.position);
        lr.SetPosition(1, pointB.position);
    }
}
