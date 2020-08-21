/*
This file is part of the OpenIMPRESS project.
OpenIMPRESS is free software: you can redistribute it and/or modify
it under the terms of the Lesser GNU Lesser General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
OpenIMPRESS is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.
You should have received a copy of the GNU Lesser General Public License
along with OpenIMPRESS. If not, see <https://www.gnu.org/licenses/>.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using MQTT;

public class MicrophoneNetworkSender : MonoBehaviour
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

            MQTTConnector.Instance.publishRawMicData(serialized);

            lastRecSample = pos;
        }
    }
}


public static class AudioSerializer
{

    public static byte[] Serialize(float[] samples, int freq, int chan)
    {
        byte[] data = null;
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(freq);
                writer.Write(chan);
                writer.Write(samples.Length);
                foreach (float sample in samples)
                {
                    writer.Write((Int16)(sample * 32767));
                }

                stream.Position = 0;
                data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
            }
        }

        return data;
    }

    public static void Deserialize(byte[] data, out float[] samples, out int freq, out int chan)
    {
        samples = new float[0];
        freq = -1;
        chan = -1;

        using (MemoryStream stream = new MemoryStream(data))
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                freq = reader.ReadInt32();
                chan = reader.ReadInt32();
                int len = reader.ReadInt32();
                samples = new float[len];
                for (int i = 0; i < len; i++)
                {
                    samples[i] = reader.ReadInt16() / 32767f;
                }
            }
        }
    }

}
