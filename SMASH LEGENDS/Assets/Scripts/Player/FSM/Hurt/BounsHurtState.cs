using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class BounsHurtState : State_Base
    {
        Vector3 StartPos;
        Vector3 reflectVec;
        CapsuleCollider playerCollider;

        public override void Setting()
        {
            StateType = PLAYERSTATE.BOUNSHURT;
        }

        public override void StateEnter()
        {
            if(PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("BounsHurt", true, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("BounsHurt",true);
            }
        }

        public override void Update()
        {
            //ÇÃ·¹ÀÌ¾î°¡ ¶¥¿¡ ´êÀ» ¶§
            if(GroundPos.position.y > Owner.position.y)
            {
                Owner_rigidbody.velocity = Vector3.zero;
                Owner_Script.BounsDir.y = 1;
                Owner_rigidbody.velocity = Owner_Script.BounsDir * Owner_Script.BounsStrong;
                state_Machine.ChangeState(PLAYERSTATE.AIRBORNE);
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("BounsHurt", false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("BounsHurt", false);
            }
        }
    }
}
