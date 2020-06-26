using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnFocus : MonoBehaviour
{
    [Range(1.0f, 8f)]
    public float speed = 1f;

    [Range(1.0f,4)]
    public float scaler = 2f;
    public Transform target;
    // Use this for initialization
    private float initScale;
    void Start()
    {
        initScale = target.localScale.x;
    }

    public void Update()
    {
        if (inFocus)
        {
            float currentScale = target.localScale.x;
            float newScale = Mathf.Lerp(currentScale, initScale * scaler, Time.deltaTime*speed* speed);
            target.transform.localScale = new Vector3(newScale, newScale, newScale);

        }
        else
        {
            float currentScale = target.localScale.x;
            float newScale = Mathf.Lerp(currentScale, initScale, Time.deltaTime* speed* speed);
            target.transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }


    public bool inFocus = false;

    public void setFocusState(bool state)
    {
        inFocus = state;
    }

}
