using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using System.Security.Policy;

public class ChangeAvatarAppearanceButton : MonoBehaviour
{
    public IntSync avatarAppearanceStateIntSync;
    private Room room;
    private RealtimeAvatarManager realtimeAvatarManager;
    private RealtimeView realtimeView;
    private bool avatarIsIK;
    public bool AvatarIsIK { get { return avatarIsIK; } set { avatarIsIK = value; } }

    private static ChangeAvatarAppearanceButton _Instance;
    public static ChangeAvatarAppearanceButton Instance { get { return _Instance; } }
    bool isQuitting = false;

    private void Awake()
    {
        if(_Instance != this && _Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _Instance = this;
        }
        Application.wantsToQuit += (() => isQuitting = true);
    }
    public void Start()
    {
        avatarAppearanceStateIntSync = GameObject.FindGameObjectWithTag("AvatarAppearanceState").GetComponent<IntSync>();
        if (avatarAppearanceStateIntSync == null)
            Debug.LogError("AvatarAppearanceState not found! This is a bug!");
        avatarAppearanceStateIntSync.intValueChanged += SetAvatarAppearances;

        // Create List of all Active avatars in scene;
        realtimeAvatarManager = FindObjectOfType<RealtimeAvatarManager>();
        realtimeAvatarManager.avatarCreated += PlayerConnected;
        // Because we always start with NON IK Avatar;
        AvatarIsIK = false;
    }

    private void OnApplicationQuit()
    {
        avatarAppearanceStateIntSync.intValueChanged -= SetAvatarAppearances;
        realtimeAvatarManager.avatarCreated -= PlayerConnected;
    }

    private void OnDestroy()
    {
        avatarAppearanceStateIntSync.intValueChanged -= SetAvatarAppearances;
        realtimeAvatarManager.avatarCreated -= PlayerConnected;

    }
   
    public void PlayerConnected(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
    {
        SetAvatarAppearances();
    }

    public void SetAvatarAppearanceToZero()
    {
        // Zero is non Ik
        avatarAppearanceStateIntSync.SetIntValue(0);
        AvatarIsIK = false;
    }

    public void SetAvatarAppearanceToOne()
    {
        // One is IK
        avatarAppearanceStateIntSync.SetIntValue(1);
        AvatarIsIK = true;
    }

    public void SetAvatarAppearances()
    {
        if (isQuitting)
            return;

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
