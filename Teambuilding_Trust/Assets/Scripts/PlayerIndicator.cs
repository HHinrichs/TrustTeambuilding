using Normal.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerIndicator : MonoBehaviour
{
    public IntSync kickPlayerValueIntSync;
    public List<Player> Players;

    public GameObject playerIndicatorPrefab;

    private RealtimeAvatarManager realtimeAvatarManager;

    private void Start()
    {
        realtimeAvatarManager = FindObjectOfType<RealtimeAvatarManager>();
        realtimeAvatarManager.avatarCreated += PlayerConnected;
        realtimeAvatarManager.avatarDestroyed += PlayerDisconnected;
    }

    public void PlayerConnected(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
    {
            InstantiatePlayer(avatar.realtimeView.ownerID);
    }


    public void PlayerDisconnected(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
    {
        foreach (Player player in Players)
        {
            if (player.PlayerNumber == avatar.realtimeView.ownerID)
            {
                Players.Remove(player);
                Destroy(player.gameObject);
                break;
            }
        }
    }

    public void InstantiatePlayer(int clientKeyValue)
    {
        GameObject playerPrefab = Instantiate(playerIndicatorPrefab,this.transform);

        Player player = playerPrefab.GetComponent<Player>();

        player.PlayerNumberString.text = clientKeyValue.ToString();

        player.PlayerNumber = clientKeyValue;

        player.kickButton.onClick.AddListener( () => KickPlayer(player.PlayerNumber) );

        Players.Add(player);
    }
    private void KickPlayer(int clientKeyValue)
    {
        kickPlayerValueIntSync.SetIntValue(clientKeyValue);
        //RealtimeAvatarManager rta = FindObjectOfType<RealtimeAvatarManager>();

        //foreach (KeyValuePair<int, RealtimeAvatar> avatar in rta.avatars)
        //{

        //    if(avatar.Key == clientKeyValue)

        //    Debug.Log(FindObjectOfType<Realtime>().clientID);

        //    Debug.Log(avatar.Key);
        //}
    }

}
