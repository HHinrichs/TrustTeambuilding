using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedPlayers : MonoBehaviour
{
    public delegate void KickPlayer(int clientID);
    public event KickPlayer kickPlayer;

    public void Awake()
    {
        //FindObjectOfType<RealtimeAvatarManager>().newAvatarJoined += AvatarCreatedLog;
    }

    public void AvatarCreatedLog()
    {
        Debug.Log("Avatar CREATED!");
    }
    public void GetConnectedPlayers()
    {
        RealtimeAvatarManager rta = FindObjectOfType<RealtimeAvatarManager>();

        foreach(KeyValuePair<int,RealtimeAvatar> avatar in rta.avatars)
        {
            Debug.Log(FindObjectOfType<Realtime>().clientID);

            Debug.Log(avatar.Key);
        }
    }
}
