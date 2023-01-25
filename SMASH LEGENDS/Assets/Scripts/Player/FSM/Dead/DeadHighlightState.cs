using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    class DeadHighlightState : State_Base
    {
        private float CurTime;
        private float HiglightTime = 0.5f;

        public override void Setting() { StateType = PLAYERSTATE.DEADHIGHLIGHT; }

        public override void StateEnter()
        {
            //Player 피격당하지 않게 변경
            Owner.gameObject.layer = LayerMask.NameToLayer("Dead");
            Owner_rigidbody.isKinematic = true;

            //Dead카메라로 변경
            Owner_Script.CameraChange();

            //현제 애니메이션을 정지시킴

            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Hurt", false, Pv_ID);
                GameManager.Instance.AnimationPlay(Pv_ID, true);
            }
            else
            {
                Owner_animator.SetBool("Hurt", true);
                Owner_animator.StartPlayback();
            }
        }

        public override void Update()
        {
            CurTime += Time.deltaTime;

            //지정시간이 지나면 DeadFlyState로 전환
            if (HiglightTime < CurTime)
            {
                //날라가는 연출로 바뀜
                state_Machine.ChangeState(PLAYERSTATE.DEADFLY);
            }
        }

        public override void StateExit()
        {
            CurTime = 0.0f;

            //카메라 MainCamera로 교체
            Owner_Script.CameraChange();

            //날라가는 단계에서 GroundCheak와 UI끄기
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.Dead(false, Pv_ID);
            }
            else
            {
                Owner_Script.Dead(false);
            }
        }
    }
}
