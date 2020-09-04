using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarHeightChangeController : MonoBehaviour
{
    private Vector3 newTransformPosition;
    private float zValueChanged = 0f;
    private float xValueChanged = 0f;
    private float yValue;
    public float SetZValue { set { zValueChanged = value; } }
    public float SetXValue { set { xValueChanged = value; } }

    public float GetYValue { get { return yValue; } }
    private void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            if (transform.position.y > 0)
            {
                newTransformPosition =  new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);
                transform.position = newTransformPosition;
                yValue = transform.position.y;
            }
        }
        if (Input.GetKeyDown("e"))
        {
            if (transform.position.y < 0.5f)
            {
                newTransformPosition = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
                transform.position = newTransformPosition;
                yValue = transform.position.y;
            }
        }

        if (Input.GetKeyDown("w"))
        {
            if (zValueChanged >= 0.5f)
                return;
            else
                zValueChanged += 0.05f;
                transform.Translate(0,0,0.05f, Space.Self);
                //newTransformPosition = transform.forward * new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z +0.05f);
                //transform.localPosition = newTransformPosition;

        }

        if (Input.GetKeyDown("s"))
        {
            if (zValueChanged <= -0.5f)
                return;
            else
                zValueChanged -= 0.05f;
            transform.Translate(0, 0, -0.05f, Space.Self);
            //newTransformPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.05f);
            //transform.localPosition = newTransformPosition;
        }

        if (Input.GetKeyDown("a"))
        {
            if (xValueChanged <= -0.5f)
                return;
            else
                xValueChanged -= 0.05f;

            transform.Translate(-0.05f, 0, 0, Space.Self);
            //newTransformPosition = new Vector3(transform.localPosition.x + 0.05f, transform.localPosition.y, transform.localPosition.z);
            //transform.localPosition = newTransformPosition;
        }

        if (Input.GetKeyDown("d"))
        {
            if (xValueChanged >= 0.5f)
                return;
            else
                xValueChanged += 0.05f;

            transform.Translate(0.05f, 0, 0, Space.Self);
            //newTransformPosition = new Vector3(transform.localPosition.x -0.05f, transform.localPosition.y , transform.localPosition.z);
            //transform.localPosition = newTransformPosition;

        }
    }

    private void Start()
    {     
        GameManager.Instance.resetAllCalled += ResetValuesOnReset;
    }
    public void ResetValuesOnReset()
    {
        zValueChanged = 0f;
        xValueChanged = 0f;
    }
}
