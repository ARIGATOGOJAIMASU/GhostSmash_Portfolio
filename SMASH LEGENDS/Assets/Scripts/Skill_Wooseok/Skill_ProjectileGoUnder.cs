using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wooseok
{
    public class Skill_ProjectileGoUnder : Skill_Projectile
    {
        [SerializeField]
        protected float angle;


        protected Skill_ProjectileGoUnder(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }

        public override void FollowUp()
        {
            base.FollowUp();
        }

        public override void SkillEffectOnEnter(GameObject otherobj)
        {
            base.SkillEffectOnEnter(otherobj);
        }

        public override void SkillEffectOnExit(GameObject otherobj)
        {
        }

        public override void SkillEffectOnStay(GameObject otherobj)
        {
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            this.gameObject.transform.localPosition = new Vector3(ParentPlayer.transform.position.x + startvector.x * ParentPlayer.transform.forward.x,
                                                                    ParentPlayer.transform.position.y + startvector.y - 0.4f,
                                                                    ParentPlayer.transform.position.z + startvector.x * ParentPlayer.transform.forward.z);
            //this.GetComponent<MeshRenderer>().enabled = true;
            this.gameObject.GetComponent<Collider>().enabled = true;
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            timer = 0f;

            if (this.ParentPlayer)
            {
                this.direction = this.ParentPlayer.transform.forward;
                base.Start();
                SetDirection();
            }
            PlaySkillSound();
        }

        public override void SetDirection()
        {
            this.gameObject.transform.LookAt(this.transform.position + this.direction * Mathf.Cos((angle * Mathf.PI) / 180f) + Vector3.down * Mathf.Sin((angle * Mathf.PI) / 180f));
            myrigidbody.velocity = this.transform.forward * speed;
        }

        // Update is called once per frame

        protected override void Update()
        {
            base.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void Restart()
        {
            base.Restart();
            this.Start();
        }


    }
}