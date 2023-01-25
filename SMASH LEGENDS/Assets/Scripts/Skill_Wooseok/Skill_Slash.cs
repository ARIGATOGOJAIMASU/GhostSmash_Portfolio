using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Junpyo;

namespace Wooseok
{

    public class Skill_Slash : Skill
    {
        public Skill_Slash(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer,FollowUp)
        {
        }
        public override void FollowUp()
        { 
            if(FollowUpSkill)
            {
                FollowUpSkill.gameObject.SetActive(true);
                FollowUpSkill.Restart();
            }
        }

        public override void Restart()
        {
            timer = 0f;
            curhit = 0;
            slappedtarget.Clear();
            SetCollider(true);

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

            if(FollowUpSkill != null)
            {
                FollowUpSkill.gameObject.SetActive(false);
            }
            PlaySkillSound();
        }

        public override void SkillEffectOnEnter(GameObject otherobj)
        {
            if (
                (!ParentPlayer.CompareTag(otherobj.tag))
                && ParentPlayer != otherobj
                && MaxHit > curhit
                && MaxTargetNumber > slappedtarget.Count
                && !GameObjectChecker(slappedtarget, otherobj)
                )
            {
                if(otherobj.layer == LayerMask.NameToLayer("Player"))
                {
                    Debug.Log("SkillSlashOnEnterOn");
                    HitEnemy(otherobj);
                }
                else if(otherobj.layer == LayerMask.NameToLayer("Coffin"))
                {
                    HitCoffin(otherobj);
                    
                }
                curhit++;
                slappedtarget.Add(new Pair<GameObject, int>(otherobj, 1));
            }
        }

        public override void SkillEffectOnExit(GameObject otherobj)
        {
            //throw new System.NotImplementedException();
        }

        public override void SkillEffectOnStay(GameObject otherobj)
        {
            //throw new System.NotImplementedException();
        }
    }

}

