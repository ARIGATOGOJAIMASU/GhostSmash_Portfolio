using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class TrueLove_JumpAttackSuccesState : State_Base
    {
        bool Start;

        public override void Setting()
        {
            StateType = PLAYERSTATE.TRUELOVE_JUMPATTACKSUCCESS;
        }

        public override void StateEnter()
        {
            Owner.position += new Vector3(0, 0.5f, 0);

            Owner_rigidbody.angularVelocity = Vector3.zero;
            Owner_rigidbody.velocity = Vector3.zero;

            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("JumpAttackSucces", true, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("JumpAttackSucces", true);
            }

            Owner_rigidbody.velocity = (-Owner.forward * 7.0f) + new Vector3(0.0f, 12.0f, 0);
        }

        public override void Update()
        {
            AnimatorStateInfo info = Owner_animator.GetCurrentAnimatorStateInfo(0);

            if (info.normalizedTime > 0.95 && info.IsName("JumpSkillSuccess"))
            {
                state_Machine.ChangeState(PLAYERSTATE.AIR);
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("JumpAttackSucces", false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("JumpAttackSucces", false);
            }
        }
    }
}
