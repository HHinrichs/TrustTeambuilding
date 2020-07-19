using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] int NumbersOfParticipatingPlayers;
    [SerializeField] GameObject PlayerSpawnPositions;
    [SerializeField] List<PodestManager> Podests;
    [SerializeField] int CountDownToStartInSeconds;
    [SerializeField] RoundRules RoundRule;
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

    private List<int> ButtonsToPress = new List<int>();

    private float timeSinceGameStart = 0f;
    private bool gameIsRunning = false;
    private bool readyForNextRound = false;
    private bool countdownToStartIsActive = false;

    public UnityEvent unityEvent;

    //Getter Setter
    public int GetRound { get { return round; } }
    public int GetPlayerCount { get { return playerCount; } }
    public float GetTimeSinceGameStart { get { return timeSinceGameStart; } }

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
        round = RoundNumberToStartWith;

        if(round == 0)
        {
            Debug.LogError("Round is Set to 0! This is not correct while starting the Game! Set round at least to 1!");
            return;
        }
        SetPlayerValues();
        SetIndicatorPlanes();
        SetButtonStates(RoundRule.GetRoundRules(round));
        gameIsRunning = true;
        //StartCoroutine(StartCountdownToStart());
        StartCoroutine(GameTimeCounter());
    }

    public void ResetAll()
    {
        // Resets the Whole Game
        StopCoroutine(GameTimeCounter());
        StopCoroutine(StartCountdownToStart());
        ClearForNextRound();
        currentLeaderValue = 0;
        lastLeaderValue = -99;
        gameIsRunning = false;
        round = 0;
        timeSinceGameStart = 0f;
        // Kick() ??
    }
    public void ClearForNextRound()
    {
        currentLeaderSet = false;
        player1Set = false;
        player2Set = false;
        foreach (PodestManager podests in Podests)
            podests.ResetAll();
    }

    private void StartNextRound()
    {
        ClearForNextRound();
        SetPlayerValues();
        SetIndicatorPlanes();
        round++;
        // Inject the RoundRules each Round to get the element Count
        SetButtonStates(RoundRule.GetRoundRules(round));
        StartCoroutine(StartCountdownToStart());
        readyForNextRound = false;
    }

    public void SetPlayerPositions()
    {
      
    }

    // REFACTOR THIS WHOLE SHIT
    public void SetPlayerValues()
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

        Debug.Log("RDN :" + randomNumber);
        // Sets the Button States of the Leader this Round and also fills the ButtonsToPress List

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
                continue;
            }
            // Sets the Player 2 if possible
            if (!Podests[i].IsCurrentLeader && !player2Set)
            {
                Podests[i].IsPlayer2 = true;
                player2Set = true;
                continue;
            }
        }

        
    }
    public void SetIndicatorPlanes()
    {
        foreach (PodestManager podest in Podests)
            podest.SetPlayerIndicators();
    }

    private void SetButtonStates(int elementCount)
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
                //Debug.Log("timeSinceGameStart" + timeSinceGameStart);
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
   
}
