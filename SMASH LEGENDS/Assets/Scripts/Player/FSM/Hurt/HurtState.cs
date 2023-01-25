using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    class HurtState : State_Base
    {
        //private bool IsHurt;
        private float HurtDelayTime = 0.5f;
        private float HurtCurTime;

        public override void Setting() { StateType = PLAYERSTATE.HURT; }
        public override void StateEnter()
        {
            //Owner_animator.SetBool("Hurt", true);
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationTrigger("Hurt", Pv_ID);
            }
            else
            {
                Owner_animator.SetTrigger("Hurt");
            }
        }

        public override void Update()
        {
            HurtCurTime += Time.deltaTime;

            if (HurtCurTime >= HurtDelayTime)
            {
                if (GroundPos.position.y > Owner.position.y)
                {
                    state_Machine.ChangeState(PLAYERSTATE.IDLE);
                }
                else
                {
                    state_Machine.ChangeState(PLAYERSTATE.AIR);
                }
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationResetTrigger("Hurt", Pv_ID);
            }
            else
            {
                Owner_animator.ResetTrigger("Hurt");
            }

            HurtCurTime = 0.0f;
        }
    }
}
