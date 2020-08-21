using System.Collections;
using System.Text;
using System;
using System.Collections.Generic;
using UnityEngine;
using uPLibrary;
using uPLibrary.Networking;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.IO;

namespace MQTT
{
    public class MQTTConnector : MonoBehaviour
    {
        //ServerIP
        public string IP = "127.0.0.1";

        //These are the relevant Topics

        //Prefix for NetWorkMassage for simple string based communication
        public const string PREFIX_NWM = "nwm"; 
        public const string TOPIC_TRANSFORM = "transform"; //Used for syncing transform data
        public const string TOPIC_AVATAR = "avatar"; //Only for avatar data
        public const string TOPIC_EVENT = "event"; //Used for alle single events, that are sent once.
        public const string TOPIC_PINGPONG = "pingpong";//Only the sender received the message.

        //Topics for raw based data, like the microphone
        public const string PREFIX_RAW = "raw";
        public const string TOPIC_MICROPHONE = "mic";


        public bool isInitialized = false;

        //Clients unique and randomized UUID
        public string clientID;

        #region Singleton
        public static MQTTConnector Instance;
        public void Awake()
        {
            Instance = this;
        }
        #endregion




        MqttClient client;
        public void connect()
        {
            clientID = "" + System.Guid.NewGuid();
            client = new MqttClient(IP, 1883, false, null);

            client.MqttMsgPublishReceived += mqttMessageReceived;


            string[] topics = new string[] {
                PREFIX_NWM+"/"+TOPIC_TRANSFORM,
                PREFIX_NWM+"/"+TOPIC_AVATAR,
                PREFIX_NWM+"/"+TOPIC_EVENT,
                PREFIX_NWM+"/"+TOPIC_PINGPONG,
                PREFIX_RAW+"/"+TOPIC_MICROPHONE
            };

            byte[] qos = new byte[] {
                MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
                MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE
            };
            client.Connect(clientID);
            client.Subscribe(topics, qos);
            Debug.Log("MQTT Connection to" + IP);
            isInitialized = true;

            //Connected To Server
            publishPingPongEvent(this.name, "OnConnectedToServerPingPong", "");
        }


