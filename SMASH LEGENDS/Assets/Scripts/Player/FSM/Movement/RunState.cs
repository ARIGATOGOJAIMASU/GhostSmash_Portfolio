using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class RunState : State_Base
    {
        //대각선 이동변수
        [SerializeField] private float SlopeTime = 0.05f;
        [SerializeField] private float SlopeCheckTimer = 0.0f;
        private bool isSlope = false;
        private Vector3 SlopeDirecion;

        public override void Setting() { StateType = PLAYERSTATE.RUN; }

        public override void StateEnter()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationFloat("Speed", 5, Pv_ID);
            }
            else
            {
                Owner_animator.SetFloat("Speed", 5);
            }

            MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            Owner.LookAt(Owner.position + MoveDirection);
        }

        public override void Update()
        {
            //속도가 0일경우 Idle상태로 돌아감
            MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            //대각선 방향일때
            if (MoveDirection.x != 0 && MoveDirection.z != 0)
            {
                SlopeDirecion = MoveDirection;
                isSlope = true;
            }
            //대각선 방향이 아닐때
            else if (Mathf.Abs(MoveDirection.z + MoveDirection.x) == 1)
            {
                if (isSlope)
                {
                    //일정시간이 자나기전까지는 대각선방향을 유지한다.
                    SlopeCheckTimer += Time.deltaTime;
                    if (SlopeTime <= SlopeCheckTimer)
                    {
                        isSlope = false;
                        SlopeCheckTimer = 0.0f;
                    }
                }
                else
                    SlopeCheckTimer = 0.0f;
            }
            else
            {
                //키값이 대각선방향이 아닐시 SlopeDirecion초기화
                SlopeDirecion = Vector3.zero;
            }

            //전 이동방향이랑 다를때 시점변화 단 Idle상태일때는 마지막 시점에서 Idle상태 유지
            if (isSlope)
            {
                MoveDirection = SlopeDirecion;
            }

            //Player의 시점 변경
            if (Owner_Look != MoveDirection)
            {
                Owner_Look = MoveDirection;
                Owner.LookAt(Owner.position + Owner_Look);
            }

            //이동시키기기
            Owner_rigidbody.velocity = new Vector3(MoveDirection.x * Owner_Script.playerInformation.CurRunSpeed,
                 Owner_rigidbody.velocity.y,
                MoveDirection.z * Owner_Script.playerInformation.CurRunSpeed);

            //키값에 따라 스테이트 변환

            //공포상태일 시 행동불능
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (!Owner_Script.playerInformation.Fear)
                {
                    if (!Owner_Script.GetBoom)
                    {
                        Owner_rigidbody.velocity = Vector3.zero;
                        state_Machine.ChangeState(PLAYERSTATE.BASEATTACK);
                    }
                    else
                    {
                        state_Machine.ChangeState(PLAYERSTATE.THROW);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.X) && !Owner_Script.playerInformation.Fear)
            {
                if (!Owner_Script.GetBoom &&
                    Owner_Script.playerInformation.SkillOn)
                {
                    state_Machine.ChangeState(PLAYERSTATE.SKILL);
                }
                else if (Owner_Script.GetBoom)
                {
                    state_Machine.ChangeState(PLAYERSTATE.THROW);
                }

            }
            else if (Input.GetKeyDown(KeyCode.C) && !Owner_Script.playerInformation.Fear)
            {
                if (!Owner_Script.GetBoom &&
                    Owner_Script.playerInformation.UltimateOn)
                {
                    if (Owner_Script.playerInformation.UtimatePrepare)
                    {
                        state_Machine.ChangeState(PLAYERSTATE.ULTIMATEPREPARE);
                    }
                    else
                    {
                        state_Machine.ChangeState(PLAYERSTATE.ULTIMATE);
                    }
                }
                else if(Owner_Script.GetBoom)
                {
                    state_Machine.ChangeState(PLAYERSTATE.THROW);
                }
            }
            //속도가 0일경우 Idle상태로 돌아감
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!Owner_Script.playerInformation.Fear)
                {
                    state_Machine.ChangeState(PLAYERSTATE.JUMP);
                }
            }
            else if (MoveDirection.magnitude == 0)
            {
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }

        public override void FixedUpdate()
        {
            //Player위치
            //Owner_rigidbody.velocity = MoveDirection * Owner_Script.playerInformation.CurRunSpeed;
            //Owner_rigidbody.velocity = Owner.forward * Owner_Script.playerInformation.CurRunSpeed;
            //Owner_rigidbody.MovePosition(Owner.position + Owner.forward * Owner_Script.playerInformation.CurRunSpeed * Time.deltaTime);
            // Owner_rigidbody.velocity = new Vector3(MoveDirection.x * Owner_Script.playerInformation.CurRunSpeed,0, MoveDirection.z * Owner_Script.playerInformation.CurRunSpeed);
        }


        public override void StateExit()
        {
            //스피드 관련 값들 
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationFloat("Speed", 0, Pv_ID);
            }
            else
            {
                Owner_animator.SetFloat("Speed", 0);
            }

            //Owner_rigidbody.velocity = Vector3.zero;
        }


    }
}
