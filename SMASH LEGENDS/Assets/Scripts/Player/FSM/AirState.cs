using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class AirState : State_Base
    {
        bool Action = false;
        float Magnitude = 20;

        public override void Setting() { StateType = PLAYERSTATE.AIR; }
        public override void StateEnter()
        {
            //Owner_rigidbody.velocity += new Vector3(0, -1.0f, 0);

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

            if (MoveDirection.magnitude != 0 && MoveDirection != Owner_Look)
            {
                Owner_Look = MoveDirection;
                Owner.LookAt(Owner.position + MoveDirection);
            }

            if (Owner_rigidbody.velocity.y <= 0.2f)
            {
                Owner_rigidbody.velocity -= new Vector3(0, Time.deltaTime * Magnitude, 0);
            }

            if (MoveDirection.magnitude != 0)
            {
                Owner_rigidbody.velocity = new Vector3(
                       Owner_Script.playerInformation.CurJumpDistance * MoveDirection.x,
                       Owner_rigidbody.velocity.y,
                       Owner_Script.playerInformation.CurJumpDistance * MoveDirection.z);
            }
        }

        public override void StateExit()
        {
        }
    }
}