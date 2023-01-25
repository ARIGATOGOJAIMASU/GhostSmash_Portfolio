using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class ThrowState : State_Base
    {
        public override void Setting()
        {
            StateType = PLAYERSTATE.THROW;
        }

        public override void StateEnter()
        {
            if(PhotonNetwork.IsConnected)
            {
                if (state_Machine._PrevState == PLAYERSTATE.JUMP)
                {
                    GameManager.Instance.AnimationTrigger("JumpThrow", Pv_ID);
                }
                else
                {
                    GameManager.Instance.AnimationTrigger("Throw", Pv_ID);
                }
            }
            else
            {
                Owner_animator.SetTrigger("Throw");
            }
        }

        public override void Update()
        {
            AnimatorStateInfo info = Owner_animator.GetCurrentAnimatorStateInfo(0);

            if((info.normalizedTime > 0.95) && 
                ((info.IsName("Throw") || info.IsName("JumpThrow"))))
            {
                if(GroundPos.position.y > Owner.position.y)
                {
                    state_Machine.ChangeState(PLAYERSTATE.IDLE);
                }
                else
                {
                    state_Machine.ChangeState(PLAYERSTATE.AIR);
                }
            }
        }
    }
}
