using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMenu : MonoBehaviour {

	public GameObject panel;
	public void close(){
		panel.SetActive (false);
	}

	public void open(){
		panel.SetActive (true);
	}


}
