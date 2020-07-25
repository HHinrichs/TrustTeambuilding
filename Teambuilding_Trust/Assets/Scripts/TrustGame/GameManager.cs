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

    private int currentLeaderValue = 99;

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
    }
    private void Update()
    {
        if (!gameIsRunning)
            return;

        if (countdownToStartIsActive)
            return;

        if (readyForNextRound)
            StartNextRound();
    }

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
        currentLeaderValue = 99;
        gameIsRunning = false;
        round = 0;
        timeSinceGameStart = 0f;
        currentLeaderValue = 99;
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

        currentLeaderSet = false;
        player1Set = false;
        player2Set = false;

        Player1 = null;
        Player2 = null;
        CurrentLeader = null;

        currentLeaderValue = 99;

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
        Debug.Log("CheckIfValuesAreCorrect called!");
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
        Debug.Log("ReadyForNextRound = " + readyForNextRound);
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
                Podests[i].PlayerNumber = 0;
                currentLeaderSet = true;
                CurrentLeader = Podests[i];
            }

            if (Podests[i].PlayerNumber == 0)
                continue;

            // MAYBE REFACTOR 
            // Sets the Player 1 and 2 ; Podest1 is always green if you do it like this
            if (Podests[i].PlayerNumber != 0 && !player1Set)
            {
                Podests[i].PlayerNumber = 1;
                player1Set = true;
                Player1 = Podests[i];
                SubscribeToPlayerEvent(Player1);
                continue;
            }
            // Sets the Player 2 if possible
            if (Podests[i].PlayerNumber != 0 && !player2Set)
            {
                Podests[i].PlayerNumber = 2;
                player2Set = true;
                Player2 = Podests[i];
                SubscribeToPlayerEvent(Player2);
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
        while (lastLeaders.Contains(randomNumber) || randomNumber == currentLeaderValue)
        {
            randomNumber = Random.Range(0, NumbersOfParticipatingPlayers);
        }
        currentLeaderValue = randomNumber;
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

    List<int> buttonNumbersToPressP1;
    List<int> buttonNumbersToPressP2;
    private void SetButtonsToPress(int elementCount)
    {
        //buttonNumbersToPressP1.Clear();
        //buttonNumbersToPressP2.Clear();
         // Generates a List of Numbers to set the Button Values for the leaders podest
        buttonNumbersToPressP1 = new List<int>();
        buttonNumbersToPressP2 = new List<int>();
        int counterLoop = 0;
        while(counterLoop < elementCount)
        {
            // Mach dies ab 3 spielern , also größer gleich 2 spieler
            if(NumbersOfParticipatingPlayers >= 2)
            {
                int possibleButtonValue = Random.Range(0, CurrentLeader.ButtonsInChildrenCount);

                // Iterate as long as possible thru the list so that no 2 values are the same
                while (buttonNumbersToPressP1.Contains(possibleButtonValue))
                {
                    possibleButtonValue = Random.Range(0, CurrentLeader.ButtonsInChildrenCount);
                }
                buttonNumbersToPressP1.Add(possibleButtonValue);
            }
            // Mach dies bei 3 Spielern, ansonsten nicht!
            if (NumbersOfParticipatingPlayers == 3)
            {
                int possibleButtonValue = Random.Range(0, CurrentLeader.ButtonsInChildrenCount);

                // Iterate as long as possible thru the list so that no 2 values are the same

                while (buttonNumbersToPressP2.Contains(possibleButtonValue))
                {
                    possibleButtonValue = Random.Range(0, CurrentLeader.ButtonsInChildrenCount);
                }

                buttonNumbersToPressP2.Add(possibleButtonValue);
            }
            counterLoop++;
        }

        if (NumbersOfParticipatingPlayers == 2) { 
            CurrentLeader.SetButtonValues(buttonNumbersToPressP1);
            Player1.SetButtonValues(buttonNumbersToPressP1);
        }

        if (NumbersOfParticipatingPlayers == 3)
        {
            CurrentLeader.SetButtonValues(buttonNumbersToPressP1, buttonNumbersToPressP2);
            Player1.SetButtonValues(buttonNumbersToPressP1);
            Player2.SetButtonValues(buttonNumbersToPressP2);
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
