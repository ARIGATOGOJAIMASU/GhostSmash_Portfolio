using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{
    public class Skill_HighAngleGun : Skill
    {
        [SerializeField]
        float speed;
        [SerializeField]
        float Angle;
        Rigidbody myrigidbody;
        Collider mycollider;

        bool Ground = false;
        [SerializeField]
        GameObject projectile;


        protected override void Awake()
        {
            base.Awake();
            myrigidbody = this.GetComponent<Rigidbody>();
            mycollider = this.GetComponent<Collider>();
        }
        public Skill_HighAngleGun(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }

        public override void FollowUp()
        {
            if(FollowUpSkill!= null)
            {
                FollowUpSkill.gameObject.SetActive(true);
                FollowUpSkill.Restart();
            }
        }

        public override void Restart()
        {

            if(FollowUpSkill != null)
            {
                FollowUpSkill.gameObject.SetActive(false);
            }
            timer = 0f;
            curhit = 0;
            slappedtarget.Clear();
            projectile.SetActive(true);
            myrigidbody.isKinematic = false;
            mycollider.enabled =true;
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
            Ground = false;
            Start();
            PlaySkillSound();

        }

        public override void SkillEffectOnEnter(GameObject otherobj)
        {
            if(
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
                    GameManager.Instance.GagePus(Ultimatecharge, ParentScript.ID);
                }
                else if (otherobj.layer == LayerMask.NameToLayer("Coffin"))
                {
                    HitCoffin(otherobj);
                }
                curhit++;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!Ground && (other.gameObject.layer == LayerMask.NameToLayer("Ground")))
            {
                Ground = true;
                FollowUp();
                projectile.SetActive(false);
                this.GetComponent<Collider>().enabled = false;
                myrigidbody.isKinematic = true;
            }
        }

        public override void SkillEffectOnExit(GameObject otherobj)
        {
        }

        public override void SkillEffectOnStay(GameObject otherobj)
        {
            SkillEffectOnEnter(otherobj);
        }

        // Start is called before the first frame update
        void Start()
        {
            SetDirection();
        }
        
        void SetDirection()
        {
            Vector3 direction = this.transform.forward * Mathf.Cos(Mathf.PI * Angle / 180) + this.transform.up * Mathf.Sin(Mathf.PI * Angle / 180);
            myrigidbody.velocity = this.ParentPlayer.transform.forward * speed;
        }

        // Update is called once per frame
        protected override void FixedUpdate()
        {
            if(FollowUpSkill != null)
            {
                if(!FollowUpSkill.gameObject.activeSelf)
                {
                    timer += Time.fixedDeltaTime;
                }
            }
            else
            {
                timer += Time.fixedDeltaTime;
            }
            if(FollowUpSkill != null)
            {
                if(timer >= LifeTime && !FollowUpSkill.gameObject.activeSelf && !myAudio.PlayCheck())
                {
                    this.gameObject.SetActive(false);
                }
            }
            else
            {
                if (timer >= LifeTime)
                {
                    if(myAudio)
                    {
                        if(!myAudio.PlayCheck())
                        {
                            this.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        this.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

}