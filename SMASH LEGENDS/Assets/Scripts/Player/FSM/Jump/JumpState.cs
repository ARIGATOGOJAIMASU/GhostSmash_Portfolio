using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class JumpState : State_Base
    {
        private Vector3 JumpDirection;
        private float JumpChangeRisk = 0.5f;
        Coroutine coroutine;
        float Timer;
        float Magnitude = 19;

        public override void Setting() { StateType = PLAYERSTATE.JUMP; }

        public override void StateEnter()
        {
            //하늘방향으로 AddPos
            Owner_rigidbody.velocity = Vector3.zero;
            MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            Owner.LookAt(Owner.position + MoveDirection);
            Owner.position += new Vector3(0, 0.5f, 0);
            JumpDirection = MoveDirection;
            Owner_rigidbody.AddForce(new Vector3(0, Owner_Script.playerInformation.JumpPower, 0));

            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationTrigger("Jump", Pv_ID);
                GameManager.Instance.PlayClip("Jump", Pv_ID);
            }
            else
            {
                Owner_animator.SetTrigger("Jump");
                Owner_animator.Play("Jump");
            }

            coroutine = CoroutineHelper.StartCoroutine(TriggetStay());
        } 

        public override void Update()
        {
            MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            if (Owner_Look != MoveDirection)
            {
                Owner.LookAt(Owner.position + MoveDirection);
                Owner_Look = MoveDirection;
            }

            if (Owner_rigidbody.velocity.y <= 0.2f)
            {
                Owner_rigidbody.velocity -= new Vector3(0, Time.deltaTime * Magnitude, 0);
            }

            //점프중 방향을 바꿀시 Velocity값 감소
            Vector3 Temp = MoveDirection;

            //내적을 활용하여 값수치 변경
            if (Vector3.Dot(JumpDirection.normalized, Owner_Look.normalized) < 0)
            {
                //반대방향을 바라볼때
                Owner_rigidbody.velocity = new Vector3(JumpChangeRisk * Temp.x,
                    Owner_rigidbody.velocity.y,
                    JumpChangeRisk * Temp.z);
            }
            else
            {
                Owner_rigidbody.velocity = new Vector3(
                    Owner_Script.playerInformation.CurJumpDistance * Temp.x,
                    Owner_rigidbody.velocity.y,
                    Owner_Script.playerInformation.CurJumpDistance * Temp.z);
            }

            //StateChange
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (!Owner_Script.GetBoom)
                {
                    if (Owner_Script.CharacterName == CHARACTERNAME.DUSEONIN)
                    {
                        state_Machine.ChangeState(PLAYERSTATE.DUSEONIN_JUMPATTACK);
                    }
                    else if (Owner_Script.CharacterName == CHARACTERNAME.TRUELOVE)
                    {
                        state_Machine.ChangeState(PLAYERSTATE.TRUELOVE_JUMPATTACK);
                    }
                    else
                    {
                        state_Machine.ChangeState(PLAYERSTATE.JUMPATTACK);
                    }
                }
                else
                {
                    state_Machine.ChangeState(PLAYERSTATE.THROW);
                }
            }
            else if(Input.GetKeyDown(KeyCode.X) &&
                Owner_Script.playerInformation.SkillOn)
            {
                if (!Owner_Script.GetBoom)
                {
                    if (Owner_Script.playerInformation.JumpSkillPrepare)
                    {
                        state_Machine.ChangeState(PLAYERSTATE.JUMPSKILLPREPARE);
                    }
                    else
                    {
                        state_Machine.ChangeState(PLAYERSTATE.JUMPSKILL);
                    }
                }
                else
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
                else if (Owner_Script.GetBoom)
                {
                    state_Machine.ChangeState(PLAYERSTATE.THROW);
                }
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationResetTrigger("Jump", Pv_ID);
            }
            else
            {
                Owner_animator.ResetTrigger("Jump");
            }

            Timer = 0.0f;
        }

        IEnumerator TriggetStay()
        {
            yield return new WaitForSeconds(0.3f);

            Owner_Script.OnGround = true;
        }
    }
}