        /**
         * <summary>Callback for all MQTT messages. The prefix are determined and the accordingly further processed</summary>
         * 
         * */
        private void mqttMessageReceived(object sender, MqttMsgPublishEventArgs e)
        {
            byte[] rawData = e.Message;
            using (MemoryStream stream = new MemoryStream(e.Message))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    string senderID = reader.ReadString();
                    int byteCount = reader.ReadInt32();
                    byte[] messageBytes = reader.ReadBytes(byteCount);


                    string prefix = e.Topic.Split(new char[] { '/' })[0];
                    string topic = e.Topic.Split(new char[] { '/' })[1];

                    if (prefix == PREFIX_NWM)
                    {
                        string rawJsonNetworkMessage = Encoding.UTF8.GetString(messageBytes);
                        NetworkMessage msg = JsonUtility.FromJson<NetworkMessage>(rawJsonNetworkMessage);

                        switch (topic)
                        {
                            case TOPIC_TRANSFORM:
                                if (msg.senderID != this.clientID)
                                    onNetworkTransformReceived(msg);

                                break;

                            case TOPIC_AVATAR:
                                if (msg.senderID != this.clientID)
                                    onNetworkAvatarTransformReceived(msg);
                                break;

                            case TOPIC_EVENT:
                                if (msg.senderID != this.clientID)
                                    onEventReceived(msg);
                                break;

                            case TOPIC_PINGPONG:
                                if (msg.senderID == this.clientID)
                                    onEventReceived(msg);
                                break;
                        }
                    }
                    else if (prefix == PREFIX_RAW)
                    {
                        switch (topic)
                        {
                            case TOPIC_MICROPHONE:
                                if (senderID != this.clientID)
                                    onMicrophoneDataReceived(senderID, messageBytes);
                                break;
                        }
                    }


                }
            }
        }

        public void OnConnectedToServerPingPong()
        {
            AudioManager.Instance.playConnectedToServerSound();
            if(ConnectionUIHolder.Instance != null)
                ConnectionUIHolder.Instance.OnConnectedToMQTTServer();
        }


        #region Receive

        private void onNetworkTransformReceived(NetworkMessage msg)
        {
            TransformQueue.Enqueue(() =>
            {
                GameObject g = GameObject.Find(msg.targetObject);
                if (g != null)
                {
                    g.SendMessage(msg.targetMethod, msg.parameters);
                }
                else
                {

                    GameObject newG = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    newG.name = msg.targetObject;
                    newG.AddComponent<SimpleNetworkTransform>();

                }
            });
        }

        private void onNetworkAvatarTransformReceived(NetworkMessage msg)
        {
            TransformQueue.Enqueue(() =>
            {
                AvatarVisualizer.Instance.updateOrCreateClientAvatar(msg);
            });
        }

        private void onEventReceived(NetworkMessage msg)
        {
            EventQueue.Enqueue(() =>
            {
                GameObject g = GameObject.Find(msg.targetObject);
                if (g != null)
                {
                    g.SendMessage(msg.targetMethod, msg.parameters);
                }
            });
        }

        private void onMicrophoneDataReceived(string senderID, byte[] rawData)
        {
            string audioObjectNaming = "AudioReceiver_" + senderID;
            RawQueue.Enqueue(() =>
            {
                GameObject audioObject = GameObject.Find(audioObjectNaming);
                if (audioObject != null)
                {
                    audioObject.GetComponent<NetworkAudioReceiver>().setAudioData(rawData);
                }
                else
                {

                    GameObject newG = new GameObject();
                    newG.name = audioObjectNaming;
                    newG.AddComponent<NetworkAudioReceiver>();

                }
            });
        }

        public Queue<Action> EventQueue = new Queue<Action>();
        public Queue<Action> TransformQueue = new Queue<Action>();
        public Queue<Action> RawQueue = new Queue<Action>();
        void Update()
        {
            while (TransformQueue.Count > 0)
            {
                TransformQueue.Dequeue().Invoke();
            }
            while (RawQueue.Count > 0)
            {
                RawQueue.Dequeue().Invoke();
            }

            if (EventQueue.Count > 0)
            {
                EventQueue.Dequeue().Invoke();
            }
        }
        #endregion


        #region Publishing

        public void publishNetworkTransform(string targetObject, string parameters)
        {
            if (!isInitialized)
                return;

            NetworkMessage msg = new NetworkMessage();
            msg.senderID = this.clientID;
            msg.targetObject = targetObject;
            msg.targetMethod = "updateTransform";
            msg.parameters = parameters;

            byte[] rawData = serializeData(msg);

            client.Publish(PREFIX_NWM + "/" + TOPIC_TRANSFORM, rawData, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
        }


        public void publishAvatarTransformForAllClients(string targetObject, string parameters)
        {
            if (!isInitialized)
                return;

            NetworkMessage msg = new NetworkMessage();
            msg.senderID = this.clientID;
            msg.targetObject = targetObject;
            msg.targetMethod = "updateTransform";
            msg.parameters = parameters;

            byte[] rawData = serializeData(msg);

            client.Publish(PREFIX_NWM + "/" + TOPIC_AVATAR, rawData, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
        }

        public void publishAvatarTransformForAllClients(string senderID, string targetObject, string parameters)
        {
            if (!isInitialized)
                return;

            NetworkMessage msg = new NetworkMessage();
            msg.senderID = senderID;
            msg.targetObject = targetObject;
            msg.targetMethod = "updateTransform";
            msg.parameters = parameters;

            byte[] rawData = serializeData(msg);

            client.Publish(PREFIX_NWM + "/" + TOPIC_AVATAR, rawData, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
        }

        public void publishEventForOtherClients(string targetObject, string targetMethod, string parameters)
        {
            if (!isInitialized)
                return;

            NetworkMessage msg = new NetworkMessage();
            msg.senderID = this.clientID;
            msg.targetObject = targetObject;
            msg.targetMethod = targetMethod;
            msg.parameters = parameters;

            byte[] rawData = serializeData(msg);

            client.Publish(PREFIX_NWM + "/" + TOPIC_EVENT, rawData, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void publishEventOnAll(string targetObject, string targetMethod, string parameters)
        {
            if (!isInitialized)
                return;

            NetworkMessage msg = new NetworkMessage();
            msg.senderID = "";
            msg.targetObject = targetObject;
            msg.targetMethod = targetMethod;
            msg.parameters = parameters;

            byte[] rawData = serializeData(msg);

            client.Publish(PREFIX_NWM + "/" + TOPIC_EVENT, rawData, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void publishEventOnClientObject(string targetObject, string targetMethod, string parameters)
        {
            if (!isInitialized)
                return;

            NetworkMessage msg = new NetworkMessage();
            msg.senderID = this.clientID;
            msg.targetObject = targetObject + "_" + this.clientID;
            msg.targetMethod = targetMethod;
            msg.parameters = parameters;

            byte[] rawData = serializeData(msg);

            client.Publish(PREFIX_NWM + "/" + TOPIC_EVENT, rawData, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void publishEventOnSepcificClient(string targetClientId, string targetObject, string targetMethod, string parameters)
        {
            if (!isInitialized)
                return;

            NetworkMessage msg = new NetworkMessage();
            msg.senderID = targetClientId;
            msg.targetObject = targetObject;
            msg.targetMethod = targetMethod;
            msg.parameters = parameters;

            byte[] rawData = serializeData(msg);

            client.Publish(PREFIX_NWM + "/" + TOPIC_PINGPONG, rawData, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }


        public void publishPingPongEvent(string targetObject, string targetMethod, string parameters)
        {
            if (!isInitialized)
                return;

            NetworkMessage msg = new NetworkMessage();
            msg.senderID = this.clientID;
            msg.targetObject = targetObject;
            msg.targetMethod = targetMethod;
            msg.parameters = parameters;

            byte[] rawData = serializeData(msg);

            client.Publish(PREFIX_NWM + "/" + TOPIC_PINGPONG, rawData, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }


        private byte[] serializeData(NetworkMessage msg)
        {
            byte[] data = null;
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(this.clientID);

                    string json = JsonUtility.ToJson(msg);
                    byte[] rawMsgBytes = Encoding.ASCII.GetBytes(json);

                    writer.Write(rawMsgBytes.Length);
                    writer.Write(rawMsgBytes);

                    stream.Position = 0;
                    data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);

                }
            }
            return data;
        }

        private byte[] serializeData(byte[] rawData)
        {
            byte[] data = null;
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(this.clientID);

                    writer.Write(rawData.Length);
                    writer.Write(rawData);

                    stream.Position = 0;
                    data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);

                }
            }
            return data;
        }


        public void publishRawMicData(byte[] rawData)
        {
            if (!isInitialized)
                return;

            byte[] data = serializeData(rawData);
            client.Publish(PREFIX_RAW + "/" + TOPIC_MICROPHONE, data, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
        }

        #endregion

        private void OnApplicationQuit()
        {
            // client.Unsubscribe(topics);
            if (!isInitialized)
                return;

            client.Disconnect();
        }


    }
    [System.Serializable]
    public class NetworkMessage
    {
        public string senderID;
        public string targetObject;
        public string targetMethod;
        public string parameters;

    }

    [System.Serializable]
    public class NetworkRawMessage
    {
        public byte[] rawData;
    }

}
