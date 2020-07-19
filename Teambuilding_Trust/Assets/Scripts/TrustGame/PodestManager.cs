using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
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
    private List<int> pressedButtonNumbers = new List<int>();
    private bool isCurrentLeader = false;
    private bool isPlayer1 = false;
    private bool isPlayer2 = false;
    private int buttonsInChildrenCount = 0;
    private int lastPressedValue = 99;
    private List<int> buttonValuesP1 = new List<int>();
    private List<int> buttonValuesP2 = new List<int>();

    // Getter, Setter
    public bool IsCurrentLeader { get { return isCurrentLeader; } set { isCurrentLeader = value; } }
    public bool IsPlayer1 { get { return isPlayer1; } set { isPlayer1 = value; } }
    public bool IsPlayer2 { get { return isPlayer2; } set { isPlayer2 = value; } }
    public int ButtonsInChildrenCount { get { return buttonsInChildrenCount; } }
    private void Start()
    {
        ButtonController[] buttonControllertmp = GetComponentsInChildren<ButtonController>();
        foreach(ButtonController child in buttonControllertmp)
        {
            ButtonControllers.Add(child);
            child.buttonPressed += GetPressedValue;
            buttonsInChildrenCount++;
            child.ResetMaterials(DefaultSymbolMaterial);
        }
    }

    public void SetButtonValues(List<int> buttonValuesP1)
    {
        this.buttonValuesP1 = buttonValuesP1;

        for(int i = 0; i < ButtonControllers.Count; ++i)
        {
            if (buttonValuesP1.Contains(i))
            {
                ButtonControllers[i].ChangeMaterialP1(P1SymbolMaterial);
            }
        }
    }

    public void SetButtonValues(List<int> buttonValuesP1, List<int> buttonValuesP2)
    {
        this.buttonValuesP1 = buttonValuesP1;
        this.buttonValuesP2 = buttonValuesP2;

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
    }

    public void SetPlayerIndicators()
    {
        if (isCurrentLeader)
        {
            PlayerColorIndicatorPlane.material = PlayerColorMaterialBlack;
            return;
        }
        if (isPlayer1)
        {
            PlayerColorIndicatorPlane.material = PlayerColorMaterialGreen;
            return;
        }
        if (isPlayer2)
        {
            PlayerColorIndicatorPlane.material = PlayerColorMaterialRed;
            return;
        }

    }

    public void ResetButtonsValues()
    {
        buttonValuesP1.Clear();
        buttonValuesP2.Clear();
    }

    public void ResetMaterial()
    {
        foreach (ButtonController buttonController in ButtonControllers)
        {
            buttonController.ResetMaterials(DefaultSymbolMaterial);
        }
    }

    public void ResetAll()
    {
        PlayerColorIndicatorPlane.material = PlayerColorMaterialBlack;
        // Resets the pressed Button Numbers
        pressedButtonNumbers.Clear();
        lastPressedValue = 99;
        IsCurrentLeader = false;
        IsPlayer1 = false;
        IsPlayer2 = false;
        ResetButtonsValues();
        ResetMaterial();
    }

    private void GetPressedValue(int buttonNumber)
    {
        if (IsCurrentLeader == true)
            return;
        // Highlight the Material of the Button if the Pressed Values is received
        ButtonControllers[buttonNumber].HighlightButtonAndSetPressedState();
        
        // Adds the Pressed Button Number to the buttons;
        pressedButtonNumbers.Add(buttonNumber);

        lastPressedValue = buttonNumber;
    }

    private void CheckIfPressedValuesAreCorrect()
    {

    }
}
