using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Junpyo;


namespace Wooseok
{
    public class Skill_BanishingSlash : Skill_Slash
    {
        [SerializeField]
        GameObject Effect;

        public Skill_BanishingSlash(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }

        public override void SkillEffectOnEnter(GameObject otherobj)
        {
            if (
                (ParentPlayer.tag != otherobj.tag)
                && ParentPlayer != otherobj
                && MaxHit > curhit
                && MaxTargetNumber > slappedtarget.Count
                && !GameObjectChecker(slappedtarget, otherobj)
                )
            {
                slappedtarget.Add(new Pair<GameObject, int>(otherobj, 1));

                if (CheckObj(otherobj))
                {
                    HitEnemy(otherobj);
                    
                }
                else if (otherobj.layer == LayerMask.NameToLayer("Coffin"))
                {
                    HitCoffin(otherobj);
                }

                curhit++;
                slappedtarget.Add(new Pair<GameObject, int>(otherobj, 1));
            }
        }

        protected void Banishing()
        {
            Debug.Log("Banishing");
            SetCollider(false);
            Effect.SetActive(false);
        }

        public override void Restart()
        {
            base.Restart();

            SetCollider(true);
            Effect.SetActive(true);
            PlaySkillSound();
        }
    }

}

