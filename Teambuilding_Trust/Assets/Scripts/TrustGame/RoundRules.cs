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
        public List<int> ButtonsToPressP1;
        public List<int> ButtonsToPressP2;
    }

    public RoundValues[] roundValues;
    public int GetElementCountThisRound(int round)
    {
        return roundValues[round].ElementCount;
    }

    public List<int> GetButtonsForPlayer1(int round)
    {
        return new List<int>(roundValues[round].ButtonsToPressP1);
    }

    public List<int> GetButtonsForPlayer2(int round)
    {
        return new List<int>(roundValues[round].ButtonsToPressP2);
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
