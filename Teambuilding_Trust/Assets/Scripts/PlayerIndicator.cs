using Normal.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

public class PlayerIndicator : MonoBehaviour
{
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

        player.kickButton.onClick.AddListener( () => KickPlayer(serializeData(1000, ConvertIntToByte(player.PlayerNumber)) ) );

        Players.Add(player);
    }
    private void KickPlayer(byte[] serializedData)
    {
        //int messageID = 1000;
        //byte[] messageIDByteArray = BitConverter.GetBytes(messageID);
        //byte[] clientKeyValueByteArray = BitConverter.GetBytes(clientKeyValue);

        //List<byte> message = new List<byte>(messageIDByteArray);
        //message.AddRange(clientKeyValueByteArray);

        FindObjectOfType<Realtime>().room.SendRPCMessage(serializedData, true);

    }

    private byte[] serializeData(int messageID, byte[] rawData)
    {
        byte[] data = null;
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(messageID);
                writer.Write(rawData.Length);
                writer.Write(rawData);

                stream.Position = 0;
                data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
            }
        }
        return data;
    }

    private byte[] ConvertIntToByte(int value)
    {
        return BitConverter.GetBytes(value);
    }

}
