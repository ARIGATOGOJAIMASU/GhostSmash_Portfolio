using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class UseJumpStationState : State_Base
    {
        float Timer;
        float Magnitude = 19;

        public override void Setting()
        {
            StateType = PLAYERSTATE.USE_JUMPSTATION;
        }

        public override void StateEnter()
        {
            Owner_rigidbody.velocity = (Owner_Script.UseJumpDirection * 30) + new Vector3(0, 10f, 0);

            if(PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationTrigger("Air",Pv_ID);
            }
            else
            {
                Owner_animator.SetTrigger("Air");
            }
        }

        public override void Update()
        {
            if (Timer > 0.5f)
            {
                MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

                if (MoveDirection.magnitude != 0 &&
                    MoveDirection != Owner_Look)
                {
                    Owner_Look = MoveDirection;
                    Owner.LookAt(Owner.position + MoveDirection);
                }

                if (MoveDirection.magnitude != 0)
                {
                    Owner_rigidbody.velocity = new Vector3(
                           Owner_Script.playerInformation.CurJumpDistance * MoveDirection.x,
                           Owner_rigidbody.velocity.y,
                           Owner_Script.playerInformation.CurJumpDistance * MoveDirection.z);
                }

                if (Owner_rigidbody.velocity.y <= 0.2f)
                {
                    Owner_rigidbody.velocity -= new Vector3(0, Time.deltaTime * Magnitude, 0);
                }
            }
            else
            {
                Timer += Time.deltaTime;
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
            else if (Input.GetKeyDown(KeyCode.X) &&
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
            else if (Input.GetKeyDown(KeyCode.C) &&
                Owner_Script.playerInformation.UltimateOn)
            {
                if (!Owner_Script.GetBoom)
                {
                    state_Machine.ChangeState(PLAYERSTATE.ULTIMATEPREPARE);
                }
            }
        }

        public override void StateExit()
        {
            Timer = 0.0f;
        }
    }
}
