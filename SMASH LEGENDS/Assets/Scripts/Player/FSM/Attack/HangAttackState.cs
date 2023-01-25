using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    class HangAttackState : State_Base
    {
        public override void Setting() { StateType = PLAYERSTATE.HANGATTACK; }

        public override void StateEnter()
        {
            //Player위치 Ground위치로 조정
            //Owner.position = GroundPos.position;

            //애니메이션 재생
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("HangAttack", true, Pv_ID);
                GameManager.Instance.AnimationBool("isAttack", true, Pv_ID);
                GameManager.Instance.IKEvent(PLAYERSTATE.HANGATTACK, true, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("HangAttack", true);
                Owner_animator.SetBool("IsAttack", true);
                OwnerIK.IKEvent(PLAYERSTATE.HANGATTACK, true);
            }
        }

        public override void Update()
        {
            //공격이 끝날 때 쯤 Idle상태로 전환
            if (Owner_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f && Owner_animator.GetCurrentAnimatorStateInfo(0).IsName("HangAttack"))
            {
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }

        public override void StateExit()
        {
            //애니메이션 재생
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("HangAttack", false, Pv_ID);
                GameManager.Instance.AnimationBool("isAttack", false, Pv_ID);
                GameManager.Instance.IKEvent(PLAYERSTATE.HANGATTACK, true, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("HangAttack", false);
                Owner_animator.SetBool("IsAttack", false);
                OwnerIK.IKEvent(PLAYERSTATE.HANGATTACK, false);
            }

            Owner.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
}
