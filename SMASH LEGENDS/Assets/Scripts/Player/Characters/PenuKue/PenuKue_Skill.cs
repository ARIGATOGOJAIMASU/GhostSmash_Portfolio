using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class PenuKue_Skill : PlayerSkill
    {
        public float test;

        public override void BaseAttackCombo1()
        {
            playerRig.velocity = playertransform.forward * 7f;
        }

        public override void BaseAttackCombo2()
        {
            base.BaseAttackCombo2();
        }

        public void Kinematic_ONOff()
        {
            playerRig.isKinematic = !playerRig.isKinematic;
        }

        public void SkillDalay()
        {

        }

        IEnumerator SkillDelay()
        {
            yield return new WaitForSeconds(test);


        }
    }
}
