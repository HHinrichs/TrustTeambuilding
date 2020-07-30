using Normal.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerIndicator : MonoBehaviour
{
    public delegate void KickPlayerEvent();
    public event KickPlayerEvent kickPlayerEvent;
    public IntSync intSync;


    public List<Player> Players;

    public GameObject playerIndicatorPrefab;

    private RealtimeAvatarManager realtimeAvatarManager;

    private void Start()
    {
        realtimeAvatarManager = FindObjectOfType<RealtimeAvatarManager>();
        realtimeAvatarManager.newAvatarJoined += PlayerConnected;
        realtimeAvatarManager.avatarDisconnected += PlayerDisconnected;
    }

    public void PlayerConnected(int clientKeyValue)
    {
        InstantiatePlayer(clientKeyValue);
    }


    public void PlayerDisconnected(int clientKeyValue)
    {
        foreach(Player player in Players)
        {
            if (player.PlayerNumber == clientKeyValue)
            {
                Players.Remove(player);
                Destroy(player.gameObject);
            }
        }
    }

    public void InstantiatePlayer(int clientKeyValue)
    {
        GameObject playerPrefab = Instantiate(playerIndicatorPrefab);
        playerPrefab.transform.parent = this.transform;

        Player player = playerPrefab.GetComponent<Player>();

        player.PlayerNumberString.text = clientKeyValue.ToString();

        player.PlayerNumber = clientKeyValue;

        player.kickButton.onClick.AddListener( () => KickPlayer(player.PlayerNumber) );

        Players.Add(player);
    }
    private void KickPlayer(int clientKeyValue)
    {
        intSync.SetIntValue(clientKeyValue);
        kickPlayerEvent.Invoke();
        //RealtimeAvatarManager rta = FindObjectOfType<RealtimeAvatarManager>();

        //foreach (KeyValuePair<int, RealtimeAvatar> avatar in rta.avatars)
        //{

        //    if(avatar.Key == clientKeyValue)

        //    Debug.Log(FindObjectOfType<Realtime>().clientID);

        //    Debug.Log(avatar.Key);
        //}
    }

}
