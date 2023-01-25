using System.Collections;

using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class ChePeSyuContlloer : PlayerController_FSM
    {
        private void Start()
        {
            if (photonView.IsMine)
            {
                state_Machine.StateAdd(new UltimatePrepareState(), PLAYERSTATE.ULTIMATEPREPARE);
                state_Machine.StateAdd(new ChePeSyuUltimateState(), PLAYERSTATE.CHEPESYULTIMATE);
            }
        }

        public void Gravity()
        {
            playerRigidbody.useGravity = !playerRigidbody.useGravity;
        }

        public void JumpSkillRebound()
        {
            Gravity();
            playerRigidbody.velocity = Vector3.zero;

            //반동주기
            playerRigidbody.velocity = transform.up * 6.0f;
            playerRigidbody.velocity += -transform.forward * 8.0f;
        }
    }
}