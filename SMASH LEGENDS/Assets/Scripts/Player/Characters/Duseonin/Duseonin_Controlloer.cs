using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Junpyo
{
    public class Duseonin_Controlloer : PlayerController_FSM
    {
        private delegate int tests(int a, int b);

        private void Start()
        {
            if(photonView.IsMine)
            {
                state_Machine.StateAdd(new Duseonin_JumpAttackState(), PLAYERSTATE.DUSEONIN_JUMPATTACK);
            }
        }

        private void test()
        {

        }
    }
}
