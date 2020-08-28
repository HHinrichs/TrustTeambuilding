using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarHeightChangeController : MonoBehaviour
{
    private Vector3 newTransformPosition;

    private void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            if (transform.position.y > 0)
            {
                newTransformPosition = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);
                transform.position = newTransformPosition;
            }
        }
        if (Input.GetKeyDown("e"))
        {
            if (transform.position.y < 0.5f)
            {
                newTransformPosition = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
                transform.position = newTransformPosition;
            }
        }
    }
}
