using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class ChePeSyuUltimateState : State_Base
    {
        public override void Setting()
        {
            StateType = PLAYERSTATE.CHEPESYULTIMATE;
        }

        public override void StateEnter()
        {
            if(PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Ultimate", true, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("Ultimate", true);
            }
        }

        public override void Update()
        {
            //체페슈가 땅에 닿을 경우 애니메이션을 다시 재생함
            if(GroundPos.position.y > Owner.position.y)
            {
                if (PhotonNetwork.IsConnected)
                {
                    GameManager.Instance.AnimationPlay(Pv_ID, true);
                }
                else
                {
                    Owner_animator.StopPlayback();
                }
            }

            //애니메이션 시간이 끝나면 Idle상태로 전환
            if(Owner_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95)
            {
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }

        public override void StateExit()
        {
            base.StateExit(); if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Ultimate", false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("Ultimate", false);
            }

            Owner.gameObject.layer = LayerMask.NameToLayer("Player");

            //만약 맵밖으로 떨어져 죽을 경우
            if(state_Machine._CurState == PLAYERSTATE.DEAD)
            {
                GameManager.Instance.Attack((int)SKILLTYPE.ULTIMATE, false, Pv_ID);
            }
        }
    }
}
