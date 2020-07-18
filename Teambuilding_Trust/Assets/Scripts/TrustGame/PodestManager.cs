using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodestManager : MonoBehaviour
{
    public int PodestNumber;
    [SerializeField] Material P1Material;
    [SerializeField] Material P2Material;
    [SerializeField] Material DefaultMaterial;

    private List<ButtonController> ButtonControllers = new List<ButtonController>();
    private bool isCurrentLeader = false;
    private int buttonsInChildrenCount = 0;
    private int lastPressedValue = 99;

    private List<int> buttonValuesP1 = new List<int>();
    private List<int> buttonValuesP2 = new List<int>();

    // Getter, Setter
    public bool IsCurrentLeader { get { return isCurrentLeader; } set { isCurrentLeader = value; } }
    public int ButtonsInChildrenCount { get { return buttonsInChildrenCount; } }
    private void Start()
    {
        ButtonController[] buttonControllertmp = GetComponentsInChildren<ButtonController>();
        foreach(ButtonController child in buttonControllertmp)
        {
            ButtonControllers.Add(child);
            child.buttonPressed += GetPressedValue;
            buttonsInChildrenCount++;
            child.ResetMaterials(DefaultMaterial);
        }
    }

    public void SetButtonValues(List<int> buttonValuesP1)
    {
        this.buttonValuesP1 = buttonValuesP1;

        for(int i = 0; i < ButtonControllers.Count; ++i)
        {
            if (buttonValuesP1.Contains(i))
            {
                ButtonControllers[i].ChangeMaterialP1(P1Material);
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
                ButtonControllers[i].ChangeMaterialP1(P1Material);
            }
            if (buttonValuesP2.Contains(i))
            {
                ButtonControllers[i].ChangeMaterialP2(P2Material);
            }
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
            buttonController.ResetMaterials(DefaultMaterial);
        }
    }

    public void ResetAll()
    {
        lastPressedValue = 99;
        isCurrentLeader = false;
        ResetButtonsValues();
        ResetMaterial();
    }

    private void GetPressedValue(int buttonNumber)
    {
        if (IsCurrentLeader == true)
            return;

        lastPressedValue = buttonNumber;
    }

    private void CheckIfPressedValuesAreCorrect()
    {

    }
}
