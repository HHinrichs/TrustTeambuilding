using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneViewManager : MonoBehaviour {

	public GameObject [] interactableObjects;

	public enum ViewType{INSTRUCTOR, WORKER};
	public ViewType viewType;

	public GameObject [] cubes;

	public static SceneViewManager Instance;
	public void Awake(){
		Instance = this;
	}


	void Start () {
		
	}

	public void setViewType(ViewType type){
		switch (type) {
		case ViewType.INSTRUCTOR:
			setInstructor ();
			break;

		case ViewType.WORKER:
			setWorker ();
			break;

		default:
			break;
		}


	}

	private void setInstructor(){

	}

	private void setWorker(){

	}
		

}
