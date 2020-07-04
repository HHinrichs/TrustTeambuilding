using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LayersToRenderSetting", order = 1)]
public class LayersToRender : ScriptableObject
{
    [SerializeField] private LayerMask customLayerMask;

    public LayerMask CustomLayerMask { get { return customLayerMask; } }
}
