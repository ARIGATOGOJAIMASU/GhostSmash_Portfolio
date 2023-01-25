using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Wooseok
{
    public class Skill_GuidedProjectile : Skill_ProjectileGoUnder
    {
        public enum State { STRAIGHT = 0, GUIDED = 1, HIT = 2 }

        [SerializeField]
        State curstate;
        GameObject target = null;
        [SerializeField]
        float straightmovementendtime;
        [SerializeField]
        float DetectionLength;
        [SerializeField]
        float angularspeed;
        
        protected Skill_GuidedProjectile(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            
            SkillEffectOnEnter(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            SkillEffectOnExit(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            SkillEffectOnStay(other.gameObject);
        }


        protected override void Update() { }

        public override void SetDirection()
        {
            //범위 안에 타겟이 있다면 추적
            if ((curstate == State.GUIDED) && 
                (target != null))
            {
                myrigidbody.velocity = (speed * (LifeTime - timer)) * (myrigidbody.velocity + (target.transform.position - this.transform.position) * Time.deltaTime * (angularspeed * (LifeTime - timer))).normalized;
                transform.LookAt(this.transform.position + myrigidbody.velocity.normalized);
            }
            //범위 안에 타겟이 없다면 전진
            else
            {
                transform.LookAt(this.transform.position + this.direction * Mathf.Cos((angle * Mathf.PI) / 180f) + Vector3.down * Mathf.Sin((angle * Mathf.PI) / 180f));
                myrigidbody.velocity = this.transform.forward * (speed * (LifeTime - timer));
            }  
        }


        protected override void FixedUpdate()
        {
            
            if(timer > straightmovementendtime && curstate != State.HIT)
            {
                changeState(State.GUIDED);
            }
            if (LifeTime > timer)
            {
                timer += Time.fixedDeltaTime;
            }
            else
            {
                if (!isdestroy)
                {
                    isdestroy = true;
                }
                if (!FollowOn)
                {
                    FollowUp();
                }
            }

            if (FollowUpSkill != null)
            {
                if (isdestroy && !FollowUpSkill.gameObject.activeSelf)
                {
                    this.gameObject.SetActive(false);
                }
            }
            else if (isdestroy)
            {
                this.gameObject.SetActive(false);
            }


            if (curstate != State.HIT)
            {
                float minlength;

                //타겟을 포착
                if (target != null)
                {
                    minlength = Vector3.Distance(this.transform.position, target.transform.position);

                    if (minlength > DetectionLength)
                    {
                        target = null;
                        minlength = DetectionLength;
                    }
                }
                //타켓이 없는 경우
                else
                {
                    minlength = DetectionLength;
                }

                //범위 안에 모든 객체의 콜라이더를 저장
                Collider[] Cols = Physics.OverlapSphere(transform.position, minlength);

                //저장한 콜라이더 중에 적이 있는지 검사
                foreach (Collider col in Cols)
                {
                    if (CheckObj(col.gameObject))
                    {
                        float len;
                        len = Vector3.Distance(this.transform.position, col.gameObject.transform.position);

                        if (len < minlength)
                        {
                            minlength = len;
                            target = col.gameObject;
                        }
                    }
                }

                //나온 결과들로 다음 속도 연산
                SetDirection();
            }


        }
        public override void Restart()
        {
            changeState(State.STRAIGHT);
            base.Restart();
            Start();
            
        }

        public override void FollowUp()
        {
            changeState(State.HIT);
            base.FollowUp();
        }

        protected void changeState(State state)
        {
            switch (state)
            {
                case State.GUIDED:
                    myrigidbody.isKinematic = false;
                    Projectile.SetActive(true);
                    //캐싱해야함
                    this.GetComponent<Collider>().enabled = true;
                    break;
                case State.HIT:
                    myrigidbody.isKinematic = true;
                    myrigidbody.velocity = Vector3.zero;
                    Projectile.SetActive(false);
                    this.GetComponent<Collider>().enabled = false;
                    break;
                case State.STRAIGHT:
                    myrigidbody.isKinematic = false;
                    Projectile.SetActive(true);
                    this.GetComponent<Collider>().enabled = true;
                    break;
            
            }

            curstate = state;
        }

    }
}