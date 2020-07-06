using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnter : MonoBehaviour
{

    public BoolSync bs;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        bs.SetBoolValue(true);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit");

        bs.SetBoolValue(false);
    }
}
