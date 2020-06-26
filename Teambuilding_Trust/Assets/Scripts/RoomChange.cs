using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal;
using Normal.Realtime;

public class RoomChange : MonoBehaviour
{
    public Realtime realtime;

    public void ConnectToRoom(string roomName)
    {
        realtime.Disconnect();
        realtime.Connect(roomName);
    }
}
