using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class DodgeAirState : State_Base
    {
        float Magnitude = 19;
        public override void Setting()
        {
            StateType = PLAYERSTATE.DODGEAIR;
        }

        public override void StateEnter()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationTrigger("Air", Pv_ID);
            }
            else
            {
                Owner_animator.SetTrigger("Air");
            }
        }

        public override void Update()
        {
            MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            if (Owner_Look != MoveDirection)
            {
                Owner.LookAt(Owner.position + MoveDirection);
                Owner_Look = MoveDirection;
            }

            Owner_rigidbody.velocity -= new Vector3(0, Time.deltaTime * Magnitude, 0);

            Owner_rigidbody.velocity = new Vector3(
                   Owner_Script.playerInformation.CurJumpDistance * MoveDirection.x,
                   Owner_rigidbody.velocity.y,
                   Owner_Script.playerInformation.CurJumpDistance * MoveDirection.z);
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationResetTrigger("Air", Pv_ID);
            }
            else
            {
                Owner_animator.ResetTrigger("Air");
            }
        }
    }
}
