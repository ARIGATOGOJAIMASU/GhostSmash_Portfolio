using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class TrueLove_JumpAttack : State_Base
    {
        public override void Setting()
        {
            StateType = PLAYERSTATE.TRUELOVE_JUMPATTACK;
        }

        public override void StateEnter()
        {
            Owner_rigidbody.velocity = Vector3.zero;
            
            if(PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("JumpAttack",true,Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("JumpAttack", true);
            }
        }

        public override void Update()
        {
            if(GroundPos.position.y > Owner.position.y)
            {
                state_Machine.ChangeState(PLAYERSTATE.TRUELOVE_JUMPATTACKFAILL);
            }
        }

        public override void StateExit()
        {
            Owner_rigidbody.velocity = Vector3.zero;

            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("JumpAttack", false, Pv_ID);
                GameManager.Instance.Attack((int)SKILLTYPE.JUMPATTACK, false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("JumpAttack", false);
                Owner_Script.playerSkill.AttackOff((int)SKILLTYPE.JUMPATTACK);
            }
        }
    }
}
