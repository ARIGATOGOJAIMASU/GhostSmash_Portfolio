using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class Gangnim_conttroller : PlayerController_FSM
    {
        [SerializeField] float y;
        private void Start()
        {
            if (IsMine)
            {
                state_Machine.StateAdd(new JumpSkillPrepare(), PLAYERSTATE.JUMPSKILLPREPARE);
                state_Machine.StateAdd(new Gangnim_JumpSkillState(), PLAYERSTATE.GANGNIM_JUMPSKILLSTATE);
            }
        }

        public void HangAttackPrepare()
        {
            transform.position += (transform.forward * 0.2f) + new Vector3(0, y, 0);
        }
    }
}
