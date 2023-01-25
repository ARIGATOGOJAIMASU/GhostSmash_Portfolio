using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class JumpAttackState : State_Base
    {
        float Magnitude = 17;

        public override void Setting() { StateType = PLAYERSTATE.JUMPATTACK; }

        public override void StateEnter()
        {
            if (PhotonNetwork.IsConnected)
            {
                if (Owner_Script.playerInformation.IsAttackUplayer)
                {
                    GameManager.Instance.AnimationSetLayerWeight(1, 1, Pv_ID);
                }

                GameManager.Instance.AnimationBool("JumpAttack", true, Pv_ID);
            }
            else
            {
                if (Owner_Script.playerInformation.IsAttackUplayer)
                {
                    Owner_animator.SetLayerWeight(1, 1);
                }

                Owner_animator.SetBool("JumpAttack", true);
            }
        }

        public override void Update()
        {
            AnimatorStateInfo info;

            if (Owner_Script.playerInformation.IsAttackUplayer)
            {
                info = Owner_animator.GetCurrentAnimatorStateInfo(1);
            }
            else
            {
                info = Owner_animator.GetCurrentAnimatorStateInfo(0);
            }

            if (info.normalizedTime > 0.95f && info.IsName("JumpAttack"))
            {
                //스킬 애니메이션이 끝난 이 후 IdleState로 전환
                 state_Machine.ChangeState(PLAYERSTATE.AIR);
            }

            if (Owner_rigidbody.velocity.y <= 0.2f)
            {
                Owner_rigidbody.velocity -= new Vector3(0, Time.deltaTime * Magnitude, 0);
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                if (Owner_Script.playerInformation.IsAttackUplayer)
                {
                    GameManager.Instance.AnimationSetLayerWeight(1, 0, Pv_ID);
                }

                GameManager.Instance.AnimationBool("JumpAttack", false, Pv_ID);
            }
            else
            {
                if (Owner_Script.playerInformation.IsAttackUplayer)
                {
                    Owner_animator.SetLayerWeight(1, 0);
                }

                Owner_animator.SetBool("JumpAttack", false);
            }
        }
    }
}
