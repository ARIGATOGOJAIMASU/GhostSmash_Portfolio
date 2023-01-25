using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Junpyo;

namespace Wooseok
{

    public class Skill_BringofDeath : Skill
    {
        [SerializeField]
        int count;

        [SerializeField]
        List<float> DmgList;
        [SerializeField]
        List<ATTACKTYPE> ATKtypeList;
        [SerializeField]
        List<int> HitSoundNumberList;
        [SerializeField]
        float Firerate;

        Collider hitbox;
        public bool ison;
        Animator myAnimator;

        [SerializeField] GameObject[] BeamList;

        Skill_BringofDeath(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            SkillEffectOnEnter(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            SkillEffectOnStay(other.gameObject);
        }

        public override void FollowUp()
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
                    Debug.Log("count: " + count);
                    Debug.Log("여기까지는 들어감");
                }
                else if (otherobj.layer == LayerMask.NameToLayer("Coffin"))
                {
                    Debug.Log("Coffin 타격 진입");
                    HitCoffin(otherobj);
                }

                curhit++;
            }
        }

        public override void SkillEffectOnExit(GameObject otherobj)
        {
            
        }

        public override void SkillEffectOnStay(GameObject otherobj)
        {
            if (otherobj.layer == LayerMask.NameToLayer("Player") || otherobj.layer == LayerMask.NameToLayer("Coffin"))
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

                    PlayerController_FSM HitObj = otherobj.GetComponent<PlayerController_FSM>();

                    if (otherobj.layer == LayerMask.NameToLayer("Player"))
                    {
                        Debug.Log("count: " + count);
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
        }
        // Start is called before the first frame update

        protected override void HitEnemy(GameObject otherobj)
        {
            PlayerController_FSM HitObj = otherobj.GetComponent<PlayerController_FSM>();

            //상대를 날리는 방향
            Vector3 Direction;

            //맞은 객체와 자신의 벡터를 이용해 자신에서 맞은객체 쪽으로 이동하는 벡터를 구함
            Direction = DrawDirection(otherobj);
            //맞은 객체에 피격함수를 호출

            GameManager.Instance.GagePus(Ultimatecharge, ParentScript.ID);
            Debug.Log("HurtOn");
            GameManager.Instance.Hurt(Direction, debuffDuration, ATKtypeList[count], DmgList[count], HitSoundNumber, ParentScript.ID, HitObj.ID);

            //데미지가 0이 아니라면 카메라효과 및 HitEffect연출
            if (Damage != 0)
            {
                AfterEffect(EFFECT.HIT, otherobj.GetComponent<Collider>());
                GameManager.Instance.CameraShaking(ParentScript.ID);
                //PlayHitSound();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            hitbox = this.gameObject.GetComponent<Collider>();
            myAnimator = this.gameObject.GetComponent<Animator>();
            myAnimator.speed = Firerate;
        }
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
        public void Hit()
        {
            Damage = DmgList[count];
            AttackType = ATKtypeList[count];
            HitSoundNumber = HitSoundNumberList[count];
            slappedtarget.Clear();
            curhit = 0;
            count++;
            
        }
        protected override void FixedUpdate()
        {
            if (ison)
            {
                SetCollider(false);
            }
            if (myAudio)
            {
                if (!myAudio.PlayCheck() && ison)
                {
                    ison = false;
                    this.gameObject.SetActive(false);
                }
            }
        }

        public override void Restart()
        {
            this.gameObject.transform.position = this.ParentPlayer.transform.position + 
                (this.ParentPlayer.transform.forward * startvector.x)
                + this.ParentPlayer.transform.up * startvector.y;

           /* foreach(GameObject beam in BeamList)
            {
                beam.transform.localRotation = ParentPlayer.transform.localRotation;
            }*/
            
            if(hitbox != null)
            {
                hitbox.enabled = false;
            }
            timer = 0f;
            curhit = 0;
            count = 0;
            slappedtarget.Clear();
            Start();
            myAnimator.Play("Play");
            Debug.Log("BringOfDeath restart");
            PlaySkillSound();
            SetCollider(true);
            ison = false;
        }
    }

}