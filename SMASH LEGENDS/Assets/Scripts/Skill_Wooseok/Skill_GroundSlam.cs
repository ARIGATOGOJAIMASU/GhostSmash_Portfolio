using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Wooseok
{

    public class Skill_GroundSlam : Skill
    {
        public bool isGround = false;

        [SerializeField]
        public GameObject Lightening_Bolt;

        [SerializeField]
        float colEnd;

        [SerializeField]
        public GameObject Spark;


        Skill_GroundSlam(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
            //
        }

        protected override void Awake()
        {
            base.Awake();
        }

        public void LighteningBolt()
        {
            isGround = true;
            Lightening_Bolt.SetActive(true);
            PlaySkillSound();
        }


        public override void SkillEffectOnEnter(GameObject otherobj)
        {
            if (
                MaxHit > curhit &&
                MaxTargetNumber > slappedtarget.Count &&
                !otherobj.CompareTag(ParentPlayer.tag)
                )
            {
                int count;
                if (!GameObjectChecker(slappedtarget, otherobj)) // 처음 맞는 경우
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
                }
                else // 두번째 맞는 경우
                { 
                    count = TargetFinder(slappedtarget, otherobj);
                    if(count != -1 && slappedtarget[count].Value < MaxHitPerTarget)
                    {
                        if (otherobj.layer == LayerMask.NameToLayer("Player"))
                        {
                            HitEnemy(otherobj);
                            GameManager.Instance.GagePus(Ultimatecharge, ParentScript.ID);
                        }
                        else if (otherobj.layer == LayerMask.NameToLayer("Coffin"))
                        {
                            HitCoffin(otherobj);
                        }
                        Pair<GameObject, int> temppair = slappedtarget[count];
                        temppair.Value += 1;
                        slappedtarget[count] = temppair;
                    }
                }
            }
        }

        protected override void HitEnemy(GameObject otherobj)
        {
            
            if (IsImmortal(otherobj))
            {
                return;
            }

            //상대를 날리는 방향
            Vector3 Direction;

            //맞은 객체와 자신의 벡터를 이용해 자신에서 맞은객체 쪽으로 이동하는 벡터를 구함  
            Direction = -ParentPlayer.transform.up;
            Direction.Normalize();
            Rigidbody otherobjrigidbody = otherobj.GetComponent<Rigidbody>();
            otherobjrigidbody.velocity = Direction * debuffDuration;
            otherobj.transform.position = new Vector3(otherobj.transform.position.x, ParentPlayer.transform.position.y, otherobj.transform.position.z);
            GameManager.Instance.Hurt(Direction, debuffDuration, AttackType, Damage, HitSoundNumber, ParentScript.ID, otherobj.GetComponent<PhotonView>().ViewID);
            GameManager.Instance.CameraShaking(ParentScript.ID);
            GameManager.Instance.GagePus(Ultimatecharge, ParentScript.ID);
            //PlayHitSound();
        }


        public override void SkillEffectOnExit(GameObject otherobj)
        {
            //
        }

        public override void SkillEffectOnStay(GameObject otherobj)
        {
            
        }

        public override void FollowUp()
        {
            //
        }

        private void Update()
        {
        }

        public override void Restart()
        {
            timer = 0f;
            curhit = 0;
            Lightening_Bolt.SetActive(false);
            SetCollider(true);
            //Spark.SetActive(true);
            isGround = false;
            slappedtarget.Clear();
            List<GameObject> cols = this.gameObject.GetComponent<SectorColumnCollider>().ColliderList;
            foreach (GameObject obj in cols)
            {               
                obj.transform.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = true;
            }
        }

        protected override void FixedUpdate()
        {
            if(isGround)
            {
                if (timer < LifeTime)
                {
                    timer += Time.fixedDeltaTime;
                }
            }
            if(timer >= 0.4f)
            {
                //Spark.SetActive(false);
                SetCollider(false);
            }
            else if (timer >= colEnd)
            {
                List<GameObject> cols = this.gameObject.GetComponent<SectorColumnCollider>().ColliderList;

                foreach (GameObject obj in cols)
                {
                    obj.transform.GetChild(0).gameObject.GetComponent<Collider>().enabled = false;
                }
            }
            if(myAudio)
            {
                if (timer >= LifeTime && !myAudio.PlayCheck())
                {
                    this.gameObject.SetActive(false);
                }
            }
            
        }
    }
}