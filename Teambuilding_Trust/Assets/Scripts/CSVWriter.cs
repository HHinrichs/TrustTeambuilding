using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using System.Globalization;

namespace CSVInteractions{
    public static class CSVWriter
    {

        static string filepath = "A:/GitRepos/TrustTeambuilding/Teambuilding_Trust/MasterarbeitCSVLog/teambuildingSpectatorLog.txt";

        public static void addRecord(string startDate, string startTime, float gameTime, float roundTime, int round, float efficiency, bool IKAvatar, int podest0, int podest0Fails, int podest1, int podest1Fails, int podest2, int podest2Fails)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
                {
                    file.WriteLine(startDate +"," + startTime + "," + gameTime.ToString("F", CultureInfo.CreateSpecificCulture("en-US")) + "," + roundTime.ToString("F", CultureInfo.CreateSpecificCulture("en-US")) + "," + round.ToString() + "," +efficiency.ToString("F", CultureInfo.CreateSpecificCulture("en-US")) + "," +IKAvatar + "," + podest0 +","+ podest0Fails+"," + podest1 + "," + podest1Fails+"," + podest2 + "," + podest2Fails);
                }
            }
            catch(Exception e)
            {
                Debug.LogError("Something went wrong wile writing out to CSV "+e);
            }
        }

        public static void addStartRoundRecord()
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
                {
                    file.WriteLine("-----------NewGameStarted------------------");
                    file.WriteLine("startDate, startTime, time, roundTime, round, efficiency, IKAvatar, podest0, podest0Fails, podest1, podest1Fails, podest2, podest2Fails");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Something went wrong wile writing out to CSV " + e);
            }
        }

        public static void addEndRoundRecord()
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
                {
                    file.WriteLine("-----------Game Ended------------------");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Something went wrong wile writing out to CSV " + e);
            }
        }
    }
}
