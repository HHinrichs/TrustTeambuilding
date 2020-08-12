using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using System.Security.Policy;

public class SetLocalAvatarPrefabForPlayers : MonoBehaviour
{
    public GameObject Head;
    public GameObject LeftHand;
    public GameObject RightHand;
    public GameObject AvatarMesh;

    private IntSync avatarAppearanceStateIntSync;
    private Room room;
    private RealtimeAvatarManager realtimeAvatarManager;
    private RealtimeView realtimeView;

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        Realtime realtime = FindObjectOfType<Realtime>();
        yield return new WaitUntil(() => realtime.connected == true);
        yield return new WaitUntil(() => realtime.room.connected == true);

        avatarAppearanceStateIntSync = GameObject.FindGameObjectWithTag("AvatarAppearanceState").GetComponent<IntSync>();
        if (avatarAppearanceStateIntSync == null)
            Debug.LogError("AvatarAppearanceState not found! This is a bug!");
        avatarAppearanceStateIntSync.intValueChanged += SetAvatarAppearances;

        realtimeView = GetComponent<RealtimeView>();
        if (realtimeView == null)
            Debug.LogError("realtimeView not found! This is a bug!");

        // Create List of all Active avatars in scene;
        realtimeAvatarManager = FindObjectOfType<RealtimeAvatarManager>();
        realtimeAvatarManager.avatarCreated += PlayerConnected;
    }

        public void PlayerConnected(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
    {
        SetAvatarAppearances();
    }

    public void SetAvatarAppearances()
    {
        if (realtimeView.isOwnedLocally || GameManager.Instance.isServer)
        {
            int avatarAppearanceValue = avatarAppearanceStateIntSync.GetIntValue;
            GameObject[] avatars = GameObject.FindGameObjectsWithTag("PlayerAvatar");

            switch (avatarAppearanceValue)
            {
                case 0:

                    for(int i = 0; i < avatars.Length; ++i)
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

    public void SetNonIkValues()
    {
        Head.SetActive(true);
        LeftHand.SetActive(true);
        RightHand.SetActive(true);
        AvatarMesh.SetActive(false);
    }

    public void SetIkValues()
    {
        Head.SetActive(false);
        LeftHand.SetActive(false);
        RightHand.SetActive(false);
        AvatarMesh.SetActive(true);
    }
}
