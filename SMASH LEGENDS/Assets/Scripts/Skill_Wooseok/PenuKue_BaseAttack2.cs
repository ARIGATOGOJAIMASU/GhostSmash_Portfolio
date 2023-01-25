using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class PenuKue_BaseAttack2 : Wooseok.Skill_Slash
    {
        [SerializeField] GameObject Effect; 
        public PenuKue_BaseAttack2(GameObject ParentPlayer, Wooseok.Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }

        public override void Restart()
        {
            timer = 0f;
            curhit = 0;
            slappedtarget.Clear();

            if (!IshaveParent)
            {
                this.transform.position = ParentPlayer.transform.position + new Vector3(startvector.x * ParentPlayer.transform.forward.x,
                                                                                            startvector.y * ParentPlayer.transform.up.y,
                                                                                            startvector.x * ParentPlayer.transform.forward.z);
            }
            else
            {
                this.transform.position = ParentPlayer.transform.position + new Vector3(startvector.x * transform.forward.x,
                                                                                            startvector.y * transform.up.y,
                                                                                            startvector.x * transform.forward.z);
                this.transform.rotation = ParentPlayer.transform.rotation;
            }
            SetCollider(true);
            PlaySkillSound();
        }
    }
}
