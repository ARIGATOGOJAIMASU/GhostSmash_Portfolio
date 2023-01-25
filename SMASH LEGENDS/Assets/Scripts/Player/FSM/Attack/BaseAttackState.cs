using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class BaseAttackState : State_Base
    {
        private bool IsStartAttack = false;
        private float StartAttackDelay = 0.1f;
        private float CurDelay = 0.0f;

        public override void Setting()
        {
            StateType = PLAYERSTATE.BASEATTACK;
        }

        public override void StateEnter()
        {
            //전 State가 Smash일 경우 초기화 생략
            if (state_Machine._PrevState != PLAYERSTATE.SMASH)
            {
                //첫 공격 시작
                Owner_rigidbody.velocity = Vector3.zero;

                if (PhotonNetwork.IsConnected)
                {
                    GameManager.Instance.AnimationFloat("Speed", 0, Pv_ID);
                    GameManager.Instance.AnimationBool("IsAttack", true, Pv_ID);
                    GameManager.Instance.AnimationTrigger("StartAttack", Pv_ID);
                    GameManager.Instance.IKEvent(PLAYERSTATE.BASEATTACK, true, Pv_ID);
                }
                else
                {
                    Owner_animator.SetFloat("Speed", 0);
                    Owner_animator.SetBool("IsAttack", true);
                    Owner_animator.SetTrigger("StartAttack");
                    Owner_Script.IK_Controller.IKEvent(PLAYERSTATE.BASEATTACK, true);
                }


                IsStartAttack = true;
            }
        }

        public override void Update()
        {
            AnimatorStateInfo StateInfo = Owner_animator.GetCurrentAnimatorStateInfo(0);

            if (StateInfo.IsName("AttackCombo1") || StateInfo.IsName("AttackCombo2"))
            {
                InputZ();
            }
            else if (!StateInfo.IsName("AttackCombo3") && !IsStartAttack)
            {
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }

            if (IsStartAttack)
            {
                if (StartAttackDelay < CurDelay)
                {
                    IsStartAttack = false;
                    CurDelay = 0.0f;
                }

                CurDelay += Time.deltaTime;
            }
        }

        public override void StateExit()
        {
            //현 State가 Smash일 경우 현재 값을 저장함
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("IsAttack", false, Pv_ID);
                GameManager.Instance.AnimationResetTrigger("ComboAttack", Pv_ID);
                GameManager.Instance.IKEvent(PLAYERSTATE.BASEATTACK, false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("IsAttack", false);
                Owner_animator.ResetTrigger("ComboAttack");
                Owner_Script.IK_Controller.IKEvent(PLAYERSTATE.BASEATTACK, false);
            }

            IsStartAttack = false;
        }

        //Key Input받기
        public void InputZ()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

                if (PhotonNetwork.IsConnected)
                {
                    GameManager.Instance.AnimationTrigger("ComboAttack", Pv_ID);
                }
                else
                {
                    Owner_animator.SetTrigger("ComboAttack");
                }

                Owner_Look = MoveDirection;
                Owner.GetComponent<PlayerController_FSM>().PlayerLook = Owner_Look;
            }
        }
    }
}
