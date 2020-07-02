using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInteractionManagerFinder : MonoBehaviour
{
    XRInteractionManager interactionManager;
    // Start is called before the first frame update
    private void Awake()
    {
        interactionManager = GetComponent<XRDirectInteractor>().interactionManager;
        
        if (interactionManager == null)
        {
            Debug.Log("<color=red> No XRInteractionManagerfound for "+this.gameObject.name+" Searching in Scene ...</color>");
            interactionManager = FindObjectOfType<XRInteractionManager>();
        }

        if(interactionManager == null)
            Debug.Log("<color=red>XRInteractionManager finally NOT found for " + this.gameObject.name + "!</color>");
    }
    
}
