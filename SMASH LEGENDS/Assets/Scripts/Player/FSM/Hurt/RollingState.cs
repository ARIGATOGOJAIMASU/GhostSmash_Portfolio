using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    class RollingState : State_Base
    {
        public override void Setting() { StateType = PLAYERSTATE.ROLLING; }

        public override void StateEnter()
        {
            //마지막에 입력한 키값으로 구르기
            MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            Owner.LookAt(Owner.position + MoveDirection);

            //구르는 동안 무적상태를 위해 Layer교체
            Owner.gameObject.layer = LayerMask.NameToLayer("Imotal");

            //애니메이션이 중첩되는 버그가 있어서 animator Play기능을 사용
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationTrigger("Rolling", Pv_ID);
                GameManager.Instance.PlayClip("Rolling", Pv_ID);
            }
            else
            {
                Owner_animator.SetTrigger("Rolling");
                Owner_animator.Play("Rolling", 0);
            }

            //이동하는 거리
            //Owner_rigidbody.velocity = Owner.forward * 12.0f;
        }

        public override void Update()
        {
            //구르기가 끝나는 타이밍에 키입력 가능
            if (Owner_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
            {
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationResetTrigger("Rolling", Pv_ID);
            }
            else
            {
                Owner_animator.ResetTrigger("Rolling");
            }

            //애니메이션이 끝나면서 무적상태 종료
            Owner.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
}
