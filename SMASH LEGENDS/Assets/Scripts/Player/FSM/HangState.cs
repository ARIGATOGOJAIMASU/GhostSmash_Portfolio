using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Junpyo
{
    public class HangState : State_Base
    {
        //3초로 고정되어 있음
        private float HangMaxTime = 3.0f;
        private float HangCurTime;
        public Image StaminaUI;
        private Transform LookCheck;
        private GroundCheack GroundCheackScript;

        public override void Setting()
        {
            StateType = PLAYERSTATE.HANG;
            StaminaUI = Owner_Script.StaminaUI;
            LookCheck = Owner_Script.LookCheck.transform;
            GroundCheackScript = GroundPos.GetComponent<GroundCheack>();
        }

        public override void StateEnter()
        {
            Owner_Canvas.GetComponent<Billboard>().Hang = true;

            //스태미나UI 활성화
            StaminaUI.enabled = true;

            //velocity값 초기하 및 중력 해제
            Owner_rigidbody.velocity = Vector3.zero;
            Owner_rigidbody.useGravity = false;

            //Owner_animator.SetBool("Hang", true);
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Hang", true, Pv_ID);
                GameManager.Instance.IKEvent(StateType, true, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("Hang", true);
                OwnerIK.IKEvent(StateType, true);
            }

            //LookCheck위치 변경
            LookCheck.localPosition -= new Vector3(0, Owner_Script.playerInformation.Hang_Y, 0);

            Owner.gameObject.layer = LayerMask.NameToLayer("HangAttack");

            GroundCheackScript.Hang = true;
            GroundCheackScript.LineSet();
        }

        public override void Update()
        {
            //Hang최대시간을 넘어가면 Hang상태를 풀고
            if (HangCurTime >= HangMaxTime)
            {
                //Fall상태로 돌입
                state_Machine.ChangeState(PLAYERSTATE.DROP);
                return;
            }

            //GroundCheck 위치 조절
            GroundPos.position = Owner.position + new Vector3(0, -Owner_Script.playerInformation.Hang_Y + 0.2f, 0);

            //매다리는 시간 측정
            HangCurTime += Time.deltaTime;

            //시간에 따른 스테미나 양 표시
            StaminaUI.fillAmount = 1 - (HangCurTime / HangMaxTime);

            //점프시 땅위치로 플레이어 위치값 초기화
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Owner.position = GroundPos.position + new Vector3(0, 0.4f, 0) + Owner.forward * 0.2f;
                state_Machine.ChangeState(PLAYERSTATE.JUMP);
                Owner.gameObject.layer = LayerMask.NameToLayer("Player");
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                //HangAttack실행
                state_Machine.ChangeState(PLAYERSTATE.HANGATTACKPREPARE);
            }
        }

        public override void StateExit()
        {
            //체크타임 초기화
            HangCurTime = 0;

            //중력 활성화
            Owner_rigidbody.useGravity = true;

            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Hang", false, Pv_ID);
                GameManager.Instance.IKEvent(StateType, false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("Hang", false);
                OwnerIK.IKEvent(StateType, false);
            }

            //스테미나 표시 및 캔버스 위치 다시 플레이로 위치로 고정
            StaminaUI.enabled = false;

            Owner_Canvas.GetComponent<Billboard>().Hang = false;
            //LookCheck위치 변경
            LookCheck.localPosition += new Vector3(0, Owner_Script.playerInformation.Hang_Y, 0);

            GroundCheackScript.Hang = false;
            GroundCheackScript.LineSet();
        }
    }
}
