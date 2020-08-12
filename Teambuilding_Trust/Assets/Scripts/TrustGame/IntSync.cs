using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.VFX;

public class IntSync : RealtimeComponent
{
    private IntSyncModel _model;
    private int intValue;

    public delegate void IntValueChanged();
    public event IntValueChanged intValueChanged;

    private IntSyncModel model
    {
        set
        {
            if (_model != null)
            {
                _model.intValueDidChange -= IntValueDidChanged;
            }
            // Store the model
            _model = value;

            if (_model != null)
            {
                UpdateIntValue();
                _model.intValueDidChange += IntValueDidChanged;
            }
        }
    }

    private void IntValueDidChanged(IntSyncModel model, int value)
    {
        UpdateIntValue();
    }

    private void UpdateIntValue()
    {
        intValue = _model.intValue;
        intValueChanged?.Invoke();
    }

    public int GetIntValue { get { return _model.intValue; } }

    public void SetIntValue(int value)
    {
        _model.intValue = value;
    }

}