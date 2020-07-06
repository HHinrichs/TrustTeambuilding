using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class TransformSync : RealtimeComponent
{

    private TransformSyncModel _model;

    private Vector3 position;
    private Vector3 rotation;
    private Vector3 scale;

    public Vector3 Position { get { return position; } }
    public Vector3 Rotation { get { return position; } }
    public Vector3 Scale { get { return position; } }


    public delegate void PositionValueChanged();
    public event PositionValueChanged positionChanged;

    public delegate void RotationValueChanged();
    public event RotationValueChanged rotationChanged;

    public delegate void ScaleValueChanged();
    public event ScaleValueChanged scaleChanged;

    private TransformSyncModel model
    {
        set
        {
            if (_model != null)
            {
                // Unregister from events
                _model.positionDidChange -= TransformDidChange;
            }

            // Store the model
            _model = value;

            if (_model != null)
            {
                // Update the mesh render to match the new model
                UpdateTransform();

                // Register for events so we'll know if the color changes later
                _model.positionDidChange += TransformDidChange;
            }
        }
    }

    private void TransformDidChange(TransformSyncModel model, Vector3 position)
    {
        // Update the mesh renderer
        UpdateTransform();
    }

    private void UpdateTransform()
    {
        // Get the color from the model and set it on the mesh renderer.
        position = _model.position;
    }

    public void SetTransform(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        // Set the color on the model
        // This will fire the colorChanged event on the model, which will update the renderer for both the local player and all remote players.
        _model.position = position;
    }
}