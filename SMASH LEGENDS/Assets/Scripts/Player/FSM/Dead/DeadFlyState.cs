using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    class DeadFlyState : State_Base
    {
        //DeadLine
        private float Daed_Y = 15.0f;
        private float FlySpeed = 50.0f;
        private Vector3 FlyDirection;

        public override void Setting() { StateType = PLAYERSTATE.DEADFLY; }

        public override void StateEnter()
        {
            //맞은 방향쪽으로 날아가게 설정
            Owner_rigidbody.velocity = Vector3.zero;
            Owner_Script.LookCheck.SetActive(false);
            FlyDirection = Owner_Script.DeadFlyDirection;
            FlyDirection.y = 0.5f;
        }

        public override void Update()
        {
            //먼지 Prefab를 생성하면서 날라감
            if (PhotonNetwork.IsConnected)
            {
                EffectManager.Instance.EffectInst(EFFECT.DUST, Owner.transform.position);
            }

            Owner.Translate(FlyDirection * FlySpeed * Time.deltaTime, Space.World);

            //데드라인 도달 시 DeadState로 전환
            if (Owner.position.y > Daed_Y)
            {
                state_Machine.ChangeState(PLAYERSTATE.DEAD);
            }
        }

        public override void StateExit()
        {
            //미리 Hurt Bool값 false로 변경
            Owner_rigidbody.velocity = Vector3.zero;

            //이펙트 소환
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Hurt", false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("Hurt", false);
            }
        }
    }
}
