using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{

    public class StandUpState : State_Base
    {
        public override void Setting()
        {
            StateType = PLAYERSTATE.STANDUP;
        }

        public override void StateEnter()
        {
            //레이어 무적으로 변경
            Owner.gameObject.layer = LayerMask.NameToLayer("Imotal");

            if(PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("StandUp",true,Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("StandUp", true);
            }
        }

        public override void Update()
        {
            AnimatorStateInfo Info = Owner_animator.GetCurrentAnimatorStateInfo(0);

            if (Info.normalizedTime > 0.97 && Info.IsName("StandUp"))
            {
                Debug.Log("이거는 좀 들어가자");
                //애니메이션 종료시 공격으로 이어짐
                state_Machine.ChangeState(PLAYERSTATE.STANDUPATTACK);
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("StandUp", false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("StandUp", false);
            }
        }
    }
}
