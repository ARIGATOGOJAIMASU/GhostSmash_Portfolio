using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{

    public class Skill_GangNimSlam : Skill
    {
        [SerializeField]
        GameObject Effect;
        public Skill_GangNimSlam(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }

        protected override void Awake()
        {
            base.Awake();

            cols = this.GetComponentsInChildren<BoxCollider>();
        }
        public override void FollowUp()
        {
            this.FollowUpSkill.gameObject.SetActive(true);
            Effect.SetActive(false);
            this.FollowUpSkill.Restart();
        }

        public override void Restart()
        {
            timer = 0f;
            curhit = 0;
            slappedtarget.Clear();
            Effect.SetActive(true);
            SetCollider(true);
            foreach (var col in cols)
            {
                col.enabled = true;
            }

            if (!IshaveParent)
            {
                transform.position = ParentPlayer.transform.forward * startvector.x + ParentPlayer.transform.up * startvector.y;
            }
            else
            {
                transform.localPosition = Vector3.forward * startvector.x + Vector3.up * startvector.y;
            }

            FollowUpSkill.gameObject.SetActive(false);
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
                if (otherobj.layer == LayerMask.NameToLayer("Player"))
                {
                    HitEnemy(otherobj);
                    GameManager.Instance.GagePus(Ultimatecharge, ParentScript.ID);
                }
                else if (otherobj.layer == LayerMask.NameToLayer("Coffin"))
                {
                    HitCoffin(otherobj);
                }
                curhit++;
                slappedtarget.Add(new Pair<GameObject, int>(otherobj, 1));
            }
        }
        
        public override void SkillEffectOnExit(GameObject otherobj)
        {
            //
        }

        public override void SkillEffectOnStay(GameObject otherobj)
        {
            //
        }

        protected override Vector3 DrawDirection(GameObject otherobj)
        {
            Vector3 Direction;
            Direction = -ParentPlayer.transform.up;
            Direction.Normalize();
            Rigidbody otherobjrigidbody = otherobj.GetComponent<Rigidbody>();
            otherobjrigidbody.velocity = Direction * debuffDuration;
            otherobj.transform.position = new Vector3(otherobj.transform.position.x, ParentPlayer.transform.position.y, otherobj.transform.position.z);
            return Direction;
        }
        private void Update()
        {
            if(FollowUpSkill.gameObject.activeSelf)
            {
                foreach (var col in cols)
                {
                    col.enabled = false;
                }
            }
        }
    }

}