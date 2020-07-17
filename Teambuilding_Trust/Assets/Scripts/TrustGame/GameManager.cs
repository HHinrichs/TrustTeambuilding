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

    private List<int> buttonsToPressThisRound = new List<int>();

    private int round = 0;
    private int playerCount = 0;
    private int currentLeader = 0;
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
        gameIsRunning = true;
        round = 3;
        ChooseThisRoundsLeader();
        //StartCoroutine(StartCountdownToStart());
        SetButtonStates();
        //StartCoroutine(GameTimeCounter());
    }

    public void ResetAll()
    {
        // Resets the Whole Game
        StopCoroutine(GameTimeCounter());
        StopCoroutine(StartCountdownToStart());

        foreach (PodestManager podests in Podests)
            podests.ResetAll();

        gameIsRunning = false;
        round = 0;
        timeSinceGameStart = 0f;
        // Kick() ??
    }

    private void StartNextRound()
    {
        round++;
        ChooseThisRoundsLeader();
        SetButtonStates();
        StartCoroutine(StartCountdownToStart());
        readyForNextRound = false;
    }

    public void SetPlayerPositions()
    {
      
    }

    private void ChooseThisRoundsLeader()
    {
        int randomNumber = Random.Range(0, NumbersOfParticipatingPlayers);
        while (currentLeader == randomNumber)
        {
            randomNumber = Random.Range(0, NumbersOfParticipatingPlayers);
        }
        currentLeader = randomNumber;
    }
    private void SetButtonStates()
    {
        // Sets the Button States of the Leader this Round and also fills the ButtonsToPress List

        // Set currentLeader Flag
        for(int i = 0; i < Podests.Count; ++i)
        {
            // Go Out if the Number Of Participating players is Smaller than the Podest count
            if (i == NumbersOfParticipatingPlayers)
                break;

            if (i == currentLeader)
                Podests[i].IsCurrentLeader = true;
            else
                Podests[i].IsCurrentLeader = false;

        }

        // Generates a List of Numbers to set the Button Values for the leaders podest

        List<int> buttonNumbersToPressP1 = new List<int>();
        List<int> buttonNumbersToPressP2 = new List<int>();
        int counterLoop = 0;
        while(counterLoop < round)
        {
            if(NumbersOfParticipatingPlayers > 1)
            {
                int possibleButtonValue = Random.Range(0, Podests[currentLeader].ButtonsInChildrenCount);

                // Iterate as long as possible thru the list so that no 2 values are the same
                while (buttonNumbersToPressP1.Contains(possibleButtonValue))
                {
                    possibleButtonValue = Random.Range(0, Podests[currentLeader].ButtonsInChildrenCount);
                }
                buttonNumbersToPressP1.Add(possibleButtonValue);
            }
            if (NumbersOfParticipatingPlayers > 2)
            {
                int possibleButtonValue = Random.Range(0, Podests[currentLeader].ButtonsInChildrenCount);

                // Iterate as long as possible thru the list so that no 2 values are the same

                while (buttonNumbersToPressP2.Contains(possibleButtonValue))
                {
                    possibleButtonValue = Random.Range(0, Podests[currentLeader].ButtonsInChildrenCount);
                }

                buttonNumbersToPressP2.Add(possibleButtonValue);
            }
            counterLoop++;
        }

        if (NumbersOfParticipatingPlayers > 1)
            Podests[currentLeader].SetButtonValues(buttonNumbersToPressP1);

        if (NumbersOfParticipatingPlayers > 2)
            Podests[currentLeader].SetButtonValues(buttonNumbersToPressP1, buttonNumbersToPressP2);

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
