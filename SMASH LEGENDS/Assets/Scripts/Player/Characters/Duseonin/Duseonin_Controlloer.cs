using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class Duseonin_Controlloer : PlayerController_FSM
    {
        private void Start()
        {
            if(photonView.IsMine)
            {
                state_Machine.StateAdd(new Duseonin_JumpAttackState(), PLAYERSTATE.DUSEONIN_JUMPATTACK);
            }
        }
    }
}
