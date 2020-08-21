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

public class GameManager : MonoBehaviour
{
    private static GameManager _Instance = null;

    public static GameManager Instance { get { return _Instance; } }

    public delegate void ResetAllCalled();
    public event ResetAllCalled resetAllCalled;
    [Header("Allgemeine Einstellungen")]
    public bool isServer;
    public bool isClient;
    [SerializeField] int NumbersOfParticipatingPlayers;
    [SerializeField] GameObject PlayerSpawnPositions;
    [SerializeField] List<PodestManager> Podests;
    [SerializeField] int CountDownToStartInSeconds;
    [SerializeField] int timeToWaitTillCountdown;
    [SerializeField] TextMeshPro GameStartTimerTMP;
    [SerializeField] VisualEffect Sphere;
    [SerializeField] NetworkAudioReceiver NetworkAudioReceiver;
    public RoundRules RoundRules;
    public int RoundNumberToStartWith;
    private Queue<Action> RawQueue = new Queue<Action>();

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
    [SerializeField] bool readyForNextRound = false;
    [SerializeField] bool countdownToStartIsActive = false;
    [SerializeField] bool nextRoundIsBootingUp = false;

    private PodestManager Player1;
    private PodestManager Player2;
    private PodestManager CurrentLeader;
    // Coroutines
    private Coroutine gameTimeCounterCoroutine = null;
    private Coroutine startCountdownToStartCoroutine = null;

    public UnityEvent unityEvent;

    //Getter Setter
    public PodestManager GetPlayer1 { get { return Player1; } }
    public PodestManager GetPlayer2 { get { return Player2; } }
    public PodestManager GetCurrentLeader { get { return CurrentLeader; } }
    public int GetRound { get { return round; } }
    public float GetTimeSinceGameStart { get { return timeSinceGameStart; } }

    public int GetNumbersOfParticipatingPlayers { get { return NumbersOfParticipatingPlayers; } }

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

    private void Start()
    {
        StartCoroutine(LateStart(5));
    }

    IEnumerator LateStart(float waitTime)
    {
        Realtime realtime = FindObjectOfType<Realtime>();
        yield return new WaitUntil(() => realtime.connected == true);
        yield return new WaitUntil(() => realtime.room.connected == true);

        if (isServer)
        {
            SetPlayerNetworkPositions();
            realtime.room.rpcMessageReceived += ClientRCPMessageReceived;
        }
        if (isClient)
            realtime.room.rpcMessageReceived += ServerRCPMessageReceived;

        Debug.Log("LateStartSuc!");
        StartBoolSync.boolValueChanged += StartGame;
        RestartBoolSync.boolValueChanged += ResetAll;
    }

    private void Update()
    {
        //Handle RawQueue Inputs
        while (RawQueue.Count > 0)
        {
            RawQueue.Dequeue().Invoke();
        }

        if (!gameIsRunning)
            return;

        if (countdownToStartIsActive)
            return;

        CheckForNextRound();

        if (readyForNextRound && !nextRoundIsBootingUp)
            StartNextRound();
    }

    public void ServerRCPMessageReceived(Room room, byte[] data, bool reliable)
    {
        Debug.Log("RCPMessageReceived");
        //byte[] messageID = getSubPartOfByteArray(data,0,sizeof(int)) ;

        int messageInt = BitConverter.ToInt32(data,0);
        switch (messageInt)
        {
            //Kick Message Received
            case 1000:
                int clientValueID = BitConverter.ToInt32(data, sizeof(int));
                Realtime realtime = FindObjectOfType<Realtime>();
                if (realtime.clientID == clientValueID)
                    realtime.Disconnect();
                break;
            default:
                break;
        }
    }

