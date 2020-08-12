using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using System.Security.Policy;

public class ChangeAvatarAppearanceButton : MonoBehaviour
{
    private IntSync avatarAppearanceStateIntSync;
    private Room room;
    private RealtimeAvatarManager realtimeAvatarManager;
    private RealtimeView realtimeView;

    public void Start()
    {
        avatarAppearanceStateIntSync = GameObject.FindGameObjectWithTag("AvatarAppearanceState").GetComponent<IntSync>();
        if (avatarAppearanceStateIntSync == null)
            Debug.LogError("AvatarAppearanceState not found! This is a bug!");
        avatarAppearanceStateIntSync.intValueChanged += SetAvatarAppearances;

        // Create List of all Active avatars in scene;
        realtimeAvatarManager = FindObjectOfType<RealtimeAvatarManager>();
        realtimeAvatarManager.avatarCreated += PlayerConnected;
    }

    public void PlayerConnected(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
    {
        SetAvatarAppearances();
    }

    public void SetAvatarAppearanceToZero()
    {
        // Zero is non Ik
        avatarAppearanceStateIntSync.SetIntValue(0);
    }

    public void SetAvatarAppearanceToOne()
    {
        // One is IK
        avatarAppearanceStateIntSync.SetIntValue(1);
    }

    public void SetAvatarAppearances()
    {
        Debug.Log("AvatarAppearance Changed");
        if (GameManager.Instance.isServer)
        {
            int avatarAppearanceValue = avatarAppearanceStateIntSync.GetIntValue;
            GameObject[] avatars = GameObject.FindGameObjectsWithTag("PlayerAvatar");

            switch (avatarAppearanceValue)
            {
                case 0:

                    for (int i = 0; i < avatars.Length; ++i)
                    {
                        avatars[i].GetComponent<SetLocalAvatarPrefabForPlayers>().SetNonIkValues();
                    }

                    break;
                case 1:
                    for (int i = 0; i < avatars.Length; ++i)
                    {
                        avatars[i].GetComponent<SetLocalAvatarPrefabForPlayers>().SetIkValues();
                    }
                    break;
            }
        }

    }
}
