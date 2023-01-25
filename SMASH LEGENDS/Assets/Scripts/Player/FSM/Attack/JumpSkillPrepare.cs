using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class JumpSkillPrepare : State_Base
    {
        public override void Setting()
        {
            StateType = PLAYERSTATE.JUMPSKILLPREPARE;
        }

        public override void StateEnter()
        {
            if(PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("JumpSkillPrepare", true,Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("JumpSkillPrepare", true);
            }

            Owner_Script.UseSkill();
        }

        public override void Update()
        {
            AnimatorStateInfo info = Owner_animator.GetCurrentAnimatorStateInfo(0); 

            if(info.normalizedTime > 0.97 && info.IsName("JumpSkillPrepare"))
            {
                if (Owner_Script.playerInformation.Name == CHARACTERNAME.GANGNIM)
                {
                    state_Machine.ChangeState(PLAYERSTATE.GANGNIM_JUMPSKILLSTATE);
                }
                else
                {
                    state_Machine.ChangeState(PLAYERSTATE.JUMPSKILL);
                }
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("JumpSkillPrepare", false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("JumpSkillPrepare", false);
            }
        }
    }
}
