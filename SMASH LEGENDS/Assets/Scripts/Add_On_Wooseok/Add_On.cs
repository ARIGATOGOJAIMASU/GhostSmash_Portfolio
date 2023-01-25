using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Junpyo;


namespace Wooseok
{
    

    public abstract class Add_On : MonoBehaviour
    {
        public enum ADD_ON_TYPE
        {
            GENERAL = 0,
            SPECIAL = 1,
            ULTIMATE = 2,

            END = 99
        }



        // Start is called before the first frame update
        public enum ADD_ON_TIMING
        {
            BEFORESKILL = 0, // 스킬 자체를 교체하는 에드온들
            AFTERSKILL = 1, //  스킬의 단순 스펙만 바꾸는 에드온들
            INTERACTION = 2, // 전투중 조건에 따라 발동하는 에드온들

            END = 99
        }

        public ADD_ON_TIMING Timing;
        public ADD_ON_TYPE Type;
        //public int level = 0;
        

        public abstract void Effect(PlayerController_FSM user, PlayerController_FSM target = null);
    }
}