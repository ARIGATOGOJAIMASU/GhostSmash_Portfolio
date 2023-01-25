using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class CurceState : State_Base
    {
        float CurceTime = 3.0f;

        public override void Setting() { StateType =  PLAYERSTATE.CURCE; }

        public override void StateEnter()
        {
            Owner_Script.playerInformation.RunSpeed *= 0.5f;
            Owner_Script.playerInformation.JumpDistance *= 0.5f;
            Owner_Script.playerInformation.Curce = true;

            CoroutineHelper.StartCoroutine(CurceDuration());

            if(state_Machine._PrevState == PLAYERSTATE.RUN)
            {
                state_Machine.ChangeState(PLAYERSTATE.RUN);
            }
            else if (GroundPos.position.y < Owner.position.y)
            {
                state_Machine.ChangeState(PLAYERSTATE.AIR);
            }
            else state_Machine.ChangeState(PLAYERSTATE.IDLE);
        }

        public IEnumerator CurceDuration()
        {
            yield return new WaitForSeconds(CurceTime);

            Owner_Script.playerInformation.RunSpeed *= 2.0f;
            Owner_Script.playerInformation.JumpDistance *= 2.0f;
            Owner_Script.playerInformation.Curce = false;
        }
    }
}
