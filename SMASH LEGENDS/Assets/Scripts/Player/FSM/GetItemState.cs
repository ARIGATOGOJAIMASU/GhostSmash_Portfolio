using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class GetItemState : State_Base
    {
        public override void Setting()
        {
            StateType = PLAYERSTATE.GETITEM;
        }

        public override void StateEnter()
        {
            Owner_rigidbody.velocity = Vector3.zero;

            if(PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationTrigger("GetItem", Pv_ID);
            }
            else
            {
                Owner_animator.SetTrigger("GetItem");
            }
        }

        public override void Update()
        {
            AnimatorStateInfo info = Owner_animator.GetCurrentAnimatorStateInfo(0);

            if((info.normalizedTime > 0.95) && (info.IsName("GetItem")))
            {
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }
    }
}
