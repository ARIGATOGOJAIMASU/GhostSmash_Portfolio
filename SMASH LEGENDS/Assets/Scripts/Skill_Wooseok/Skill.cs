using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Junpyo;
using Photon.Pun;
using Photon.Realtime;

namespace Wooseok
{
    public abstract class Skill : MonoBehaviour
    {
        // Start is called before the first frame update
        
        

        public enum SKILLTYPE
        {
            BASE = 0,
            SKILL = 1,
            ULTIMATE = 2
        }

        [SerializeField]
        protected Juhyung.AudioManager myAudio;
        public int SkillSoundNumber;
        public int HitSoundNumber;

        public SKILLTYPE SkillType;

        //True인 객체는 Player의 자식으로 생성되며 기본공격같이 Player랑 이동방향이 같은 스킬들에 설정함
        public bool IshaveParent;

        [SerializeField]
        public Collider[] cols;

        [SerializeField]
        public Skill FollowUpSkill;

        [SerializeField]
        protected float LifeTime;
        [SerializeField]
        protected float timer;
        [SerializeField]
        protected int MaxHit; // 스킬 자체의 타수에 제한이 있는 경우 Curhit과 엮어 사용하기.
        [SerializeField]
        protected int curhit;

        [SerializeField]
        protected int MaxHitPerTarget; // 한 대상이 최대 몇타까지 이 스킬을 맞을 수 있는가 체크하기.
        [SerializeField]
        protected int MaxTargetNumber; // 최대 몇 명까지 스킬을 맞아도 되는가?
        [SerializeField]
        public List<Pair<GameObject,int>> slappedtarget; // 현재까지 맞은 대상, 맞은 횟수

        public GameObject ParentPlayer;
        public PlayerController_FSM ParentScript;

        [SerializeField]
        protected ATTACKTYPE AttackType;

        [SerializeField]
        public float Damage;

        [SerializeField]
        protected Vector2 startvector;

        [SerializeField]
        protected float debuffDuration;

        //궁극기 게이지 충전량
        [SerializeField]
        protected int Ultimatecharge;

        [SerializeField]
        protected GameObject[] ColliderArr;

        [SerializeField]
        protected Junpyo.SKILLTYPE MyJunpyoSKILLTYPE;
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        public abstract void SkillEffectOnEnter(GameObject otherobj);
        public abstract void SkillEffectOnStay(GameObject otherobj);
        public abstract void SkillEffectOnExit(GameObject otherobj);

        //리스트에서 Pair의 특정 Key값(2번째 매개변수 제너릭 Target targetobject)를 가진 페어가 있으면 true를 리턴시키고, 아니면 false를 반환함.
        protected bool GameObjectChecker<Target, other>(List<Pair<Target,other>> targetlist, Target targetobject)
        {
            bool result = false;
            foreach (Pair<Target, other> targetpair in targetlist)
            {
                if(result)
                {
                    break;
                }

                if(targetpair.Key.Equals(targetobject))
                {
                    result = true;
                }
            }
            return result;
        }


        //리스트에서 Pair의 특정 Key값(2번째 매개변수 제너릭 Target targetobject)를 가진 페어가 있으면
        //해당 페어가 리스트의 몇번째에 있는지 리턴시키고
        //없으면 -1을 반환함.
        //이 코드 지은놈의 실력 미숙으로 인해 이렇게 C스타일로 지어놨으니 나중에 누가 유지보수 할 때 실패할 시의 리턴값좀 제대로 지어주길 바람.
        protected int TargetFinder<Target,Other>(List<Pair<Target, Other>> targetlist, Target targetobject)
        {
            for (int i = 0; i < targetlist.Count; i++)
            {
                if (targetlist[i].Key.Equals(targetobject))
                {
                    return i;
                }
            }
            return -1;
        }

        public Skill(GameObject ParentPlayer, Skill FollowUp)
        {
            curhit = 0;
            this.ParentPlayer = ParentPlayer;
            this.FollowUpSkill = FollowUp;
        }


