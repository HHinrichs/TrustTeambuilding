﻿using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetPlayerNetworkPosition : MonoBehaviour
{
    [SerializeField] List<Transform> PlayerNetworkPositions;
    private GameManager gameManager;
    private int playerNetworkPositionInt;
    private XRRig playerRig;
    private Realtime realtime;

    private void Start()
    {
        playerRig = FindObjectOfType<XRRig>();
        realtime = FindObjectOfType<Realtime>();
        gameManager = GameManager.Instance;
        realtime.didConnectToRoom += ChooseMyPositionAtConnection;
        gameManager.resetAllCalled += ChooseMyPositionAtReset;
    }

    public void ChooseMyPositionAtConnection(Realtime realtime)
    {

        if (gameManager.isClient && gameManager.GetNetworkPlayerPositionsInitialized)
        {
            StartCoroutine(ChooseMyPositionCoroutine());
        }
        else
        {
            Debug.Log("It seems that you are the SERVER OR the server has not defined network positions yet... therefore there is no server availible!");
            Debug.Log("gameManager.isClient == " + gameManager.isClient + "///gameManager.GetNetworkPlayerPositionsInitialized " + gameManager.GetNetworkPlayerPositionsInitialized);
        }
    }

    public void ChooseMyPositionAtReset()
    {

        if (gameManager.isClient && gameManager.GetNetworkPlayerPositionsInitialized)
        {
            StartCoroutine(ChooseMyPositionCoroutine());
        }
        else
        {
            Debug.Log("It seems that you are the SERVER OR the server has not defined network positions yet... therefore there is no server availible!");
            Debug.Log("gameManager.isClient == " + gameManager.isClient + "///gameManager.GetNetworkPlayerPositionsInitialized " + gameManager.GetNetworkPlayerPositionsInitialized);
        }
    }

    public IEnumerator ChooseMyPositionCoroutine()
    {
        playerNetworkPositionInt = gameManager.NetworkPlayerPositions.GetIntValue;
        
        for(int i = 0; i < gameManager.GetNumbersOfParticipatingPlayers; ++i)
        {
            if (IntToBoolean.IsBitSetTo1(playerNetworkPositionInt, 0))
            {
                playerRig.gameObject.transform.position = PlayerNetworkPositions[i].position;
                playerRig.gameObject.transform.rotation = PlayerNetworkPositions[i].rotation;
                playerNetworkPositionInt = IntToBoolean.SetBitTo0(playerNetworkPositionInt, i);
                // SETS THE UPDATED PLAYER POSITION VALUE FOR THIS PLAYER
                gameManager.NetworkPlayerPositions.SetIntValue(IntToBoolean.SetBitTo0(playerNetworkPositionInt, i));
                Debug.Log("PLAYER POSITION SET TO " + PlayerNetworkPositions[i]);
                yield break;
            }
        }

        Debug.Log("NO POSITION LEFT, SETTING TO DEFAULT POSITION");
        playerRig.gameObject.transform.position = PlayerNetworkPositions[3].position;
        playerRig.gameObject.transform.rotation = PlayerNetworkPositions[3].rotation;


    }
}