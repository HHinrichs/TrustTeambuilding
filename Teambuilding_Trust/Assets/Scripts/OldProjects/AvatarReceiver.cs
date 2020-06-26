using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarReceiver : MonoBehaviour {
	public Transform Origin;
	public GameObject Avatar;
	public Transform headPosition;
    public Transform rightHand;
    public Transform leftHand;
	public AnimationController animationController;


	[Range(1f,10f)]
	public float smoothFactor = 3f;
    private Vector3 headForward;

	

	public void Start(){
	
        if(Origin == null)
        {
            Origin = GameObject.FindGameObjectWithTag("Origin").transform;
        }
	}

	public void Update(){
		if (Time.realtimeSinceStartup > lastStillAlive) {
			Avatar.SetActive (false);
		}	

        //Head
		headPosition.position = Vector3.LerpUnclamped(headPosition.position, networkHeadPos, Time.deltaTime * smoothFactor*smoothFactor);
        headForward = Vector3.LerpUnclamped (headForward, networkHeadForwardPos, Time.deltaTime * smoothFactor * smoothFactor);
		headPosition.LookAt (headForward);

        //Hands
        rightHand.position = Vector3.LerpUnclamped(rightHand.position, networkRightHandPos, Time.deltaTime * smoothFactor * smoothFactor);
        leftHand.position = Vector3.LerpUnclamped(leftHand.position, networkLeftHandPos, Time.deltaTime * smoothFactor * smoothFactor);
    }

	float lastStillAlive = 0f;
	public void stillAlive(){
		Avatar.SetActive (true);
		lastStillAlive = Time.realtimeSinceStartup + 2f; //plus 2 seconds delay

	}



	#region Async Recieving
	private Vector3 networkHeadPos;
	private Vector3 networkHeadForwardPos;
	private Vector3 networkRightHandPos;
	private Vector3 networkLeftHandPos;

	public void updateTransform(string data){
		AvatarTransformInfos avatarTrans = JsonUtility.FromJson<AvatarTransformInfos> (data);
		
        if(Origin != null)
        {
            networkHeadPos = Origin.TransformPoint(avatarTrans.HeadPos);
            networkHeadForwardPos = Origin.TransformPoint(avatarTrans.HeadPosForward);
            networkRightHandPos = Origin.TransformPoint(avatarTrans.RightHandPos);
            networkLeftHandPos = Origin.TransformPoint(avatarTrans.LeftHandPos);
        }
        else
        {
            networkHeadPos = (avatarTrans.HeadPos);
            networkHeadForwardPos = (avatarTrans.HeadPosForward);
            networkRightHandPos = (avatarTrans.RightHandPos);
            networkLeftHandPos = (avatarTrans.LeftHandPos);
        }
       

		stillAlive ();

	}

	#endregion
}
