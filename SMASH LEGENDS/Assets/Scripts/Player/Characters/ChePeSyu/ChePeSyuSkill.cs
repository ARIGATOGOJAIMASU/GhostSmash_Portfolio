using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class ChePeSyuSkill : PlayerSkill
    { 
        public override void BaseAttackCombo1()
        {
            playerRig.velocity = playertransform.forward * 5.5f;
        }

        public override void BaseAttackCombo2()
        {
            transform.LookAt(transform.position + playerController.PlayerLook);
            playerRig.velocity = playertransform.forward * 4f;
        }
        public override void BaseAttackCombo3_1()
        {
            transform.LookAt(transform.position + playerController.PlayerLook);
            playerRig.velocity = playertransform.forward * 5f;
        }

        public void SkillRebound()
        {
            playerRig.velocity = new Vector3(-transform.forward.x , 0, -transform.forward.z) * 8f;
        }

        protected  void usejumpskill()
        {
            myJumpSkill.SetActive(true);
            myJumpSkill.GetComponent<Wooseok.Skill>().Restart();

        }

        public void UltFollowUp()
        {
            myUltimate.GetComponent<Wooseok.Skill>().FollowUp();
        }

        public void UltLightOn()
        {
            myUltimate.GetComponent<Wooseok.Skill>().FollowUpSkill.GetComponent<Wooseok.Skill_GroundSlam>().LighteningBolt();
        }
    }
}
