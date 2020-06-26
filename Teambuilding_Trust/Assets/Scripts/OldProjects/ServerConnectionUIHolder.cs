using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ServerConnectionUIHolder : MonoBehaviour
{

    public Image connectionImage;
    public Text connectionText;
    public Button disconnectButton;
    public Button connectBtn;
    public InputField IpInputField;
    //public TMP_InputField textMeshProInputField;

    //public InputField IpInputField;

    public static ServerConnectionUIHolder Instance;
    public void Awake()
    {
        Instance = this;
    }

    //private void Start()
    //{
    //    if (!NetworkClientManager.Instance.enableConnection)
    //    {
    //        connectBtn.interactable = false;
    //        disconnectButton.interactable = false;
    //    }

    //    if (!NetworkClientManager.Instance.enableSelfNameSet)
    //        textMeshProInputField.interactable = false;

    //    if (!NetworkClientManager.Instance.enableSelfIPSet)
    //        IpInputField.interactable = false;
    //}

    public void OnConnectedToMQTTServer()
    {
        connectionImage.color = Color.green;
        connectionText.text = "Connected to MQTT server";

        connectBtn.GetComponentInChildren<Text>().text = "CONNECTED";
        connectBtn.interactable = false;
        disconnectButton.interactable = true;

        IpInputField.interactable = false;

    }

    public void OnDisconnectToMQTTServer()
    {
        connectionImage.color = Color.red;
        connectionText.text = "Disconnected to MQTT server";

        connectBtn.GetComponentInChildren<Text>().text = "Connect to Server";
        connectBtn.interactable = true;
        disconnectButton.interactable = false;

        IpInputField.interactable = true;

    }
}
