using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Normal.Realtime;

public class ClientAudioSender : MonoBehaviour
{
    public int sendRate = 15;
    public bool sending = true;
    private AudioClip mic;
    private int lastRecSample = 0;
    private int pos;
    private int recFreq;
    private Realtime realtime;


    void Start()
    {

        StartCoroutine(sendingCoroutine());
    }

    IEnumerator sendingCoroutine()
    {
        realtime = FindObjectOfType<Realtime>();
        yield return new WaitUntil(() => realtime.connected == true);
        yield return new WaitUntil(() => realtime.room.connected == true);

        int minFreq;
        int maxFreq;
        Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);
        if (minFreq == 0) 
            recFreq = 16000;
        else 
            recFreq = minFreq;
        mic = Microphone.Start(null, true, 10, recFreq);
        while (sending)
        {
            SendMicSamples();
            yield return new WaitForSecondsRealtime(1f / sendRate);
        }
    }

    void SendMicSamples()
    {
        pos = Microphone.GetPosition(null);
        int diff = pos - lastRecSample;
        if (diff < 0) diff = pos;
        if (diff > 0)
        {
            float[] samples = new float[diff * mic.channels];
            mic.GetData(samples, lastRecSample);
            byte[] serialized = AudioSerializer.Serialize(samples, recFreq, mic.channels);

            
            SendAudioViaNetwork(serializeData(2000,serialized));
            lastRecSample = pos;
        }
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

    public void SendAudioViaNetwork(byte[] serialized)
    {
        //int messageID = 2000;
        //byte[] messageIDByteArray = BitConverter.GetBytes(messageID);
        //List<byte> message = new List<byte>(messageIDByteArray);
        //message.AddRange(serialized);
        //realtime.room.SendRPCMessage(message.ToArray(), false);
        Debug.Log("RCP Audio Message send via Network from Client!");

        //byte[] messageIDByteArray = BitConverter.GetBytes(serialized);
        //List<byte> message = new List<byte>(messageIDByteArray);
        //realtime.room.SendRPCMessage(serialized, false);
        //Debug.Log("RCP Audio Message send via Network from Client!");

        realtime.room.SendRPCMessage(serialized, false);
    }
}
