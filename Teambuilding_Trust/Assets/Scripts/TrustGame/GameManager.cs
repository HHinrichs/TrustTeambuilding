using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    private static GameManager _Instance = null;

    public static GameManager Instance { get { return _Instance; } }

    [SerializeField] int NumbersOfParticipatingPlayers;
    [SerializeField] GameObject PlayerSpawnPositions;
    [SerializeField] List<PodestManager> Podests;
    [SerializeField] int CountDownToStartInSeconds;
    [SerializeField] TextMeshPro GameStartTimerTMP;
    public RoundRules RoundRules;
    public int RoundNumberToStartWith;

    public BoolSync Podest1BoolSync;
    public BoolSync Podest2BoolSync;
    public BoolSync Podest3BoolSync;
    public BoolSync StartBoolSync;
    public BoolSync RestartBoolSync;

    private PodestManager Player1;
    private PodestManager Player2;
    private PodestManager CurrentLeader;
    public PodestManager GetPlayer1 { get { return Player1; } }
    public PodestManager GetPlayer2 { get { return Player2; } }
    public PodestManager GetCurrentLeader { get { return CurrentLeader; } }

    [Header("GameInformations")]
    public bool gameIsRunning = false;
    [SerializeField] int round = 0;
    [SerializeField] float timeSinceGameStart = 0f;
    [SerializeField] bool readyForNextRound = false;
    [SerializeField] bool countdownToStartIsActive = false;

    // Coroutines
    private Coroutine gameTimeCounterCoroutine = null;
    private Coroutine startCountdownToStartCoroutine = null;

    public UnityEvent unityEvent;

    //Getter Setter
    public int GetRound { get { return round; } }
    public float GetTimeSinceGameStart { get { return timeSinceGameStart; } }

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

        StartBoolSync.boolValueChanged += StartGame;
        RestartBoolSync.boolValueChanged += ResetAll;
    }
    private void Update()
    {
        if (!gameIsRunning)
            return;

        if (countdownToStartIsActive)
            return;

        CheckForNextRound();

        if (readyForNextRound)
            StartNextRound();
    }

    // Networking

    public void StartGame()
    {
        Debug.Log("StartCalled!");
        ResetAll();
        gameIsRunning = true;
        SetGameRulesForPlayers();
        round = RoundNumberToStartWith;

        if (NumbersOfParticipatingPlayers == 1)
            Debug.LogError("At least 2 and the most 3 players are required!");
        if(round == 0)
        {
            Debug.LogError("Round is Set to 0! This is not correct while starting the Game! Set round at least to 1!");
            return;
        }

        StartCoroutine(SequencingStart());

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
    }

    public void ResetAll()
    {
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
        round = 0;
        timeSinceGameStart = 0f;
        // Kick() ??
    }

    public void SetGameRulesForPlayers()
    {
        foreach (PodestManager podest in Podests)
            podest.SetRoundRules = RoundRules;
    }

    public void ClearForNextRound()
    {
        if (Player1 != null)
            UnsubscribeToPlayerEvent(Player1);
        if (Player2 != null)
            UnsubscribeToPlayerEvent(Player2);

        Player1 = null;
        Player2 = null;
        CurrentLeader = null;

        foreach (PodestManager podests in Podests)
            podests.ResetAll();
    }

    private void StartNextRound()
    {
        ClearForNextRound();
        round++;
        StartCoroutine(SequencingStartNextRound());
        readyForNextRound = false;
    }

    public void SubscribeToPlayerEvent(PodestManager player)
    {
        player.allButtonsHaveBeenPressed += CheckForNextRound;
    }
    public void UnsubscribeToPlayerEvent(PodestManager player)
    {
        player.allButtonsHaveBeenPressed -= CheckForNextRound;
    }

    private void CheckForNextRound()
    {
        if (Player1 == null || Player2 == null || CurrentLeader == null)
            return;

        if (!Player1.boolSync.GetBoolValue && !Player2.boolSync.GetBoolValue)
            return;

        //Debug.Log("CheckIfValuesAreCorrect called!");
        if (NumbersOfParticipatingPlayers == 2 && Player1 != null)
        {
            if (Player1.GetPressedValuesAreCorrect)
            {
                readyForNextRound = true;
            }
            Debug.Log("Player1 Pressed Values are correct : " + Player1.GetPressedValuesAreCorrect);
        }

        if (NumbersOfParticipatingPlayers == 3 && Player1 != null && Player2 != null)
        {
            if (Player1.GetPressedValuesAreCorrect && Player2.GetPressedValuesAreCorrect)
            {
                readyForNextRound = true;
            }
            Debug.Log("Player1 Pressed Values are correct : " + Player1.GetPressedValuesAreCorrect + "Player2 Pressed Values are correct:" + Player2.GetPressedValuesAreCorrect);
        }
        //Debug.Log("ReadyForNextRound = " + readyForNextRound);
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
                SubscribeToPlayerEvent(Player1);
            }
            else if (RoundRules.GetWhoIsPlayer2(round) == i)
            {
                Debug.Log("IsPlayer2 "+RoundRules.GetWhoIsPlayer2(round));
                Podests[i].PlayerNumber = 2;
                Player2 = Podests[i];
                SubscribeToPlayerEvent(Player2);
            }
        }

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

    public float Efficiency()
    {
        float effiziency = 0f;
        if (GetRound == 0)
            return effiziency;
        effiziency = GetRound / GetTimeSinceGameStart;
        return effiziency;
    }

}
