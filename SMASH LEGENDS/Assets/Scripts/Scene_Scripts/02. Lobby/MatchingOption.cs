using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Juhyung
{
    struct MatchingOption
    {
        public string s_Name;         // Room Name
        public int i_GameMode;     // GameMode
        public byte b_maxPlayer;    // Room's MaxPlayer Count
        public float f_MaxTime;      // Game Play Max Time

        public void SetOption1() // Set Gamemode 1
        {
            s_Name = "1vs1";
            i_GameMode = 0;
            b_maxPlayer = 2;
            f_MaxTime = 20.0f;
        }

        public void SetOption2() // Set Gamemode 2
        {
            s_Name = "3vs3";
            i_GameMode = 1;
            b_maxPlayer = 6;
            f_MaxTime = 20.0f;
        }

        public void SetOption3() // Set Gamemode 3
        {
            s_Name = "3vs3(2)";
            i_GameMode = 2;
            b_maxPlayer = 6;
            f_MaxTime = 20.0f;
        }
    }
}
