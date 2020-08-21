﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Normal.Realtime;

public class ClientAudioSender : MonoBehaviour
{
    public int sendRate = 10;
    public bool sending = true;
    private AudioClip mic;
    private int lastRecSample = 0;
    private int pos;
    private int recFreq;


    void Start()
    {
        int minFreq;
        int maxFreq;
        Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);
        if (minFreq == 0) recFreq = 16000;
        else recFreq = minFreq;
        mic = Microphone.Start(null, true, 60, recFreq);

        StartCoroutine(sendingCoroutine());
    }

    IEnumerator sendingCoroutine()
    {
        Realtime realtime = FindObjectOfType<Realtime>();
        yield return new WaitUntil(() => realtime.connected == true);
        yield return new WaitUntil(() => realtime.room.connected == true);

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

            SendAudioViaNetwork(serialized);
            lastRecSample = pos;
        }
    }

    public void SendAudioViaNetwork(byte[] serialized)
    {
        int messageID = 2000;
        byte[] messageIDByteArray = BitConverter.GetBytes(messageID);

        List<byte> message = new List<byte>(messageIDByteArray);
        message.AddRange(serialized);

        FindObjectOfType<Realtime>().room.SendRPCMessage(message.ToArray(), false);
    }
}