using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class TrueLove_Controller : PlayerController_FSM
    {
        private void Start()
        {
            if (IsMine)
            {
                state_Machine.StateAdd(new TrueLove_JumpAttack(), PLAYERSTATE.TRUELOVE_JUMPATTACK);
                state_Machine.StateAdd(new TrueLove_JumpAttackFail(), PLAYERSTATE.TRUELOVE_JUMPATTACKFAILL);
                state_Machine.StateAdd(new TrueLove_JumpAttackSuccesState(), PLAYERSTATE.TRUELOVE_JUMPATTACKSUCCESS);
            }
        }

        public void JumpAttackSuccess()
        {
            if (IsMine)
            {
                state_Machine.ChangeState(PLAYERSTATE.TRUELOVE_JUMPATTACKSUCCESS);
            }
        }
    }
}
