using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldAcceptor : MonoBehaviour
{
    IntSync roundNumberToStartWithIntSync;
    public TMP_InputField tmpInputField;
    private void Start()
    {
        roundNumberToStartWithIntSync = GetComponent<IntSync>();
    }

    public void SetNetRoundNumberToStartWith()
    {
        string tmpInputFieldText = tmpInputField.text;
        int valueToInt;
        int.TryParse(tmpInputFieldText, out valueToInt);
        roundNumberToStartWithIntSync.SetIntValue(valueToInt);
    }

}
