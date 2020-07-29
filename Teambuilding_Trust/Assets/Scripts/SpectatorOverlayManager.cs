using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class SpectatorOverlayManager : MonoBehaviour
{
    public TextMeshProUGUI ElapsedTime;
    public TextMeshProUGUI RoundNumber;
    public TextMeshProUGUI Efficienzy;
    public TextMeshProUGUI CurrentlyCorrectP1;
    public TextMeshProUGUI CurrentlyCorrectP2;
    public TextMeshProUGUI PossibleValues;

    public void Update()
    {
        ElapsedTime.text = GameManager.Instance.GetTimeSinceGameStart.ToString();
        RoundNumber.text = GameManager.Instance.GetRound.ToString();
        Efficienzy.text = GameManager.Instance.RoundEfficiency().ToString();
        if(GameManager.Instance.GetPlayer1 != null )
            CurrentlyCorrectP1.text = GameManager.Instance.GetPlayer1.pressedValuesAreCorrectBoolSync.GetBoolValue.ToString();
        if (GameManager.Instance.GetPlayer2 != null) 
            CurrentlyCorrectP2.text = GameManager.Instance.GetPlayer2.pressedValuesAreCorrectBoolSync.GetBoolValue.ToString();
        PossibleValues.text = GameManager.Instance.RoundRules.GetElementCountThisRound(GameManager.Instance.GetRound).ToString();
    }

}
