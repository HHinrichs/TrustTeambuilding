using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLinking : MonoBehaviour {
	public GameObject Avatar;
	public Transform AvatarHead;

	public static TransformLinking Instance;
	public void Awake(){
		Instance = this;
	}



	public void Update(){
		if (Time.realtimeSinceStartup > lastStillAlive) {
			Avatar.SetActive (false);
		}	
	}

	float lastStillAlive = 0f;
	public void stillAlive(string name){
		if(name.Contains("HoloLens")){
			Avatar.SetActive (true);
			lastStillAlive = Time.realtimeSinceStartup + 2f; //plus 2 seconds delay
		}
	}

	

	public void setupLinkings(string name, Transform netTransform){
		Avatar.SetActive (true);
		if(name.Contains("HoloLens")){
			AvatarHead.gameObject.GetComponent<ObjectLinking> ().target = netTransform;
			//AvatarHead.position = netTransform.position;
			//AvatarHead.rotation = netTransform.rotation;
		}
		if(name.Contains("Hand_R")){
			
		}
		if(name.Contains("Hand_L")){

		}
		if(name.Contains("Cursor")){

		}
	}

}
