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


[RequireComponent(typeof(AudioSource))]
public class NetworkAudioReceiver : MonoBehaviour
{

    AudioSource aud;
    private int clipFreq = -1;
    private int clipChan = -1;
    private int clipLen = 441000;
    private int lastSamplePos = 0;


    void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.loop = true;
    }


    void Update()
    {
        UpdateAudioPlayer();
    }

    void UpdateAudioPlayer()
    {
        if (!aud.isPlaying)
        {
            return;
        }
        if (aud.timeSamples > lastSamplePos && aud.timeSamples - lastSamplePos < 10000)
        {
            aud.Pause();
        }
    }


    public void setAudioData(byte[] rawData)
    {
        float[] samples;
        int freq;
        int chan;
        AudioSerializer.Deserialize(rawData, out samples, out freq, out chan);
        if (freq != -1 && chan != -1)
        {

            if (clipFreq != freq || clipChan != chan)
            {
                clipFreq = freq;
                clipChan = chan;
                aud.clip = AudioClip.Create("RemoteAudio",
                    clipLen, clipChan, freq, false);
                lastSamplePos = 0;
                aud.timeSamples = 0;
            }

            aud.clip.SetData(samples, lastSamplePos);

            if (lastSamplePos > aud.timeSamples + 10000 ||
                (lastSamplePos < aud.timeSamples && lastSamplePos > 10000
                && aud.timeSamples < clipLen - 10000))
            {
                aud.timeSamples = lastSamplePos;
            }
            if (!aud.isPlaying)
            {
               // Debug.Log("BeginToPlayAudio!");
                aud.Play();
            }

            lastSamplePos += samples.Length;
            if (lastSamplePos >= clipLen)
                lastSamplePos -= clipLen;
        }



    }
}
