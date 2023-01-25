using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class Gangnim_Skill : PlayerSkill
    {
        public override void BaseAttackCombo1()
        {
            playerRig.velocity = playertransform.forward * 4.0f;
        }

        public override void BaseAttackCombo2()
        {
            transform.LookAt(transform.position + playerController.PlayerLook);
            playerRig.velocity = playertransform.forward * 5.0f;
        }

        public override void BaseAttackCombo3_1()
        {
            transform.LookAt(transform.position + playerController.PlayerLook);
            playerRig.velocity = playertransform.forward * 3.0f;
        }

        public override void JumpSkillPrepareAction()
        {
            playerRig.velocity = Vector3.zero;
            playerRig.velocity = (playertransform.forward * 6.5f) + new Vector3(0, 6, 0);
        }
    }
}
