using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Button kickButton;
    public TextMeshProUGUI PlayerNumberString;

    private int playerNumber;

    public int PlayerNumber { get { return playerNumber; } set { playerNumber = value; } }

}
