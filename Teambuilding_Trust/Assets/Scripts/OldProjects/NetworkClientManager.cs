using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkClientManager : MonoBehaviour
{
    public string IP = "127.0.0.1";
    public bool enableConnection = false;
    public bool enableSelfIPSet = false;
    public bool enableAutoConnection = false;
    public bool enableSelfNameSet = false;

    public static NetworkClientManager Instance;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(Instance);
            Debug.Log("There was more than one Instance of " + this.gameObject.name + " in the scene.. therefore Destroing it!");
        }
    }

    private void Start()
    {
    }
}
