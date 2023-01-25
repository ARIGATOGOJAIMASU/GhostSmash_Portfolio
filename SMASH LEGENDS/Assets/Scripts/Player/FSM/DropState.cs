using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class DropState : State_Base
    {
        public override void Setting()
        {
            StateType = PLAYERSTATE.DROP;
        }

        public override void StateEnter()
        {
            Owner.gameObject.layer = LayerMask.NameToLayer("Imotal");

            if(PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Drop", true, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("Drop", true);
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Drop", false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("Drop", false);
            }
        }
    }
}
