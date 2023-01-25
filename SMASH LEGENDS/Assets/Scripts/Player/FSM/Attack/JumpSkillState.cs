using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class JumpSkillState : State_Base
    {
        public override void Setting()
        {
            StateType = PLAYERSTATE.JUMPSKILL;
        }

        public override void StateEnter()
        {
            if (PhotonNetwork.IsConnected)
            {
                if (Owner_Script.playerInformation.IsSkillUplayer)
                {
                    GameManager.Instance.AnimationSetLayerWeight(1, 1, Pv_ID);
                }

                GameManager.Instance.AnimationBool("JumpSkill", true, Pv_ID);
                GameManager.Instance.IKEvent(PLAYERSTATE.JUMPSKILL, true, Pv_ID);
            }
            else
            {
                if (Owner_Script.playerInformation.IsSkillUplayer)
                {
                    Owner_animator.SetLayerWeight(1, 1);
                }

                Owner_animator.SetBool("JumpSkill", true);
                Owner_Script.IK_Controller.IKEvent(PLAYERSTATE.JUMPSKILL, true);
            }

            Owner_Script.UseSkill();
        }

        public override void Update()
        {
            AnimatorStateInfo info;

            if(Owner_Script.playerInformation.IsSkillUplayer)
            {
                info = Owner_animator.GetCurrentAnimatorStateInfo(1);
            }
            else
            {
                info = Owner_animator.GetCurrentAnimatorStateInfo(0);
            }

            if (info.normalizedTime > 0.97f && info.IsName("JumpSkill"))
            {
                if (GroundPos.position.y < Owner.position.y)
                {
                    //스킬 애니메이션이 끝난 이 후 IdleState로 전환
                    state_Machine.ChangeState(PLAYERSTATE.AIR);
                }
                else
                {
                    state_Machine.ChangeState(PLAYERSTATE.IDLE);
                }
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                if (Owner_Script.playerInformation.IsSkillUplayer)
                {
                    GameManager.Instance.AnimationSetLayerWeight(1, 0, Pv_ID);
                }

                GameManager.Instance.AnimationBool("JumpSkill", false, Pv_ID);
                GameManager.Instance.IKEvent(PLAYERSTATE.JUMPSKILL, false, Pv_ID);
            }
            else
            {
                if (Owner_Script.playerInformation.IsSkillUplayer)
                {
                    Owner_animator.SetLayerWeight(1, 0);
                }

                Owner_animator.SetBool("JumpSkill", false);
                Owner_Script.IK_Controller.IKEvent(PLAYERSTATE.JUMPSKILL, false);
            }
        }
    }
}
