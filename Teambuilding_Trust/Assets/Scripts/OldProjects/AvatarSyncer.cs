using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSyncer : MonoBehaviour {
	public Transform sceneOrigin;
	public Transform camera;
	public Transform rightHand;
    public Transform leftHand;
	public string clientName;

	[Range(1,30)]
	public int sendRate = 30;
	public Type type;
	public enum Type {Reciever, Sender};

	private float nextUpdate = 0f;

	private void Start()
	{
		rightHand = GameObject.FindGameObjectWithTag("RightHandAnchor").transform;
		leftHand = GameObject.FindGameObjectWithTag("LeftHandAnchor").transform;
		camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}



	#region SyncedSending

	
	#endregion
}

[System.Serializable]
public class AvatarTransformInfos{
	public string clientID;
	public string clientName;
	public Vector3 HeadPos;
	public Vector3 HeadPosForward;
	public Vector3 RightHandPos;
	public Vector3 LeftHandPos;
	public int sendRate = 15;
}