    public void ClientRCPMessageReceived(Room room, byte[] data, bool reliable)
    {
        Debug.Log("ClientRCPMessageReceived");
        //byte[] messageID = getSubPartOfByteArray(data,0,sizeof(int)) ;

        int messageInt = BitConverter.ToInt32(data, 0);
        switch (messageInt)
        {
            //ClientAudioStreamReceived Message Received
            case 2000:
                Debug.Log("Audio Stream from Client Received!");
                RawQueue.Enqueue(() =>
                {
                    byte[] rawMicrophoneData = getSubPartOfByteArray(data, sizeof(int), data.Length - sizeof(int));
                    if (NetworkAudioReceiver != null)
                    {
                        NetworkAudioReceiver.setAudioData(rawMicrophoneData);
                    }
                    else
                    {
                        GameObject NewlyCreatedNetworkAudioReceiver = new GameObject();
                        NewlyCreatedNetworkAudioReceiver.name = "NewlyCreatedNetworkAudioReceiver";
                        NetworkAudioReceiver = NewlyCreatedNetworkAudioReceiver.AddComponent<NetworkAudioReceiver>();
                    }
                });
                break;

            default:
                break;
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
        int playerNetworkPositionInt = 0;
        for(int i = 0; i < NumbersOfParticipatingPlayers; ++i)
        {
            playerNetworkPositionInt = IntToBoolean.SetBitTo1(playerNetworkPositionInt, i);
        }
        NetworkPlayerPositions.SetIntValue(playerNetworkPositionInt);
    }

    public void StartGame()
    {
        Debug.Log("StartCalled!");

        round = RoundNumberToStartWithIntSync.GetIntValue;

        if (NumbersOfParticipatingPlayers == 1)
            Debug.LogError("At least 2 and the most 3 players are required!");
        if(round == 0)
        {
            Debug.LogError("Round is Set to 0! This is not correct while starting the Game! Set round at least to 1!");
            return;
        }

        ResetAll();
        gameIsRunning = true;
        SetGameRulesForPlayers();

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

    IEnumerator SequencingStart()
    {
        yield return startCountdownToStartCoroutine = StartCoroutine(StartCountdownToStart());
        gameTimeCounterCoroutine = StartCoroutine(GameTimeCounter());
        InitializePlayers();
    }
    IEnumerator SequencingStartNextRound()
    {
        yield return startCountdownToStartCoroutine = StartCoroutine(StartCountdownToStart());
        InitializePlayers();
    }
    public void InitializePlayers()
    {
        SetPlayerValues();
        SetIndicatorPlanes();
        // Inject the RoundRules each Round to get the element Count
        SetButtonsToPress(RoundRules.GetElementCountThisRound(round));

        readyForNextRound = false;
        nextRoundIsBootingUp = false;
        Debug.Log("....Bootup next round finished");
    }

    public void ResetAll()
    {
        if(isServer)
            SetPlayerNetworkPositions();
        // Resets the Whole Game
        if(gameTimeCounterCoroutine != null)
        {
            StopCoroutine(gameTimeCounterCoroutine);
            gameTimeCounterCoroutine = null;
        }
        if(startCountdownToStartCoroutine != null)
        {
            StopCoroutine(startCountdownToStartCoroutine);
        }
        ClearForNextRound();
        gameIsRunning = false;
        round = RoundNumberToStartWithIntSync.GetIntValue;
        timeSinceGameStart = 0f;
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
            if (RoundRules.GetWhoIsLeader(round) == i)
            {
                Debug.Log("IsLeader "+RoundRules.GetWhoIsLeader(round));
                Podests[i].PlayerNumber = 0;
                CurrentLeader = Podests[i];
            }
            else if(RoundRules.GetWhoIsPlayer1(round) == i)
            {
                Debug.Log("IsPlayer1 "+RoundRules.GetWhoIsPlayer1(round));
                Podests[i].PlayerNumber = 1;
                Player1 = Podests[i];
                //SubscribeToPlayerEvent(Player1);
            }
            else if (RoundRules.GetWhoIsPlayer2(round) == i)
            {
                Debug.Log("IsPlayer2 "+RoundRules.GetWhoIsPlayer2(round));
                Podests[i].PlayerNumber = 2;
                Player2 = Podests[i];
                //SubscribeToPlayerEvent(Player2);
            }
        }

        Debug.Log("IsLeader " + RoundRules.GetWhoIsLeader(round)+"/// IsPlayer1 "+ RoundRules.GetWhoIsPlayer1(round)+ "/// IsPlayer1 " + RoundRules.GetWhoIsPlayer2(round));

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
            while (gameIsRunning && !countdownToStartIsActive)
            {
                timeSinceGameStart += Time.unscaledDeltaTime;
                yield return null;
            }

            yield return null;
        }
    }

    IEnumerator StartCountdownToStart()
    {

        // Changes the SphereMaterial
        Sphere.SetInt("SpawnRate", 0);
        Sphere.SetFloat("ForceRadius", 5f);
        Sphere.SetInt("ColorSwitch0G1R", 0);
        yield return new WaitForSeconds(1.5f);
        Sphere.SetFloat("ForceRadius", 0.3f);
        yield return new WaitForSeconds(1.5f);
        Sphere.SetInt("SpawnRate", 100000);
        Sphere.SetInt("ColorSwitch0G1R", 1);
        // Starts an Countdown each Round

        countdownToStartIsActive = true;
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
        effiziency = (GetRound / GetTimeSinceGameStart)*100f;
        return effiziency;
    }
}
