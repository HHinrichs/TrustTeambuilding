using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private static GameManager _Instance = null;

    public static GameManager Instance { get { return _Instance; } }

    [SerializeField] int NumbersOfParticipatingPlayers;
    [SerializeField] GameObject PlayerSpawnPositions;
    [SerializeField] List<PodestManager> Podests;
    [SerializeField] int CountDownToStartInSeconds;
    public RoundRules RoundRules;
    private List<int> buttonsToPressThisRound = new List<int>();
    public int RoundNumberToStartWith;
    private int round = 0;
    private int playerCount = 0;
    private int currentLeaderValue = 0;
    private int lastLeaderValue = -99;

    private List<int> lastLeaders = new List<int>();

    private bool currentLeaderSet = false;
    private bool player1Set = false;
    private bool player2Set = false;
    private PodestManager Player1;
    private PodestManager Player2;
    private PodestManager CurrentLeader;
    public PodestManager GetPlayer1 { get { return Player1; } }
    public PodestManager GetPlayer2 { get { return Player2; } }
    public PodestManager GetCurrentLeader { get { return CurrentLeader; } }

    private List<int> ButtonsToPress = new List<int>();

    private float timeSinceGameStart = 0f;
    private bool gameIsRunning = false;
    private bool readyForNextRound = false;
    private bool countdownToStartIsActive = false;

    // Coroutines
    private Coroutine gameTimeCounterCoroutine = null;
    private Coroutine startCountdownToStartCoroutine = null;

    public UnityEvent unityEvent;

    //Getter Setter
    public int GetRound { get { return round; } }
    public int GetPlayerCount { get { return playerCount; } }
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
    }
    private void Update()
    {
        if (!gameIsRunning)
            return;

        if (readyForNextRound)
            StartNextRound();

    }

    private void Start()
    {
        //StartGame();
    }

    public void StartGame()
    {
        ResetAll();
        SetGameRulesForPlayers();
        round = RoundNumberToStartWith;

        if(round == 0)
        {
            Debug.LogError("Round is Set to 0! This is not correct while starting the Game! Set round at least to 1!");
            return;
        }

        InitializePlayers();

        gameIsRunning = true;
        //startCountdownToStartCoroutine = StartCoroutine(StartCountdownToStart());
        gameTimeCounterCoroutine = StartCoroutine(GameTimeCounter());
    }

    public void InitializePlayers()
    {
        SetPlayerValues();
        SetIndicatorPlanes();
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
        currentLeaderValue = 0;
        lastLeaderValue = -99;
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
        currentLeaderSet = false;
        player1Set = false;
        player2Set = false;
        Player1 = null;
        Player2 = null;
        CurrentLeader = null;
        foreach (PodestManager podests in Podests)
            podests.ResetAll();
    }

    private void StartNextRound()
    {
        ClearForNextRound();
        InitializePlayers();
        round++;
        // Inject the RoundRules each Round to get the element Count
        StartCoroutine(StartCountdownToStart());
        readyForNextRound = false;
    }

    public void SetPlayerPositions()
    {
      
    }

    // REFACTOR THIS WHOLE SHIT
    public void SetPlayerValues()
    {
        SetCurrentLeader();
        SetCurrentRoundForPlayers(round);
        // Set currentLeader Flag
        for (int i = 0; i < Podests.Count; ++i)
        {
            // Go Out if the Number Of Participating players is Smaller than the Podest count
            if (i == NumbersOfParticipatingPlayers)
                break;

            if (i == currentLeaderValue)
            {
                Podests[i].IsCurrentLeader = true;
                currentLeaderSet = true;
                CurrentLeader = Podests[i];
            }
            else
                Podests[i].IsCurrentLeader = false;

            if (Podests[i].IsCurrentLeader)
                continue;

            // MAYBE REFACTOR 
            // Sets the Player 1 and 2 ; Podest1 is always green if you do it like this
            if (!Podests[i].IsCurrentLeader && !player1Set)
            {
                Podests[i].IsPlayer1 = true;
                player1Set = true;
                Player1 = Podests[i];
                continue;
            }
            // Sets the Player 2 if possible
            if (!Podests[i].IsCurrentLeader && !player2Set)
            {
                Podests[i].IsPlayer2 = true;
                player2Set = true;
                Player2 = Podests[i];
                continue;
            }
        }
    }

    public void SetCurrentLeader()
    {
        // Cycles semi Random between the 3 Players
        if (lastLeaders.Count == NumbersOfParticipatingPlayers)
            lastLeaders.Clear();

        int randomNumber = Random.Range(0, NumbersOfParticipatingPlayers);
        while (lastLeaders.Contains(randomNumber) || randomNumber == lastLeaderValue)
        {
            randomNumber = Random.Range(0, NumbersOfParticipatingPlayers);
        }
        currentLeaderValue = randomNumber;
        lastLeaderValue = randomNumber;
        lastLeaders.Add(currentLeaderValue);
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
       
        // Generates a List of Numbers to set the Button Values for the leaders podest
        List<int> buttonNumbersToPressP1 = new List<int>();
        List<int> buttonNumbersToPressP2 = new List<int>();
        int counterLoop = 0;
        while(counterLoop < elementCount)
        {
            if(NumbersOfParticipatingPlayers > 1)
            {
                int possibleButtonValue = Random.Range(0, Podests[currentLeaderValue].ButtonsInChildrenCount);

                // Iterate as long as possible thru the list so that no 2 values are the same
                while (buttonNumbersToPressP1.Contains(possibleButtonValue))
                {
                    possibleButtonValue = Random.Range(0, Podests[currentLeaderValue].ButtonsInChildrenCount);
                }
                buttonNumbersToPressP1.Add(possibleButtonValue);
            }
            if (NumbersOfParticipatingPlayers > 2)
            {
                int possibleButtonValue = Random.Range(0, Podests[currentLeaderValue].ButtonsInChildrenCount);

                // Iterate as long as possible thru the list so that no 2 values are the same

                while (buttonNumbersToPressP2.Contains(possibleButtonValue))
                {
                    possibleButtonValue = Random.Range(0, Podests[currentLeaderValue].ButtonsInChildrenCount);
                }

                buttonNumbersToPressP2.Add(possibleButtonValue);
            }
            counterLoop++;
        }

        if (NumbersOfParticipatingPlayers > 1)
            Podests[currentLeaderValue].SetButtonValues(buttonNumbersToPressP1);

        if (NumbersOfParticipatingPlayers > 2)
            Podests[currentLeaderValue].SetButtonValues(buttonNumbersToPressP1, buttonNumbersToPressP2);

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
                Debug.Log("timeSinceGameStart" + timeSinceGameStart);
                yield return null;
            }

            yield return null;
        }
    }

    IEnumerator StartCountdownToStart()
    {
        // Starts an Countdown each Round

        countdownToStartIsActive = true;
        float startTime = Time.time;
        int countdown = 0;
        while (Time.time - startTime < CountDownToStartInSeconds)
        {
             countdown = (int)(CountDownToStartInSeconds - (Time.time - startTime));
            //Debug.Log("StartCountdown " +(CountDownToStartInSeconds-(Time.time - startTime)));
            yield return null;
            // TODO : REF TO TMP
        }
        countdown = 0;

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