        protected virtual void FixedUpdate()
        {
            if (timer < LifeTime)
            {
                timer += Time.fixedDeltaTime;
            }
            else
            {
                SetCollider(false);
            }
            if(myAudio)
            {
                if (!myAudio.PlayCheck() && timer >= LifeTime)
                {
                    this.gameObject.SetActive(false);
                }
            }
            else
            {
                if(timer >= LifeTime)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }

        //게임매니저가 실행하게하삼
        protected virtual void HitEnemy(GameObject otherobj)
        {
            PlayerController_FSM HitObj = otherobj.GetComponent<PlayerController_FSM>();

            //상대를 날리는 방향
            Vector3 Direction;

            //맞은 객체와 자신의 벡터를 이용해 자신에서 맞은객체 쪽으로 이동하는 벡터를 구함
            Direction = DrawDirection(otherobj);
            //맞은 객체에 피격함수를 호출

            GameManager.Instance.GagePus(Ultimatecharge, ParentScript.ID);
            Debug.Log("HurtOn");
            GameManager.Instance.Hurt(Direction, debuffDuration, AttackType, Damage, HitSoundNumber, ParentScript.ID, HitObj.ID);

            //데미지가 0이 아니라면 카메라효과 및 HitEffect연출
            if (Damage != 0)
            {
                AfterEffect(EFFECT.HIT, otherobj.GetComponent<Collider>());
                GameManager.Instance.CameraShaking(ParentScript.ID);
                //PlayHitSound();
            }
        }
        protected void HitFail()
        {
            Debug.Log("Hitfail");
        }

        //무적인지 체크하기. 무적이면 true 돌려줌.
        protected bool IsImmortal(GameObject otherobj)
        {
            if (otherobj.layer == LayerMask.NameToLayer("Imotal"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //상대를 날리는 방향 구하기.
        //맞은 객체와 자신의 벡터를 이용해 자신에서 맞은객체 쪽으로 이동하는 벡터를 구함
        protected virtual Vector3 DrawDirection(GameObject otherobj)
        {
            Vector3 direction = otherobj.transform.position - ParentPlayer.transform.position;
            direction.y = 0;
            direction.Normalize();

            return direction;
        }


        protected virtual void Awake()
        {
            slappedtarget = new List<Pair<GameObject, int>>();

            if(FollowUpSkill != null)
            {
                FollowUpSkill.GetComponent<Skill>().ParentPlayer = this.ParentPlayer;
            }
            if(!IshaveParent)
            {
                transform.parent = null;
            }
            GetCols();
            myAudio = this.GetComponent<Juhyung.AudioManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            //클라이언트가 마스터가 아니면 나감 연산은 마스터클라이어트에서 처리하자
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            Debug.Log("SkillEffect On Enter");
            SkillEffectOnEnter(other.gameObject); //맞으면 바로 효과가 나타나는 유형
        }
        private void OnTriggerStay(Collider other)
        {
            //클라이언트가 마스터가 아니면 나감 연산은 마스터클라이어트에서 처리하자
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            SkillEffectOnStay(other.gameObject); //장판기 형으로, 효과가 나타나는 유형
        }
        private void OnTriggerExit(Collider other)
        {
            //클라이언트가 마스터가 아니면 나감 연산은 마스터클라이어트에서 처리하자
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            SkillEffectOnExit(other.gameObject); //존에서 탈출할 시 효과가 나타나는 유형
        }

        public abstract void FollowUp();

        public abstract void Restart();
        public void SkillStop()
        {
            this.gameObject.SetActive(false);
        }
        
        public void UltimategageCharge()
        {
            ParentScript.UtimateGageUp(Ultimatecharge);
        }

        public void SetParent(GameObject parent)
        {
            ParentPlayer = parent;
            ParentScript = ParentPlayer.GetComponent<PlayerController_FSM>();
        }

        public virtual void HitCoffin(GameObject Coffin)
        {
            if(Coffin.layer == LayerMask.NameToLayer("Coffin"))
            {
               
                Skill_Coffin thisCoffin = Coffin.GetComponent<Skill_Coffin>();
                if(thisCoffin.CurHP <= Damage)
                {
                    GameManager.Instance.Attack((int)Junpyo.SKILLTYPE.ULTIMATE, false, Coffin.GetComponent<Skill>().ParentScript.ID);
                }
                thisCoffin.CurHP -= Damage;
                AfterEffect(Junpyo.EFFECT.HIT, Coffin.GetComponent<Collider>());
                PlayHitSound();

            }
            else
            {
                return;
            }
        }

        public virtual bool CheckObj(GameObject obj)
        {
            if(((obj.layer == LayerMask.NameToLayer("Player")) ||
                (obj.layer == LayerMask.NameToLayer("SuperArmer"))) &&
               !(obj.CompareTag(ParentPlayer.gameObject.tag)) &&
               (obj != ParentPlayer))
            {
                return true;
            }

            return false;
        }

        public void SetCollider(bool ison)
        {
            foreach(var col in cols)
            {
                col.enabled = (ison);
            }
        }

        public void AfterEffect(EFFECT effecttype, Collider Target)
        {
            EffectManager.Instance.EffectInst(effecttype, Target.bounds.ClosestPoint(this.gameObject.transform.position));
        }

        protected virtual void GetCols()
        {
            cols = GetComponentsInChildren<Collider>();
        }
        public virtual void PlaySkillSound()
        {
            if(SkillSoundNumber != -1)
            {
                myAudio.Play(Juhyung.SoundManager.Instance.AttSound[SkillSoundNumber]);
            }
        }
        public virtual void PlayHitSound()
        {
            if(HitSoundNumber != -1)
            {
                myAudio.Play(Juhyung.SoundManager.Instance.HitSound[HitSoundNumber]);
            }
        }
    }
    

}

