using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundRule", menuName = "ScriptableObjects/RoundRule")]
public class RoundRules : ScriptableObject
{
    [System.Serializable]
    public struct RoundValues{
        public int RoundNumber;
        public int ElementCount;
    }

    public RoundValues[] roundValues;
    public int GetRoundRules(int round)
    {
        return roundValues[round].ElementCount;
    }
}
