using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class Skill_Gangnim_JumpSkill_FollowUp : Wooseok.Skill_Slash
    {
        public bool colOn = true;
        public Skill_Gangnim_JumpSkill_FollowUp(GameObject ParentPlayer, Wooseok.Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }

        protected override void FixedUpdate()
        {
            if (timer < LifeTime)
            {
                timer += Time.fixedDeltaTime;

                if(timer > 0.3f && colOn)
                {
                    colOn = false;
                    SetCollider(false);
                }
            }
            else if(myAudio)
            {   if(!myAudio.PlayCheck())
                {
                    this.gameObject.SetActive(false);
                }
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }

        public override void Restart()
        {
            base.Restart();
            colOn = true;
        }
    }
}
