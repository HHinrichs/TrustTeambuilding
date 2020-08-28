using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AnimationController : MonoBehaviour
{

    public Animator anim;
    public Transform headEffector;
    public Transform rightHand;
    public Transform leftHand;

    [Range(0.5f, 2.5f)]
    public float yOffset = 2f;

    [Range(0.0f, 1f)]
    public float zOffset = 0.0f;

    [Range(1f, 10f)]
    private float rotationSpeed = 3f;
    [Range(1f, 10f)]
    private float movementSpeed = 8f;
    public bool ForceHandPosition = true;
    // Use this for initialization

    void Start()
    {

    }

    // Update is called once per frame
    void OnAnimatorIK()
    {
        anim.SetLookAtWeight(1f);
        anim.SetLookAtPosition(headEffector.position + headEffector.forward);

        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        anim.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
       // Quaternion rotationRightHand = Quaternion.Euler(new Vector3(rightHand.localRotation.x, rightHand.localRotation.y, rightHand.localRotation.z - 90f));
        anim.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);

        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
       // Quaternion rotationLeftHand = Quaternion.Euler(new Vector3(leftHand.localRotation.x, leftHand.localRotation.y, leftHand.localRotation.z + 90f));
        anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);


    }
    private bool rotationAnimation = false;
    private Quaternion rotateTowards = Quaternion.identity;
    public void LateUpdate()
    {

        //Natural Human rotation behavior
        Vector3 headEffectorMod = headEffector.position - headEffector.forward * zOffset;
        headEffectorMod.y -= yOffset;

        this.transform.position = Vector3.Lerp(this.transform.position, headEffectorMod, Time.deltaTime * movementSpeed);


        Quaternion modHeadRot = Quaternion.Euler(new Vector3(0f, headEffector.rotation.eulerAngles.y, 0f));
        if (Quaternion.Angle(this.transform.rotation, modHeadRot) > 45f)
        {
            rotationAnimation = true;
        }
        if (rotationAnimation)
        {
            rotateTowards = modHeadRot;
            if (Quaternion.Angle(this.transform.rotation, modHeadRot) < 20f)
            {
                rotationAnimation = false;
            }
        }
        if (ForceHandPosition)
        {
            Transform leftHandTransform = anim.GetBoneTransform(HumanBodyBones.LeftHand);
            leftHandTransform.position = leftHand.position;

            Transform rightHandTransform = anim.GetBoneTransform(HumanBodyBones.RightHand);
            rightHandTransform.position = rightHand.position;

        }



        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotateTowards, Time.deltaTime * rotationSpeed);

    }
}
