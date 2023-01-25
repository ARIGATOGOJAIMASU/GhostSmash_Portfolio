using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class IdleState : State_Base
    {
        public override void StateEnter()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.PlayClip("Idle", Pv_ID);
                GameManager.Instance.AnimationFloat("Speed", 0, Pv_ID);
                GameManager.Instance.IKEvent(PLAYERSTATE.IDLE, true, Pv_ID);
            }
            else
            {
                Owner_animator.SetFloat("Speed", 0);
                Owner_animator.Play("Idle", 0);
                OwnerIK.IKEvent(PLAYERSTATE.IDLE, true);
            }

        }

        public override void Setting()
        {
            StateType = PLAYERSTATE.IDLE;
        }

        public override void Update()
        {
            //키값에 따라 스테이트 변환

            if (Input.GetKeyDown(KeyCode.Z) && !Owner_Script.playerInformation.Fear)
            {
                if (!Owner_Script.GetBoom)
                {
                    state_Machine.ChangeState(PLAYERSTATE.BASEATTACK);
                }
                else
                {
                    state_Machine.ChangeState(PLAYERSTATE.THROW);
                }
            }
            else if (Input.GetKeyDown(KeyCode.X) && !Owner_Script.playerInformation.Fear)
            {
                if (!Owner_Script.GetBoom && Owner_Script.playerInformation.SkillOn)
                {
                    state_Machine.ChangeState(PLAYERSTATE.SKILL);
                }
                else if(Owner_Script.GetBoom)
                {
                    state_Machine.ChangeState(PLAYERSTATE.THROW);
                }
            }
            else if (Input.GetKeyDown(KeyCode.C) && !Owner_Script.playerInformation.Fear)
            {
                if (!Owner_Script.GetBoom && Owner_Script.playerInformation.UltimateOn)
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
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!Owner_Script.playerInformation.Fear)
                {
                    state_Machine.ChangeState(PLAYERSTATE.JUMP);
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
            {
                state_Machine.ChangeState(PLAYERSTATE.RUN);
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.IKEvent(PLAYERSTATE.IDLE, false, Pv_ID);
            }
            else
            {
                OwnerIK.IKEvent(PLAYERSTATE.IDLE, false);
            }
        }
    }
}
