using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConnectionAudio : MonoBehaviour
{
    private RealtimeAvatarManager realtimeAvatarManager;
    private AudioSource audioSource;

    [SerializeField] AudioClip ConnectionClip;
    [SerializeField] AudioClip DisconnectionClip;
    [SerializeField] AudioClip Connected;
    [SerializeField] AudioClip Disconnected;

    private void Awake()
    {
        realtimeAvatarManager = FindObjectOfType<RealtimeAvatarManager>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        realtimeAvatarManager.avatarCreated += PlayConnectionSound;
        realtimeAvatarManager.avatarDestroyed += PlayDisconnectionSound;
    }

    private void OnDestroy()
    {
        realtimeAvatarManager.avatarCreated -= PlayConnectionSound;
        realtimeAvatarManager.avatarDestroyed -= PlayDisconnectionSound;
    }

    public void PlayConnectionSound(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
    {
        if (isLocalAvatar)
        {
            audioSource.PlayOneShot(Connected);
            return;
        }

        audioSource.PlayOneShot(ConnectionClip);
    }

    public void PlayDisconnectionSound(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
    {
        if (isLocalAvatar)
        {
            audioSource.PlayOneShot(Connected);
            return;
        }

        audioSource.PlayOneShot(DisconnectionClip);
    }
}
