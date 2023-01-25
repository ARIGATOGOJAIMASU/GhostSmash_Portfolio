using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Wooseok
{
    public class Skill_Projectile : Skill
    {
        protected bool isdestroy;
        protected Vector3 direction;
        [SerializeField]
        protected float speed;
        [SerializeField]
        protected GameObject Projectile;
        protected bool FollowOn = false;
        protected Rigidbody myrigidbody;

        public bool getcol;

        [SerializeField]
        protected int FlyingSoundNumber;

        protected override void Awake()
        {
            slappedtarget = new List<Pair<GameObject, int>>();

            if (FollowUpSkill != null)
            {
                FollowUpSkill.GetComponent<Skill>().ParentPlayer = this.ParentPlayer;
            }
            if (!IshaveParent)
            {
                transform.parent = null;
            }
            if (getcol)
            {
                GetCols();
            }
            myAudio = this.GetComponent<Juhyung.AudioManager>();
            slappedtarget = new List<Pair<GameObject, int>>();
            myrigidbody = this.GetComponent<Rigidbody>();
            if(FollowUpSkill != null)
            {
                FollowUpSkill.ParentPlayer = this.ParentPlayer;
            }
        }

        protected override void GetCols()
        {
            base.GetCols();
            int size = cols.Length;
            for(int i = 0; i < size; i++)
            {
                foreach(Collider innercol in FollowUpSkill.cols)
                {
                    if(cols[i] == innercol)
                    {
                        cols[i] = null;
                    }
                }
            }
        }

        protected Skill_Projectile(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }

        public override void FollowUp()
        {
            if(FollowUpSkill != null)
            {
                if (!FollowUpSkill.IshaveParent)
                {
                    FollowUpSkill.transform.position = this.transform.position;
                }
                if(!FollowOn)
                {
                    FollowUpSkill.gameObject.SetActive(true);
                    FollowUpSkill.Restart();
                }
            }

            FollowOn = true;
            StopProjectile();
            //this.gameObject.SetActive(false);
            //Instantiate(this.FollowUpSkill, this.gameObject.transform.position, this.gameObject.transform.rotation);


            //생성하고 죽음
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
                GameManager.Instance.FollowUpSkill((int)MyJunpyoSKILLTYPE ,ParentScript.ID);
                //GameManager.Instance.FollowUpSkill(ParentScript.player ,ParentScript.ID);
                if(curhit >= MaxHit)
                {
                    
                    isdestroy = true;
                }
            }
            else if (otherobj.tag == "Ground" && otherobj.layer == LayerMask.NameToLayer("Ground"))
            {
                GameManager.Instance.FollowUpSkill((int)MyJunpyoSKILLTYPE, ParentScript.ID);
                isdestroy = true;
            }
        }

        public override bool CheckObj(GameObject obj)
        {
            if(
                (obj.layer == LayerMask.NameToLayer("Player") || obj.layer == LayerMask.NameToLayer("SuperArmer")) &&
               !(obj.CompareTag(ParentPlayer.gameObject.tag)) &&
               (obj != ParentPlayer)
               )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void SkillEffectOnExit(GameObject otherobj)
        {
        }

        public override void SkillEffectOnStay(GameObject otherobj)
        {
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            this.gameObject.transform.localPosition = new Vector3(  ParentPlayer.transform.position.x + startvector.x * ParentPlayer.transform.forward.x,
                                                                    ParentPlayer.transform.position.y + startvector.y, 
                                                                    ParentPlayer.transform.position.z + startvector.x * ParentPlayer.transform.forward.z);
            //this.GetComponent<MeshRenderer>().enabled = true;
            this.gameObject.GetComponent<Collider>().enabled = true;
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            timer = 0f;
            if(this.ParentPlayer)
            { 
                this.direction = this.ParentPlayer.transform.forward;
                
            }
            else
            {
                this.direction = Vector3.forward;
            }
            SetDirection();
        }

        public virtual void SetDirection()
        {
            this.gameObject.transform.LookAt(this.transform.position + this.direction);
            this.GetComponent<Rigidbody>().velocity = this.transform.forward * speed;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if(FollowUpSkill != null)
            {
                if (isdestroy && !FollowUpSkill.gameObject.activeSelf)
                {
                    SetCollider(false);
                    if(!myAudio.PlayCheck())
                    {
                        this.gameObject.SetActive(false);
                    }
                }
                
            }
            else if(isdestroy)
            {
                SetCollider(false);
                if(!myAudio.PlayCheck())
                {
                    this.gameObject.SetActive(false);
                }
            }
            
        }

        protected override void FixedUpdate()
        {
            if (LifeTime > timer)
            {
                timer += Time.fixedDeltaTime;
            }
            else
            {
                if(!isdestroy)
                {
                    isdestroy = true;
                }
                if(!FollowOn)
                {
                    if (PhotonNetwork.IsConnected)
                    {
                        GameManager.Instance.FollowUpSkill((int)MyJunpyoSKILLTYPE, ParentScript.ID);
                    }
                    else
                    {
                        FollowUp();
                    }
                }
            }
        }

        public override void Restart()
        {
            timer = 0f;
            curhit = 0;
            slappedtarget.Clear();
            Projectile.SetActive(true);
            isdestroy = false;
            if(FollowUpSkill != null)
            {
                FollowUpSkill.gameObject.SetActive(false);
            }
            Start();
            FollowOn = false;
            SetCollider(true);
            PlaySkillSound();
        }

        public void StopProjectile()
        {
            this.gameObject.GetComponent<Collider>().enabled = false;
            myrigidbody.velocity = Vector3.zero;
            myrigidbody.isKinematic = true;
            Projectile.SetActive(false);
        }

        public override void PlaySkillSound()
        {
            if (SkillSoundNumber != -1)
            {
                myAudio.Play(Juhyung.SoundManager.Instance.AttSound[SkillSoundNumber]);
            }
            if (FlyingSoundNumber != -1)
            {
                myAudio.Play(Juhyung.SoundManager.Instance.AttSound[FlyingSoundNumber], Juhyung.AudioManager.DEFINE.REPEAT);
            }
        }
    }
}