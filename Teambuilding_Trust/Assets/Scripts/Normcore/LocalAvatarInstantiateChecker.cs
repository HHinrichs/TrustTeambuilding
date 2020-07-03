using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalAvatarInstantiateChecker : MonoBehaviour
{
    bool checkForLocalAvatar = true;
    public GameObject XRDependencies;
    RealtimeAvatarManager realtime;

    private void Awake()
    {
        realtime = FindObjectOfType<RealtimeAvatarManager>();
    }

    private void Start()
    {
        StartCoroutine(StartListening());
    }

    IEnumerator StartListening()
    {
        while (realtime.localAvatar == null)
        {
            Debug.Log("LocalAvatarIsStillNull");
            yield return null;
        }
        Debug.Log("-------------------------------------LocalAvatarCreated, adding dependencies----------------------------------------");
        XRDependencies.transform.parent = realtime.localAvatar.transform;
        yield return null;

    }
}
