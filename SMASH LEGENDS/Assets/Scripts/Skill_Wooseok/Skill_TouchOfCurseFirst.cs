using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Junpyo;


namespace Wooseok
{

    public class Skill_TouchOfCurseFirst : Skill
    {

        Skill_TouchOfCurseFirst(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {

        }

        public override void FollowUp()
        {
            return;
        }

        public override void SkillEffectOnEnter(GameObject otherobj)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            if (
                ParentPlayer.tag != otherobj.tag
                && ParentPlayer != otherobj
                && MaxHit > curhit
                && MaxTargetNumber > slappedtarget.Count
                && !GameObjectChecker(slappedtarget, otherobj)
                )
            {
                slappedtarget.Add(new Pair<GameObject, int>(otherobj, 1));                   
                if(otherobj.layer == LayerMask.NameToLayer("Player"))
                {
                    HitEnemy(otherobj);
                    GameManager.Instance.GagePus(Ultimatecharge, ParentScript.ID);
                }
                else if(otherobj.layer == LayerMask.NameToLayer("Coffin"))
                {
                    HitCoffin(otherobj);
                }
                curhit++;
            }
        }

        public override void SkillEffectOnExit(GameObject otherobj)
        {
            return;
        }

        public override void SkillEffectOnStay(GameObject otherobj)
        {
            return;
        }


        protected override void HitEnemy(GameObject otherobj)
        {
            if (IsImmortal(otherobj))
            {
                return;
            }

            PlayerController_FSM HitObj = otherobj.GetComponent<PlayerController_FSM>();

            //상대를 날리는 방향
            Vector3 Direction;

            //맞은 객체와 자신의 벡터를 이용해 자신에서 맞은객체 쪽으로 이동하는 벡터를 구함
            Direction = this.transform.position - otherobj.transform.position;
            Direction.y = 0;
            Direction.Normalize();

            GameManager.Instance.CameraShaking(ParentScript.ID);
            GameManager.Instance.Hurt(Direction, debuffDuration, AttackType, Damage, HitSoundNumber, ParentScript.ID ,HitObj.ID);
            //PlayHitSound();
        }

        public override void Restart()
        {
            this.gameObject.transform.position = this.ParentPlayer.transform.position + (this.ParentPlayer.transform.forward * 1.5f) + this.ParentPlayer.transform.up * 0.1f;
            timer = 0f;
            curhit = 0;
            slappedtarget.Clear();
            SetCollider(true);
            PlaySkillSound();
        }
    }

}