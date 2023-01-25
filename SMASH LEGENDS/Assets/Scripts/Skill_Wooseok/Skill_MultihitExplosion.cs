using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Wooseok
{
    public class Skill_MultihitExplosion : Skill_Explosion
    {

        [SerializeField]
        List<ATTACKTYPE> AttackTypes;
        [SerializeField]
        List<float> Damages;

        public Skill_MultihitExplosion(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }

        public override void SkillEffectOnEnter(GameObject otherobj)
        {
            if (
                otherobj.tag != ParentPlayer.tag
                && ParentPlayer != otherobj
                && MaxHit > curhit
                && MaxTargetNumber > slappedtarget.Count
                && !GameObjectChecker(slappedtarget, otherobj)
                )
            {
                if (otherobj.layer == LayerMask.NameToLayer("Player"))
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

        protected override void HitEnemy(GameObject otherobj)
        {
            if (IsImmortal(otherobj))
            {
                return;
            }

            Vector3 direction;
            direction = DrawDirection(otherobj);

            GameManager.Instance.Hurt(direction, debuffDuration, AttackTypes[0], Damages[0],HitSoundNumber ,ParentScript.ID, otherobj.GetComponent<PhotonView>().ViewID);

            if (Damage != 0)
            {
                GameManager.Instance.CameraShaking(ParentScript.ID);
                GameManager.Instance.GagePus(Ultimatecharge, ParentScript.ID);
                //PlayHitSound();
            }
        }
    }
}