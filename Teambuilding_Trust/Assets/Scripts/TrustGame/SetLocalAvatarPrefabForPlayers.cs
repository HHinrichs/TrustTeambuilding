using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class SetLocalAvatarPrefabForPlayers : MonoBehaviour
{
    public RealtimeAvatarManager ServerAvatarManager;
    public GameObject NoBodyAvatar;
    public GameObject BodyAvatar;
    public void Start()
    {
        if (ServerAvatarManager == null)
            ServerAvatarManager = FindObjectOfType<RealtimeAvatarManager>();
    }

    public void ChangeAvatarAppearanceToBodyAvatar()
    {
        ServerAvatarManager.localAvatarPrefab = BodyAvatar;
    }

    public void ChangeAvatarAppearanceToNoBodyAvatar()
    {
        ServerAvatarManager.localAvatarPrefab = NoBodyAvatar;

    }
}
