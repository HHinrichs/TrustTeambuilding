using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;
using Normal.Realtime;
using JetBrains.Annotations;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using System.IO;
using CSVInteractions;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private static GameManager _Instance = null;

    public static GameManager Instance { get { return _Instance; } }

    public delegate void ResetAllCalled();
    public event ResetAllCalled resetAllCalled;
    [Header("Allgemeine Einstellungen")]
    public bool isServer;
    public bool isClient;
    [SerializeField]
    public enum ControllerMode
    {
        Oculus,
        HTC,
        Both
    }
    public ControllerMode controllerMode = ControllerMode.Both;
    [SerializeField] int NumbersOfParticipatingPlayers;
    [SerializeField] GameObject PlayerSpawnPositions;
    [SerializeField] List<PodestManager> Podests;
    [SerializeField] int CountDownToStartInSeconds;
    [SerializeField] TextMeshPro GameStartTimerTMP;
    [SerializeField] VisualEffect Sphere;
    [Header("ServerSettings")]
    public float OverallGameTime;
    //[SerializeField] NetworkAudioReceiver NetworkAudioReceiver;
    public RoundRules RoundRules;
    public int RoundNumberToStartWith;
    private Queue<Action> RawQueue = new Queue<Action>();
    private Queue<Action> ReliableQueue = new Queue<Action>();
    [Header("Networking Links")]
    public BoolSync Podest1BoolSync;
    public BoolSync Podest2BoolSync;
    public BoolSync Podest3BoolSync;
    public BoolSync StartBoolSync;
    public BoolSync RestartBoolSync;
    public IntSync NetworkPlayerPositions;
    public BoolSync ReadyForNextRoundBoolSync;
    public IntSync RoundNumberToStartWithIntSync;
    [Space(25)]
    [Header("GameInformations")]
    public bool gameIsRunning = false;
    [SerializeField] int round = 0;
    [SerializeField] float timeSinceGameStart = 0f;
    [SerializeField] float timeSinceRoundStarts = 0f;
    [SerializeField] bool readyForNextRound = false;
    [SerializeField] bool countdownToStartIsActive = false;
    [SerializeField] bool nextRoundIsBootingUp = false;

    [SerializeField] string theTime;
    [SerializeField] string theDate;
    private PodestManager Player1;
    private PodestManager Player2;
    private PodestManager CurrentLeader;
    // Coroutines
    private Coroutine gameTimeCounterCoroutine = null;
    private Coroutine startCountdownToStartCoroutine = null;
    private Coroutine roundTimeCounterCoroutine = null;
    bool isQuitting = false;

    public UnityEvent unityEvent;
    Dictionary<int, NetworkAudioReceiver> NetworkAudioDictionary = new Dictionary<int, NetworkAudioReceiver>();
    public Dictionary<int, NetworkAudioReceiver> GetNetworkAudioDictionary { get { return NetworkAudioDictionary; } }

    //Getter Setter
    public PodestManager GetPlayer1 { get { return Player1; } }
    public PodestManager GetPlayer2 { get { return Player2; } }
    public PodestManager GetCurrentLeader { get { return CurrentLeader; } }
    public int GetRound { get { return round; } }
    public float GetTimeSinceGameStart { get { return timeSinceGameStart; } }

    public int GetNumbersOfParticipatingPlayers { get { return NumbersOfParticipatingPlayers; } }

    public ControllerMode GetControllerMode { get { return controllerMode; } }
    private void Awake()
    {
        if(_Instance != null && _Instance != this)
        {
            Destroy(_Instance);
        }
        else
        {
            _Instance = this;
        }

        // DISABLE THE REALTIME AVATAR MANAGER FOR THE SERVER SO THAT IT DOES NOT SPAWN ANY AVATARAS
        if (isServer)
            FindObjectOfType<RealtimeAvatarManager>().localAvatarPrefab = null;
    }

    public void QuitGame()
    {
        StartBoolSync.boolValueChanged -= StartGame;
        RestartBoolSync.boolValueChanged -= ResetAll;
        ChangeAvatarAppearanceButton.Instance.avatarAppearanceStateIntSync.intValueChanged -= ChangeAvatarAppearanceButton.Instance.SetAvatarAppearances;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    private void Start()
    {
        Application.wantsToQuit += ( ()=> isQuitting = true);
        Application.quitting += (() => isQuitting = true);
        EditorApplication.wantsToQuit += (() => isQuitting = true);
        EditorApplication.quitting += (() => isQuitting = true);
        StartCoroutine(LateStart(5));
    }
    IEnumerator LateStart(float waitTime)
    {
        Realtime realtime = FindObjectOfType<Realtime>();
        yield return new WaitUntil(() => realtime.connected == true);
        yield return new WaitUntil(() => realtime.room.connected == true);

        if (isServer)
        {
            if (OverallGameTime == 0)
                OverallGameTime = 600f;

            SetPlayerNetworkPositions();
            realtime.room.rpcMessageReceived += ClientRCPMessageReceived;
        }
        if (isClient)
            realtime.room.rpcMessageReceived += ServerRCPMessageReceived;

        Debug.Log("LateStartSuc!");
        StartBoolSync.boolValueChanged += StartGame;
        RestartBoolSync.boolValueChanged += ResetAll;

    }

    void OnApplicationQuit()
    {
        Debug.Log("Unsubscribing at OnApplicationQuit!()");
        StartBoolSync.boolValueChanged -= StartGame;
        RestartBoolSync.boolValueChanged -= ResetAll;
    }

    private void OnDestroy()
    {
        Debug.Log("Unsubscribing at OnDestroy!()");

        StartBoolSync.boolValueChanged -= StartGame;
        RestartBoolSync.boolValueChanged -= ResetAll;
    }

    private void Update()
    {
        HandleNetworkMessages();

        if (!gameIsRunning)
            return;

        if (countdownToStartIsActive)
            return;

        CheckForNextRound();

        if (readyForNextRound && !nextRoundIsBootingUp)
            StartNextRound();
    }

    public void HandleNetworkMessages()
    {
        while (RawQueue.Count > 0)
        {
            RawQueue.Dequeue().Invoke();
        }

        //Handle ReliableRCP Messages
        while (ReliableQueue.Count > 0)
        {
            ReliableQueue.Dequeue().Invoke();
        }
    }
    public void ServerRCPMessageReceived(Room room, byte[] data, bool reliable)
    {
        Debug.Log("RCPMessageReceived from Server");
        int messageID;
        int clientID;
        int byteCount;
        byte[] messageBytes;
        using (MemoryStream stream = new MemoryStream(data))
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {

                messageID = reader.ReadInt32();
                switch (messageID)
                {
                    // Kick Player Message
                    case 1000:
                        byteCount = reader.ReadInt32();
                        messageBytes = reader.ReadBytes(byteCount);

                        Debug.Log("Kick Message from Server received!");
                        ReliableQueue.Enqueue(() =>
                        {
                            Realtime realtime = FindObjectOfType<Realtime>();
                            if (realtime.clientID == BitConverter.ToInt32(messageBytes, 0))
                            {
                                realtime.Disconnect();
                                Debug.Log("Player " + realtime.clientID + " kicked from the Server");
                            }
                        });
                        break;

                    //ClientAudioStreamReceived Message Received
                    case 3000:
                        clientID = reader.ReadInt32();
                        byteCount = reader.ReadInt32();
                        messageBytes = reader.ReadBytes(byteCount);

                        Debug.Log("Audio Stream from Server Received!");
                        string NetworkAudioObjectName = "SpectatorAudioSource" + clientID;
                        
                        RawQueue.Enqueue(() =>
                        {
                            if (NetworkAudioDictionary.ContainsKey(clientID))
                            {
                                NetworkAudioReceiver NAR = NetworkAudioDictionary[clientID];
                                if (NAR.aud == null)
                                {
                                    Debug.Log("Returing due to no AudioSource created yet!");
                                    return;
                                }
                                NAR.setAudioData(messageBytes);
                            }
                            else
                            {
                                Debug.Log("No AudioSource found! Creating new one!");
                                GameObject NewlyCreatedNetworkAudioReceiver = new GameObject();
                                NewlyCreatedNetworkAudioReceiver.name = NetworkAudioObjectName;
                                NetworkAudioReceiver nar = NewlyCreatedNetworkAudioReceiver.AddComponent<NetworkAudioReceiver>();
                                NetworkAudioDictionary.Add(clientID, nar);
                            }
                        });
                        break;
                    case 9999:
                        Debug.Log("PlayerLeaderValueMessagReceived");
                        int leaderEnqueue = reader.ReadInt32();
                        int player1Enqueue = reader.ReadInt32();
                        int player2Enqueue = reader.ReadInt32();
                        ReliableQueue.Enqueue(() =>
                        {
                            leader = leaderEnqueue;
                            player1 = player1Enqueue;
                            player2 = player2Enqueue;
                            leaderAndPlayerValuesInitializedByServer = true;
                        });
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public void ClientRCPMessageReceived(Room room, byte[] data, bool reliable)
    {
        Debug.Log("RCP Message received from Client");
        int messageID;
        int clientID;
        int byteCount;
        byte[] messageBytes;

        using (MemoryStream stream = new MemoryStream(data))
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                messageID = reader.ReadInt32();         
                switch (messageID)
                {
                    //ClientAudioStreamReceived Message Received
                    case 2000:
                        clientID = reader.ReadInt32();
                        byteCount = reader.ReadInt32();
                        messageBytes = reader.ReadBytes(byteCount);

                        Debug.Log("Audio Stream from Client Received!");
                        string NetworkAudioObjectName = "ClientAudioSourceFor_" + clientID;

                        RawQueue.Enqueue(() =>
                        {
                            if (NetworkAudioDictionary.ContainsKey(clientID))
                            {
                                NetworkAudioReceiver NAR = NetworkAudioDictionary[clientID];
                                if(NAR.aud == null)
                                {
                                    Debug.Log("Returing due to no AudioSource created yet!");
                                    return;
                                }
                                NAR.setAudioData(messageBytes);
                            }
                            else
                            {
                                Debug.Log("No AudioSource found! Creating new one!");
                                GameObject NewlyCreatedNetworkAudioReceiver = new GameObject();
                                NewlyCreatedNetworkAudioReceiver.name = NetworkAudioObjectName;
                                NetworkAudioReceiver nar = NewlyCreatedNetworkAudioReceiver.AddComponent<NetworkAudioReceiver>();
                                NetworkAudioDictionary.Add(clientID, nar);
                            }
                        });
                        break;

                    default:
                        break;
                }
            }
        }
    }
    private byte[] getSubPartOfByteArray(byte[] data, int start, int length)
    {
        byte[] subPart = new byte[length];
        int index = 0;
        for(int i = start; i < start+length; ++i)
        {
            subPart[index] = data[i];
            index++;
        }
        return subPart;
    }

    // Networking
    public void SetPlayerNetworkPositions()
    {
        StartCoroutine(SetPlayerNetworkPositionCoroutine());
    }

    IEnumerator SetPlayerNetworkPositionCoroutine()
    {
        while (!NetworkPlayerPositions.realtimeView.isOwnedLocally)
        {
            NetworkPlayerPositions.realtimeView.RequestOwnership();
            // Waits one second to get Updated if the ownership is still mine!
            yield return new WaitForSeconds(0.5f);
        }
        int playerNetworkPositionInt = 0;
        for (int i = 0; i < NumbersOfParticipatingPlayers; ++i)
        {
            playerNetworkPositionInt = IntToBoolean.SetBitTo1(playerNetworkPositionInt, i);
        }
        NetworkPlayerPositions.SetIntValue(playerNetworkPositionInt);

        NetworkPlayerPositions.realtimeView.ClearOwnership();
    }

    public void StartGame()
    {
        if (isQuitting)
            return;

        Debug.Log("StartCalled!");

        round = RoundNumberToStartWithIntSync.GetIntValue;

        if (NumbersOfParticipatingPlayers == 1)
            Debug.LogError("At least 2 and the most 3 players are required!");
        if(round == 0)
        {
            Debug.LogError("Round is Set to 0! This is not correct while starting the Game! Set round at least to 1!");
            return;
        }

        ResetAllFirstTime();
        gameIsRunning = true;
        SetGameRulesForPlayers();

        // WRITE START DATA TO CSV
        if (isServer)
        {
            theDate = System.DateTime.Now.ToString("MM/dd/yyyy");
            theTime = System.DateTime.Now.ToString("hh:mm:ss"); 
            CSVWriter.addStartRoundRecord();
        }

        StartCoroutine(SequencingStart());

    }
    private void StartNextRound()
    {
        Debug.Log("Booting up next round ....");
        nextRoundIsBootingUp = true;
        ClearForNextRound();
        round++;
        StartCoroutine(SequencingStartNextRound());
    }

    bool leaderAndPlayerValuesInitializedByServer = false;
    List<int> lastLeaders = new List<int>();
    int lastLeaderValue = -99;
    int leader = -99;
    int player1 = -99;
    int player2 = -99;

    public void ServerCalcRandomLeaderAndPlayerValues()
    {
        CalculateLeaderAndPlayerValues();
        SendPlayerValues(serializePlayerLeaderData(9999,leader,player1,player2));
        leaderAndPlayerValuesInitializedByServer = true;
        Debug.Log("leader and Player Values set and send!");
    }
    private byte[] serializePlayerLeaderData(int messageID,int leader, int player1, int player2)
    {
        byte[] data = null;
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(messageID);
                writer.Write(BitConverter.GetBytes(leader));
                writer.Write(BitConverter.GetBytes(player1));
                writer.Write(BitConverter.GetBytes(player2));

                stream.Position = 0;
                data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
            }
        }
        return data;
    }

    public void SendPlayerValues(byte[] serialized)
    {
        FindObjectOfType<Realtime>().room.SendRPCMessage(serialized, false);
    }

    public void CalculateLeaderAndPlayerValues()
    {
        if (lastLeaders.Count == NumbersOfParticipatingPlayers)
            lastLeaders.Clear();

        int randomNumber = UnityEngine.Random.Range(0, NumbersOfParticipatingPlayers);
        while (lastLeaders.Contains(randomNumber) || randomNumber == lastLeaderValue)
        {
            randomNumber = UnityEngine.Random.Range(0, NumbersOfParticipatingPlayers);
        }
        lastLeaderValue = randomNumber;
        lastLeaders.Add(randomNumber);

        int[] tmpPlayerValues = new int[2];
        int count = 0;
        for(int i = 0; i < NumbersOfParticipatingPlayers; ++i)
        {
            if (i == randomNumber)
            {
                continue;
            }
            tmpPlayerValues[count] = i;
            count = count + 1;
        }

        leader = randomNumber;
        player1 = tmpPlayerValues[0];
        player2 = tmpPlayerValues[1];

        Debug.Log("RDN :" + randomNumber);
        // Sets the Button States of the Leader this Round and also fills the ButtonsToPress List
    }
    IEnumerator SequencingStart()
    {
        if (isServer)
            ServerCalcRandomLeaderAndPlayerValues();
        yield return new WaitUntil(() => leaderAndPlayerValuesInitializedByServer == true);
        if(isClient)
        Debug.Log("Leader and Player Values Succsessfully Initialized by Server!");

        yield return startCountdownToStartCoroutine = StartCoroutine(StartCountdownToStart());

        //if (isServer)
        //{
        //    CSVWriter.addRecord(theDate, theTime, timeSinceGameStart, timeSinceRoundStarts, GetRound, RoundEfficiency(), ChangeAvatarAppearanceButton.Instance.AvatarIsIK, Podests[0].PlayerNumber, Podests[0].DeselectCount, Podests[1].PlayerNumber, Podests[1].DeselectCount, Podests[2].PlayerNumber, Podests[2].DeselectCount);
        //}
        nextRoundIsBootingUp = true;
        gameTimeCounterCoroutine = StartCoroutine(GameTimeCounter());
        roundTimeCounterCoroutine = StartCoroutine(RoundTimerCounter());
        InitializePlayers();
    }
    IEnumerator SequencingStartNextRound()
    {
        if (isServer)
            ServerCalcRandomLeaderAndPlayerValues();
        yield return new WaitUntil(() => leaderAndPlayerValuesInitializedByServer == true);
        if (isClient)
            Debug.Log("Leader and Player Values Succsessfully Initialized by Server!");
        
        yield return startCountdownToStartCoroutine = StartCoroutine(StartCountdownToStart());

        InitializePlayers();
    }
    public void InitializePlayers()
    {
        SetPlayerValues();
        SetIndicatorPlanes();
        // Inject the RoundRules each Round to get the element Count
        SetButtonsToPress(RoundRules.GetElementCountThisRound(round));

        if (isServer)
        {
            CSVWriter.addRecord(theDate, theTime, timeSinceGameStart, timeSinceRoundStarts, GetRound, RoundEfficiency(), ChangeAvatarAppearanceButton.Instance.AvatarIsIK, Podests[0].PlayerNumber, Podests[0].DeselectCount, Podests[1].PlayerNumber, Podests[1].DeselectCount, Podests[2].PlayerNumber, Podests[2].DeselectCount);
        }
        timeSinceRoundStarts = 0f;
        readyForNextRound = false;
        nextRoundIsBootingUp = false;
        leaderAndPlayerValuesInitializedByServer = false;
        Debug.Log("....Bootup next round finished");
    }

    public void ResetAllFirstTime()
    {
        if (isQuitting)
            return;

        // Resets the Whole Game
        if (gameTimeCounterCoroutine != null)
        {
            StopCoroutine(gameTimeCounterCoroutine);
            gameTimeCounterCoroutine = null;
        }
        if (startCountdownToStartCoroutine != null)
        {
            StopCoroutine(startCountdownToStartCoroutine);
        }
        if (roundTimeCounterCoroutine != null)
        {
            StopCoroutine(roundTimeCounterCoroutine);
        }
        ClearForNextRound();
        gameIsRunning = false;
        round = RoundNumberToStartWithIntSync.GetIntValue;
        timeSinceGameStart = 0f;
        timeSinceRoundStarts = 0f;
        resetAllCalled.Invoke();
        // Kick() ??
    }

    public void ResetAll()
    {
        if (isQuitting)
            return;

        if (isServer)
        {
            CSVWriter.addRecord(theDate, theTime, timeSinceGameStart, timeSinceRoundStarts, GetRound, RoundEfficiency(), ChangeAvatarAppearanceButton.Instance.AvatarIsIK, Podests[0].PlayerNumber, Podests[0].DeselectCount, Podests[1].PlayerNumber, Podests[1].DeselectCount, Podests[2].PlayerNumber, Podests[2].DeselectCount);
            CSVWriter.addEndRoundRecord();
            Debug.Log("ResetAllLogsWritten");
        }
        // Resets the Whole Game
        if (gameTimeCounterCoroutine != null)
        {
            StopCoroutine(gameTimeCounterCoroutine);
            gameTimeCounterCoroutine = null;
        }
        if(startCountdownToStartCoroutine != null)
        {
            StopCoroutine(startCountdownToStartCoroutine);
        }
        if(roundTimeCounterCoroutine != null)
        {
            StopCoroutine(roundTimeCounterCoroutine);
        }
        ClearForNextRound();
        gameIsRunning = false;
        round = RoundNumberToStartWithIntSync.GetIntValue;
        timeSinceGameStart = 0f;
        timeSinceRoundStarts = 0f;
        resetAllCalled.Invoke();
        // Kick() ??
    }

    public void SetGameRulesForPlayers()
    {
        foreach (PodestManager podest in Podests)
            podest.SetRoundRules = RoundRules;
    }

    public void ClearForNextRound()
    {
        if(isServer)
            StartCoroutine(CheckForPlayerResetCoroutine());

        StartCoroutine(ClearForNextRoundCoroutine());
    }
    IEnumerator ClearForNextRoundCoroutine()
    {
        foreach (PodestManager podests in Podests)
            podests.ResetAll();

        // SYNC POINT
        yield return new WaitUntil(() => !ReadyForNextRoundBoolSync);

        Player1 = null;
        Player2 = null;
        CurrentLeader = null;

    }

    IEnumerator CheckForPlayerResetCoroutine()
    {
        // WAIT TILL ALL PLAYERS HAVE SUCC RESETTED THEIR VALUES TILL IT GOES ON
        yield return new WaitForSeconds(0.5f);
        if (NumbersOfParticipatingPlayers == 2 && Player1 != null)
        {
            while (Player1.pressedValuesAreCorrectBoolSync.GetBoolValue)
            {
                yield return null;
            }
            ReadyForNextRoundBoolSync.SetBoolValue(false);
        }

        if (NumbersOfParticipatingPlayers == 3 && Player1 != null && Player2 != null)
        {
            while (Player1.pressedValuesAreCorrectBoolSync.GetBoolValue && Player2.pressedValuesAreCorrectBoolSync.GetBoolValue)
            {
                Debug.Log("P1 bool Sync is still " + Player1.pressedValuesAreCorrectBoolSync.GetBoolValue + "/// P2 bool Sync is still " + Player2.pressedValuesAreCorrectBoolSync.GetBoolValue);
                yield return null;
            }
          ReadyForNextRoundBoolSync.SetBoolValue(false);
        }
    }



    //public void SubscribeToPlayerEvent(PodestManager player)
    //{
    //    player.allButtonsHaveBeenPressed += CheckForNextRound;
    //}
    //public void UnsubscribeToPlayerEvent(PodestManager player)
    //{
    //    player.allButtonsHaveBeenPressed -= CheckForNextRound;
    //}

    private void CheckForNextRound()
    {
        if (readyForNextRound)
            return;

        if (isServer)
        {
            if (Player1 == null || Player2 == null || CurrentLeader == null)
                return;

            if (!Player1.pressedValuesAreCorrectBoolSync.GetBoolValue && !Player2.pressedValuesAreCorrectBoolSync.GetBoolValue)
                return;

            //Debug.Log("CheckIfValuesAreCorrect called!");
            if (NumbersOfParticipatingPlayers == 2 && Player1 != null)
            {
                if (Player1.pressedValuesAreCorrectBoolSync.GetBoolValue)
                {
                    ReadyForNextRoundBoolSync.SetBoolValue(true);
                    readyForNextRound = true;
                }
                Debug.Log("Player1 Pressed Values are correct : " + Player1.pressedValuesAreCorrectBoolSync.GetBoolValue);
            }

            if (NumbersOfParticipatingPlayers == 3 && Player1 != null && Player2 != null)
            {
                if (Player1.pressedValuesAreCorrectBoolSync.GetBoolValue && Player2.pressedValuesAreCorrectBoolSync.GetBoolValue)
                {
                    ReadyForNextRoundBoolSync.SetBoolValue(true);
                    readyForNextRound = true;
                }
                Debug.Log("Player1 Pressed Values are correct : " + Player1.pressedValuesAreCorrectBoolSync.GetBoolValue + "Player2 Pressed Values are correct:" + Player2.pressedValuesAreCorrectBoolSync.GetBoolValue);
            }
            //Debug.Log("ReadyForNextRound = " + readyForNextRound);
        }
        if (isClient)
        {
            if (ReadyForNextRoundBoolSync.GetBoolValue)
            {
                readyForNextRound = true;
            }
        }
    }
    public void SetPlayerPositions()
    {

    }

    // REFACTOR THIS WHOLE SHIT
    public void SetPlayerValues()
    {
        SetCurrentRoundForPlayers(round);
        
        for(int i = 0; i < Podests.Count; ++i)
        {
            if (leader == i)
            {
                //Debug.Log("IsLeader "+RoundRules.GetWhoIsLeader(round));
                Podests[i].PlayerNumber = 0;
                CurrentLeader = Podests[i];
            }
            else if(player1 == i)
            {
                //Debug.Log("IsPlayer1 "+RoundRules.GetWhoIsPlayer1(round));
                Podests[i].PlayerNumber = 1;
                Player1 = Podests[i];
                //SubscribeToPlayerEvent(Player1);
            }
            else if (player2 == i)
            {
                //Debug.Log("IsPlayer2 "+RoundRules.GetWhoIsPlayer2(round));
                Podests[i].PlayerNumber = 2;
                Player2 = Podests[i];
                //SubscribeToPlayerEvent(Player2);
            }
        }

        Debug.Log("IsLeader " + leader + "/// IsPlayer1 "+ player1 + "/// IsPlayer2 " + player2);

    }

    public void SetCurrentRoundForPlayers(int round)
    {
        foreach (PodestManager podest in Podests)
            podest.SetCurrentRound = round;
    }

    public void SetIndicatorPlanes()
    {
        foreach (PodestManager podest in Podests)
            podest.SetPlayerIndicators();
    }

    private void SetButtonsToPress(int elementCount)
    {

        if (NumbersOfParticipatingPlayers == 2) {
            CurrentLeader.SetButtonValues(RoundRules.GetButtonsForPlayer1(round));
            Player1.SetButtonValues(RoundRules.GetButtonsForPlayer1(round));
        }

        else if (NumbersOfParticipatingPlayers == 3)
        {
            CurrentLeader.SetButtonValues(RoundRules.GetButtonsForPlayer1(round), RoundRules.GetButtonsForPlayer2(round));
            Player1.SetButtonValues(RoundRules.GetButtonsForPlayer1(round));
            Player2.SetButtonValues(RoundRules.GetButtonsForPlayer2(round));
        }

    }
    private void CheckIfPlayersFinishedButtonPress()
    {
        // Checks the Player1 and Player2 Pressed Buttons if they are filled
    }

    private void ActivateLastStep()
    {
        // Activate the Last Step where the Players need to make an cooperative Handshake
    }

    private void GiveRoundCorrectOrIncorrectFeedback()
    {
        // Gives Feedback wether the Pressed Buttons were correct or not
    }

    IEnumerator GameTimeCounter()
    {
        while (true)
        {
            while (gameIsRunning && !countdownToStartIsActive && !nextRoundIsBootingUp)
            {
                timeSinceGameStart += Time.unscaledDeltaTime;

                if (isServer)
                    if (timeSinceGameStart >= OverallGameTime)
                        RestartBoolSync.ToggleBoolValue();

                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator RoundTimerCounter()
    {
        while (true)
        {
            while (gameIsRunning && !countdownToStartIsActive && !nextRoundIsBootingUp)
            {
                timeSinceRoundStarts += Time.unscaledDeltaTime;
                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator StartCountdownToStart()
    {
        countdownToStartIsActive = true;
        // Changes the SphereMaterial
        Sphere.SetInt("SpawnRate", 0);
        Sphere.SetFloat("ForceRadius", 25f);
        Sphere.SetInt("ColorSwitch0G1R", 0);
        yield return new WaitForSeconds(1.5f);
        Sphere.SetFloat("ForceRadius", 0.3f);
        yield return new WaitForSeconds(1.5f);
        Sphere.SetInt("SpawnRate", 100000);
        Sphere.SetInt("ColorSwitch0G1R", 1);
        // Starts an Countdown each Round

        GameStartTimerTMP.enabled = true;
        float startTime = Time.time;
        int countdown = 0;
        while (Time.time - startTime < CountDownToStartInSeconds)
        {
            countdown = (int)(CountDownToStartInSeconds - (Time.time - startTime));
            GameStartTimerTMP.text = countdown.ToString();
            //Debug.Log("StartCountdown " +(CountDownToStartInSeconds-(Time.time - startTime)));
            yield return null;
            // TODO : REF TO TMP
        }
        countdown = 0;
        GameStartTimerTMP.enabled = false;
        countdownToStartIsActive = false;
    }

    public float RoundEfficiency()
    {
        float effiziency = 0f;
        if (GetRound == 0)
            return effiziency;
        effiziency = 1 - (timeSinceRoundStarts / OverallGameTime);
        return effiziency;
    }
    // ROUND
    // OVERALLGAMETIME
    // ROUNDTIME
    // GESAMTZEIT
    public float SmallRoundEfficiency()
    {
        float effiziency = 0f;
        if (GetRound == 0)
            return effiziency;
        effiziency = 1 - (GetRound / OverallGameTime);
        return effiziency;
    }

    public float blabla()
    {
        float effiziency = 0f;
        if (GetRound == 0)
            return effiziency;
        effiziency = 1 - (GetRound / timeSinceGameStart);
        return effiziency;
    }
}
