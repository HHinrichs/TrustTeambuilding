using Normal.Realtime;
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
    public List<BoxCollider> FadeToBlackBoxes;

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
        StartCoroutine(ChooseMyPositionAtConnectionCoroutine());
    }
    public void ChooseMyPositionAtReset()
    {
        StartCoroutine(ChooseMyPositionAtResetCoroutine());
    }

    IEnumerator ChooseMyPositionAtConnectionCoroutine()
    {
        if (gameManager.isClient)
        {
            while (!gameManager.NetworkPlayerPositions.realtimeView.isOwnedLocally)
            {
                gameManager.NetworkPlayerPositions.realtimeView.RequestOwnership();
                // Waits one second to get Updated if the ownership is still mine!
                yield return new WaitForSeconds(1f);
            }
            playerNetworkPositionInt = gameManager.NetworkPlayerPositions.GetIntValue;

            for (int i = 0; i < gameManager.GetNumbersOfParticipatingPlayers; ++i)
            {
                if (IntToBoolean.IsBitSetTo1(playerNetworkPositionInt, i))
                {
                    playerRig.gameObject.transform.position = PlayerNetworkPositions[i].position;
                    playerRig.gameObject.transform.rotation = PlayerNetworkPositions[i].rotation;
                    // Maybe Race Condition here!
                    playerNetworkPositionInt = IntToBoolean.SetBitTo0(playerNetworkPositionInt, i);
                    // Sets the corresponding FadeToBlackBoxCollider
                    playerRig.GetComponentInChildren<FadeToBlack>().SetCorrespondingBoxCollider = FadeToBlackBoxes[i];
                    // SETS THE UPDATED PLAYER POSITION VALUE FOR THIS PLAYER
                    gameManager.NetworkPlayerPositions.SetIntValue(IntToBoolean.SetBitTo0(playerNetworkPositionInt, i));
                    Debug.Log("PLAYER POSITION SET TO " + PlayerNetworkPositions[i]);
                    gameManager.NetworkPlayerPositions.realtimeView.ClearOwnership();
                    yield break;
                }
            }

            Debug.Log("NO POSITION LEFT, SETTING TO DEFAULT POSITION");
            playerRig.gameObject.transform.position = PlayerNetworkPositions[3].position;
            playerRig.gameObject.transform.rotation = PlayerNetworkPositions[3].rotation;
            gameManager.NetworkPlayerPositions.realtimeView.ClearOwnership();
        }
        else
        {
            Debug.Log("It seems that you are the SERVER OR the server has not defined network positions yet... therefore there is no server availible!");
            Debug.Log("gameManager.isClient == " + gameManager.isClient);
        }
        yield return null;
    }
    IEnumerator ChooseMyPositionAtResetCoroutine()
    {
        yield return new WaitUntil(() => realtime.connected);

        if (gameManager.isClient)
        {
            while (!gameManager.NetworkPlayerPositions.realtimeView.isOwnedLocally)
            {
                gameManager.NetworkPlayerPositions.realtimeView.RequestOwnership();
                // Waits one second to get Updated if the ownership is still mine!
                yield return new WaitForSeconds(1f);
            }
            playerNetworkPositionInt = gameManager.NetworkPlayerPositions.GetIntValue;

            for (int i = 0; i < gameManager.GetNumbersOfParticipatingPlayers; ++i)
            {
                if (IntToBoolean.IsBitSetTo1(playerNetworkPositionInt, i))
                {
                    playerRig.gameObject.transform.position = PlayerNetworkPositions[i].position;
                    playerRig.gameObject.transform.rotation = PlayerNetworkPositions[i].rotation;
                    playerNetworkPositionInt = IntToBoolean.SetBitTo0(playerNetworkPositionInt, i);
                    // Sets the corresponding FadeToBlackBoxCollider
                    playerRig.GetComponentInChildren<FadeToBlack>().SetCorrespondingBoxCollider = FadeToBlackBoxes[i];
                    // SETS THE UPDATED PLAYER POSITION VALUE FOR THIS PLAYER
                    gameManager.NetworkPlayerPositions.SetIntValue(IntToBoolean.SetBitTo0(playerNetworkPositionInt, i));
                    Debug.Log("PLAYER POSITION SET TO " + PlayerNetworkPositions[i]);
                    gameManager.NetworkPlayerPositions.realtimeView.ClearOwnership();
                    yield break;
                }
            }

            Debug.Log("NO POSITION LEFT, SETTING TO DEFAULT POSITION");
            playerRig.gameObject.transform.position = PlayerNetworkPositions[3].position;
            playerRig.gameObject.transform.rotation = PlayerNetworkPositions[3].rotation;
            gameManager.NetworkPlayerPositions.realtimeView.ClearOwnership();
        }
        else
        {
            Debug.Log("It seems that you are the SERVER OR the server has not defined network positions yet... therefore there is no server availible!");
            Debug.Log("gameManager.isClient == " + gameManager.isClient);
        }
        yield return null;
    }
}
