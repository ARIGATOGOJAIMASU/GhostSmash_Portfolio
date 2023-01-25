using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    class SmashState : State_Base
    {
        private float CurTime;
        private float HiglightTime = 0.3f;
        public override void Setting() { StateType = PLAYERSTATE.SMASH; }
        public override void StateEnter()
        {
            //Player 피격당하지 않게 변경
            Owner.gameObject.layer = LayerMask.NameToLayer("Imotal");
            Owner_rigidbody.isKinematic = true;
            Owner_Script.IsUpdate = false;

            //카메라 줌인
            Owner_Script.CameraChange();

            //에니메이션 잠시 정지
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationPlay(Pv_ID, true);
            }
            else
            {
                Owner_animator.StartPlayback();
            }

            CoroutineHelper.StartCoroutine(SetInitialization());

            //스메쉬 시간동안 업데이트를 중지함
            Debug.Log("스메쉬 들어감");
            state_Machine.ChangeState(state_Machine._PrevState);
        }

        public override void Update()
        {
         /*   CurTime += Time.deltaTime;

            //지정시간이 지나면 DeadFlyState로 전환
            if (HiglightTime < CurTime)
            {
                //이전 State로 돌아가야함
                state_Machine.ChangeState(state_Machine._PrevState);
            }*/
        }

        public override void StateExit()
        {
            /*//타이머 초기화
            CurTime = 0.0f;

            //애니메이션 재가동
            GameManager.Instance.AnimationStart(Pv_ID);

            //물리작용 다시 활성화 시키기
            Owner_rigidbody.isKinematic = false;

            //카메라 다시 되돌리기
            //Owner_Script.CameraChange();

            //Layer다시 되돌리기
            Owner.gameObject.layer = LayerMask.NameToLayer("Player");*/
        }

        IEnumerator SetInitialization()
        {
            yield return new WaitForSeconds(HiglightTime);

            Debug.Log(state_Machine._CurState);
            Debug.Log("스메쉬 끝남");
            //애니메이션 재생
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationPlay(Pv_ID, true);
            }
            else
            {
                Owner_animator.StopPlayback();
            }

            //물리작용 다시 활성화 시키기
            Owner_rigidbody.isKinematic = false;

            //카메라 다시 되돌리기
            Owner_Script.CameraChange();

            //Layer다시 되돌리기
            Owner.gameObject.layer = LayerMask.NameToLayer("Player");

            //스메쉬 시간동안 업데이트를 중지함
            Owner_Script.IsUpdate = true;
        }
    }
}
