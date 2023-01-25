using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class Gangnim_JumpSkillState : State_Base
    {
        public override void Setting()
        {
            StateType = PLAYERSTATE.GANGNIM_JUMPSKILLSTATE;
        }

        public override void StateEnter()
        {
            if(PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("JumpSkill", true,Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("JumpSkill", true);
            }

            Owner_rigidbody.velocity = -Owner.up * 12.0f;
        }

        public override void Update()
        {
            //땅에 닿을때 애니메이션을 다시 재생
            if(Owner.position.y < GroundPos.position.y)
            {
                if (PhotonNetwork.IsConnected)
                {
                    GameManager.Instance.AnimationPlay(Pv_ID, true);
                }
                else
                {
                    Owner_animator.StopPlayback();
                }
            }

            AnimatorStateInfo info = Owner_animator.GetCurrentAnimatorStateInfo(0);

            if(info.normalizedTime > 0.97 && info.IsName("JumpSkill"))
            {
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("JumpSkill", false, Pv_ID);
                GameManager.Instance.AnimationPlay(Pv_ID, true);
            }
            else
            {
                Owner_animator.SetBool("JumpSkill", false);
                Owner_animator.StopPlayback();
            }

        }
    }
}
