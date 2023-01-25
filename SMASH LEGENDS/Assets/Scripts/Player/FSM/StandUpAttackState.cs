using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class StandUpAttackState : State_Base
    {
        public override void Setting()
        {
            StateType = PLAYERSTATE.STANDUPATTACK;
        }

        public override void StateEnter()
        {
            //키 값 받기
            MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            //키값으로 시선 변경
            Owner.LookAt(Owner.position + MoveDirection);

            //애니메이션 재생 
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("StandUpAttack", true, Pv_ID);
                GameManager.Instance.IKEvent(PLAYERSTATE.STANDUPATTACK, true, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("StandUpAttack", true);
                OwnerIK.IKEvent(PLAYERSTATE.STANDUPATTACK, true);
            }

        }

        public override void Update()
        {
            AnimatorStateInfo Info = Owner_animator.GetCurrentAnimatorStateInfo(0);

            if (Info.normalizedTime > 0.97 && Info.IsName("StandUpAttack"))
            {
                //애니메이션 종료시 공격으로 이어짐
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }

        public override void StateExit()
        {
            //레이러 다시 Player로 교체
            Owner.gameObject.layer = LayerMask.NameToLayer("Player");

            //애니메이션 해제
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("StandUpAttack", false, Pv_ID);
                GameManager.Instance.IKEvent(PLAYERSTATE.STANDUPATTACK, false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("StandUpAttack", false);
                OwnerIK.IKEvent(PLAYERSTATE.STANDUPATTACK, false);
            }
        }
    }
}
