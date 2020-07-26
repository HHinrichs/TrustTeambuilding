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
        public int WhoIsLeader;
        public int WhoIsPlayer1;
        public int WhoIsPlayer2;
        public int[] ButtonsToPressP1;
        public int[] ButtonsToPressP2;
    }

    public RoundValues[] roundValues;
    public int GetElementCountThisRound(int round)
    {
        return roundValues[round].ElementCount;
    }

    public int[] GetButtonsForPlayer1(int round)
    {
        return roundValues[round].ButtonsToPressP1;
    }

    public int[] GetButtonsForPlayer2(int round)
    {
        return roundValues[round].ButtonsToPressP2;
    }

    public int GetWhoIsLeader(int round)
    {
        return roundValues[round].WhoIsLeader;
    }

    public int GetWhoIsPlayer1(int round)
    {
        return roundValues[round].WhoIsPlayer1;
    }


    public int GetWhoIsPlayer2(int round)
    {
        return roundValues[round].WhoIsPlayer2;
    }

}
