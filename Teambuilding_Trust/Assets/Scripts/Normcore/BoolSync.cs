using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class BoolSync : RealtimeComponent
{
    private BoolSyncModel _model;
    private bool boolValue;

    public delegate void BoolValueChanged();
    public event BoolValueChanged boolValueChanged;

    private BoolSyncModel model
    {
        set
        {
            if(_model != null)
            {
                _model.boolValueDidChange -= BoolValueDidChange;
            }
            // Store the model
            _model = value;

            if(_model != null)
            {
                UpdateBoolValue();
                _model.boolValueDidChange += BoolValueDidChange;
            }
            Debug.Log("MODEL SUCC SET");
        }
    }

    private void BoolValueDidChange(BoolSyncModel model, bool value)
    {
        UpdateBoolValue();
    }

    private void UpdateBoolValue()
    {
        boolValue = _model.boolValue;
    }

    public bool GetBoolValue { get { return boolValue; } }

    public void SetBoolValue(bool value)
    {
        _model.boolValue = value;
        boolValueChanged.Invoke();
    }
}