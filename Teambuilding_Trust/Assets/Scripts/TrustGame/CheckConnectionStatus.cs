using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using TMPro;
using UnityEngine.UI;

public class CheckConnectionStatus : MonoBehaviour
{
    Realtime realtime;
    TextMeshProUGUI connectionStatusText;
    Image backgroundImage;
    private void Start()
    {
        realtime = FindObjectOfType<Realtime>();
        if (realtime == null)
            Debug.Log("realtime is null!");

        connectionStatusText = GetComponent<TextMeshProUGUI>();
        if (connectionStatusText == null)
            Debug.Log("connectionStatusText is null!");

        backgroundImage = GetComponentInParent<Image>();
        realtime.didConnectToRoom += ChangeDisplay;
    }
    public void ChangeDisplay(Realtime realtime)
    {
        connectionStatusText.text = "IsConnected : "+realtime.connected.ToString();
        if (realtime.connected)
            backgroundImage.color = Color.green;
        else
            backgroundImage.color = Color.red;

    }

    public void OnDisable()
    {
        realtime.didConnectToRoom -= ChangeDisplay;
    }
}
