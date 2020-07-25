using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class PodestManager : MonoBehaviour
{
    public int PodestNumber;
    [SerializeField] Material P1SymbolMaterial;
    [SerializeField] Material P2SymbolMaterial;
    [SerializeField] Material DefaultSymbolMaterial;
    [SerializeField] MeshRenderer PlayerColorIndicatorPlane;
    [SerializeField] Material PlayerColorMaterialRed;
    [SerializeField] Material PlayerColorMaterialGreen;
    [SerializeField] Material PlayerColorMaterialBlack;

    private List<ButtonController> ButtonControllers = new List<ButtonController>();
    private RoundRules roundRules;
    private int currentRound = 0;
    private List<int> pressedButtonNumbers = new List<int>();

    private int playerNumber = 99;

    private int buttonsInChildrenCount = 0;
    private int lastPressedValue = 99;
    private List<int> buttonValuesP1 = new List<int>();
    private List<int> buttonValuesP2 = new List<int>();

    private bool pressedValuesAreCorrect = false;

    // Getter, Setter
    public int SetCurrentRound { set { currentRound = value; } }
    public RoundRules SetRoundRules { set { roundRules = value; } }
    public int ButtonsInChildrenCount { get { return buttonsInChildrenCount; } }

    public int PlayerNumber { get { return playerNumber; } set { playerNumber = value; } }
    public bool GetPressedValuesAreCorrect { get { return pressedValuesAreCorrect; } }

    public delegate void PressedValueChanged();
    public event PressedValueChanged allButtonsHaveBeenPressed;

    private void Start()
    {
        ButtonController[] buttonControllertmp = GetComponentsInChildren<ButtonController>();
        foreach(ButtonController child in buttonControllertmp)
        {
            ButtonControllers.Add(child);
            child.buttonPressed += GetPressedValueFromPlayer;
            buttonsInChildrenCount++;
            child.ResetMaterials(DefaultSymbolMaterial);
        }
    }

    public void SetButtonValues(List<int> buttonValues)
    {
        switch (PlayerNumber)
        {
            case 0:
                for (int i = 0; i < ButtonControllers.Count; ++i)
                {
                    if (buttonValues.Contains(i))
                    {
                        ButtonControllers[i].ChangeMaterialP1(P1SymbolMaterial);
                    }
                }
                break;

            case 1: this.buttonValuesP1 = buttonValues;
                break;
            case 2: this.buttonValuesP2 = buttonValues;
                break;
        }

    }

    public void SetButtonValues(List<int> buttonValuesP1, List<int> buttonValuesP2)
    {
        switch (PlayerNumber)
        {
            case 0:
                for (int i = 0; i < ButtonControllers.Count; ++i)
                {
                    if (buttonValuesP1.Contains(i))
                    {
                        ButtonControllers[i].ChangeMaterialP1(P1SymbolMaterial);
                    }
                    if (buttonValuesP2.Contains(i))
                    {
                        ButtonControllers[i].ChangeMaterialP2(P2SymbolMaterial);
                    }
                }
                break;

            case 1:
                this.buttonValuesP1 = buttonValuesP1;
                break;
            case 2:
                this.buttonValuesP2 = buttonValuesP2;
                break;
        }
    }

    public void SetPlayerIndicators()
    {
        switch (PlayerNumber)
        {
            case 0:
                PlayerColorIndicatorPlane.material = PlayerColorMaterialBlack;
                break;
            case 1:
                PlayerColorIndicatorPlane.material = PlayerColorMaterialRed;
                break;
            case 2:
                PlayerColorIndicatorPlane.material = PlayerColorMaterialGreen;
                break;
            default:
                PlayerColorIndicatorPlane.enabled = false;
                break;
        }
    }

    public void ResetButtons()
    {
        foreach (ButtonController button in ButtonControllers)
        {
            button.IsPressed = false;
            button.ResetMaterials(DefaultSymbolMaterial);
            button.SetNonHighlightMaterial();
        }

        buttonValuesP1.Clear();
        buttonValuesP2.Clear();
        pressedButtonNumbers.Clear();
    }

    public void ResetAll()
    {
        playerNumber = 99;
        PlayerColorIndicatorPlane.material = PlayerColorMaterialBlack;
        SetCurrentRound = 0;
        lastPressedValue = 99;
        pressedValuesAreCorrect = false;
        ResetButtons();
    }

    private void GetPressedValueFromPlayer(int buttonNumber)
    {
        if (!GameManager.Instance.gameIsRunning)
            return;

        if (PlayerNumber == 0)
            return;

        // If the Button is Pressed twice, remove it from the List
        if (ButtonControllers[buttonNumber].IsPressed)
        {
            Debug.Log("Deselecting Button " + ButtonControllers[buttonNumber]);
            ButtonControllers[buttonNumber].DeselectButton();
            pressedButtonNumbers.Remove(buttonNumber);
            return;
        }

        // Jump out if no button is deselected and the numbers of possible pressed buttons is received.
        if (pressedButtonNumbers.Count == roundRules.GetElementCountThisRound(currentRound))
            return;

        // Highlight the Material of the Button if the Pressed Values is received
        ButtonControllers[buttonNumber].SelectButton();
        
        // Adds the Pressed Button Number to the buttons;
        pressedButtonNumbers.Add(buttonNumber);

        lastPressedValue = buttonNumber;

        // Jumps in and Checks if pressedValuesAreCorrect if all Buttons this Round have been pressed and notifys the Gamemanager about an status update
        if (pressedButtonNumbers.Count == roundRules.GetElementCountThisRound(currentRound))
        {
            pressedValuesAreCorrect = CheckIfPressedValueFromPlayerIsCorrect();
            allButtonsHaveBeenPressed.Invoke();
        }

    }

    private bool CheckIfPressedValueFromPlayerIsCorrect()
    {
        bool isCorrectValue = true;
        List<bool> pressedValueCheckerP1 = new List<bool>();
        List<bool> pressedValueCheckerP2 = new List<bool>();
        switch (PlayerNumber)
        {
            case 1:
                Debug.Log("Im in P1");
                for (int i = 0; i < pressedButtonNumbers.Count; ++i)
                {
                    if (buttonValuesP1.Contains(pressedButtonNumbers[i]))
                        pressedValueCheckerP1.Add(true);
                    else
                        pressedValueCheckerP1.Add(false);
                }

                for(int i = 0; i < pressedValueCheckerP1.Count; ++i)
                {
                    isCorrectValue &= pressedValueCheckerP1[i];
                    Debug.Log("P1 "+ isCorrectValue);
                }
                break;

            case 2:
                Debug.Log("Im in P2");
                for (int i = 0; i < pressedButtonNumbers.Count; ++i)
                {
                    if (buttonValuesP2.Contains(pressedButtonNumbers[i]))
                        pressedValueCheckerP2.Add(true);
                    else
                        pressedValueCheckerP2.Add(false);
                }
                for (int i = 0; i < pressedValueCheckerP2.Count; ++i)
                {
                    isCorrectValue &= pressedValueCheckerP2[i];
                    Debug.Log("P2 " + isCorrectValue);
                }
                break;
        }

        return isCorrectValue;
    }
}
