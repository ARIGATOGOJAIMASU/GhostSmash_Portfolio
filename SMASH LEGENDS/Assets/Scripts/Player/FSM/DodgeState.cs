using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Junpyo
{
    public class DodgeState : State_Base
    {
        float DodgeSpeed = 15.0f;
        float Timer;
        float Magnitude = 19;

        public override void Setting() { StateType = PLAYERSTATE.DODGE; }

        public override void StateEnter()
        {
            //Rigidbody초기화 및 중력 해제
            Owner_rigidbody.velocity = Vector3.zero;
            //Owner_rigidbody.useGravity = false;
            //무적상태 돌입
            Owner.gameObject.layer = LayerMask.NameToLayer("Imotal");
            //키입력받기
            Owner.LookAt(Owner.position + MoveDirection);



            //애니메이션 재생
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationTrigger("Dodge",Pv_ID);
            }
            else
            {
                Owner_animator.SetTrigger("Dodge");
            }

            Owner_rigidbody.velocity = (DodgeSpeed * -Owner.forward) + new Vector3(0, 5f, 0);
        }

        public override void Update()
        {
            //무적상태로 이동
            if (0.05 <= Timer)
            {
                Owner_rigidbody.velocity -= new Vector3(0, Time.deltaTime  * Magnitude, 0);
                //Owner_rigidbody.velocity -= new Vector3(0, Time.deltaTime * -Magnitude, 0);
            }

            AnimatorStateInfo info = Owner_animator.GetCurrentAnimatorStateInfo(0);

            //Dodge시간 체크
            Timer += Time.deltaTime;

            //회피기 시간이 지나면 자동으로 돌아옴
            if ((info.normalizedTime > 0.97f) && info.IsName("Dodge"))
            {
                //공중인지 아닌지를 판단 후 StateChange
                if (Mathf.Abs(GroundPos.position.y - Owner.position.y) >= 0.1f)
                {
                    state_Machine.ChangeState(PLAYERSTATE.DODGEAIR);
                }
                else
                {
                    state_Machine.ChangeState(PLAYERSTATE.IDLE);
                }
            }


        }

        public override void StateExit()
        {
            Timer = 0.0f;

            //Layer재설정
            Owner.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
}
