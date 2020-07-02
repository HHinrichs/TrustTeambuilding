using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class TeleportationAnchorNetworkable : TeleportationAnchor
{
    Realtime realtime;

    protected override void Awake()
    {
        base.Awake();
        realtime = FindObjectOfType<Realtime>();
        if (realtime == null)
            Debug.LogError("No Realtime found in Scene!");
        realtime.didConnectToRoom += SetTeleportationProvider;
    }

    private void SetTeleportationProvider(Realtime realtime)
    {
        if (!realtime.connected)
        {
            Debug.LogError("Realtime is not connected!");
            return;
        }

        StartCoroutine(SetTeleportationProviderCoroutine());
    }

    IEnumerator SetTeleportationProviderCoroutine()
    {
        // Unforunately we need to wait here till the Avatar is Instantiated to get the teleportation Provider ...
        yield return new WaitForSeconds(0.5f);

        teleportationProvider = FindObjectOfType<TeleportationProvider>();
        if (teleportationProvider == null)
            Debug.LogError("No TeleportationProvider found in Scene!");

    }


}
