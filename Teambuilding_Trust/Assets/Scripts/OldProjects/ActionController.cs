using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour {

    public enum Action {Selection, Point ,Self};

    public static ActionController Instance;

    public delegate void OnNewAction(Action action, string param, Vector3 pos);
    public static event OnNewAction onNewAction;

	// Use this for initialization
	void Awake() {
        Instance = this;
	}

    public List<Action> allActions = new List<Action>();
	
	public void reportAction(Action action, string param, Vector3 pos)
    {
        allActions.Add(action);
		if(onNewAction != null)
        	onNewAction(action, param,pos);
    }



    
}
