using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Junpyo;

namespace Wooseok
{
    public class Skill_HealAndDamage : Skill_Explosion
    {
        [SerializeField]
        ATTACKTYPE HealType;
        float Healing;

        public override void Restart()
        {
            base.Restart();
            Healing = ParentScript.playerInformation.HP_Max * 0.15f;
            SetCollider(true);
        }

        public Skill_HealAndDamage(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
            
        }
        
        public override void SkillEffectOnEnter(GameObject otherobj)
        {
            base.SkillEffectOnEnter(otherobj);
            if (
               CheckObj(otherobj)
                && MaxHit > curhit
                && MaxTargetNumber > slappedtarget.Count
                && !GameObjectChecker(slappedtarget, otherobj)
                )
            {
                Heal(otherobj);
                slappedtarget.Add(new Pair<GameObject, int>(otherobj, 1));
            }
        }

        public void Heal(GameObject otherobj)
        {

            PlayerController_FSM HitObj = otherobj.GetComponent<PlayerController_FSM>();

            //상대를 날리는 방향
            Vector3 Direction;

            //맞은 객체와 자신의 벡터를 이용해 자신에서 맞은객체 쪽으로 이동하는 벡터를 구함
            Direction = Vector3.zero;
            GameManager.Instance.Hurt(Direction, debuffDuration, HealType, Healing, HitSoundNumber, ParentScript.ID, HitObj.ID);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}