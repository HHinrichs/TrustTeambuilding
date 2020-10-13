using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusHTCHandPositioner : MonoBehaviour
{
    [SerializeField]
    private enum Hands
    {
        LeftHand,
        RightHand
    }

    [SerializeField]
    private enum ControllerMode
    {
        Oculus,
        HTC,
        Both
    }


    [SerializeField] GameObject UnderlyringHandMesh;
    [SerializeField] SphereCollider UnderlyingSphereCollider;
    [SerializeField] Hands myHand;


    public void Start()
    {

        StartCoroutine(SetControllerStates());
        
    }

    IEnumerator SetControllerStates()
    {
        yield return new WaitForSeconds(0.5f);
        ControllerMode controllerMode = (ControllerMode)GameManager.Instance.GetControllerMode;

        if (myHand == Hands.LeftHand)
        {
            if (controllerMode == ControllerMode.Oculus || controllerMode == ControllerMode.Both)
            {
                UnderlyringHandMesh.transform.localPosition = Vector3.zero;
                UnderlyringHandMesh.transform.localRotation = Quaternion.identity;
                UnderlyringHandMesh.transform.localScale = new Vector3(-1, 1, 1);
                UnderlyingSphereCollider.center = new Vector3(-0.01f, -0.04f, 0.04f);

            }

            if (controllerMode == ControllerMode.HTC)
            {
                UnderlyringHandMesh.transform.localPosition = new Vector3(0.01f, -0.021f, -0.047f);
                UnderlyringHandMesh.transform.localRotation = Quaternion.Euler(1.245f, -18.832f, 3.771f);
                UnderlyringHandMesh.transform.localScale = new Vector3(-1, 1, 1);
                UnderlyingSphereCollider.center = new Vector3(0f, -0.06f, -0.02f);

            }

        }

        if (myHand == Hands.RightHand)
        {
            if (controllerMode == ControllerMode.Oculus || controllerMode == ControllerMode.Both)
            {
                UnderlyringHandMesh.transform.localPosition = Vector3.zero;
                UnderlyringHandMesh.transform.localRotation = Quaternion.identity;
                UnderlyringHandMesh.transform.localScale = new Vector3(1, 1, 1);
                UnderlyingSphereCollider.center = new Vector3(0.01f, -0.045f, 0.04f);

            }

            if (controllerMode == ControllerMode.HTC)
            {
                UnderlyringHandMesh.transform.localPosition = new Vector3(-0.008f, -0.019f, -0.1f);
                UnderlyringHandMesh.transform.localRotation = Quaternion.Euler(-1.819f, 21.373f, 6.564f);
                UnderlyringHandMesh.transform.localScale = new Vector3(1, 1, 1);
                UnderlyingSphereCollider.center = new Vector3(0.01f, -0.05f, -0.08f);

            }
        }
    }
}
