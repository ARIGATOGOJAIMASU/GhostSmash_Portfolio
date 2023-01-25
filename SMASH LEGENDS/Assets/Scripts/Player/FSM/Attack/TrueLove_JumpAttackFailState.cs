using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class TrueLove_JumpAttackFail : State_Base
    {
        float Delay = 0.5f;
        float Timer;

        public override void Setting()
        {
            StateType = PLAYERSTATE.TRUELOVE_JUMPATTACKFAILL;
        }

        public override void StateEnter()
        {
            Owner_rigidbody.velocity = Owner.forward * 7.0f;

            if(PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("JumpAttackFail", true, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("JumpAttackFail", true);
            }
        }

        public override void Update()
        {
            Timer += Time.deltaTime;

            if(Timer > Delay)
            {
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }

        public override void StateExit()
        {
            Timer = 0.0f;

            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("JumpAttackFail", false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("JumpAttackFail", false);
            }
        }
    }
}
