using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Junpyo;
using System.ComponentModel;
using Photon.Pun;

namespace Wooseok
{
    public class Skill_CursedCoffin : Skill
    {
        Rigidbody myrigidbody;
        [SerializeField]
        float Range;
        [SerializeField]
        float interval;
        [SerializeField]
        Dictionary<GameObject, Pair<bool, float>> PlayerTimer = new Dictionary<GameObject, Pair<bool, float>>();
        [SerializeField] bool Ground = false;
        [SerializeField] GameObject[] Effect;
        bool Start;

        [SerializeField]
        Skill_Coffin CoffinData;
        Skill_CursedCoffin(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {

        }
        protected override void Awake()
        {
            base.Awake();
            PlayerTimer = new Dictionary<GameObject, Pair<bool, float>>();
            myrigidbody = this.gameObject.GetComponent<Rigidbody>();
        }
        
        public override void FollowUp()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!Ground && (other.gameObject.layer == LayerMask.NameToLayer("Ground")))
            {
                Ground = true;
                PlaySkillSound();
                timer = 0f;
                transform.position = new Vector3(transform.position.x, other.bounds.max.y, transform.position.z);

                //이펙트 켜기
                foreach (GameObject effect in Effect)
                {
                    effect.SetActive(true);
                }

                //콜라이더 켜기
                foreach (GameObject effect in ColliderArr)
                {
                    effect.SetActive(true);
                }

                myrigidbody.isKinematic = true;
                Start = true;
            }
        }

        public override void SkillEffectOnEnter(GameObject otherobj)
        {
            if (Ground)
            {
                if (
                    otherobj != this.ParentPlayer
                    && !ParentPlayer.CompareTag(otherobj.tag)
                    && otherobj.layer == LayerMask.NameToLayer("Player")
                    )
                {
                    //처음 들어올때 Dictionary에 등록
                    if (!PlayerTimer.ContainsKey(otherobj))
                    {
                        PlayerTimer.Add(otherobj, new Pair<bool, float>(true, 0.0f));
                    }
                    //등록된 객체면 시간 갱신
                    else
                    {
                        Pair<bool, float> temp_pair = PlayerTimer[otherobj];
                        temp_pair.Key = true;
                        PlayerTimer[otherobj] = temp_pair;
                    }
                }
            }
        }

        public override void SkillEffectOnExit(GameObject otherobj)
        {
            if (Ground)
            {
                //객체의 재등록을 피하기 위해 범위안에 없다는 것을 알리는 키값만 false로 변경함
                if (PlayerTimer.ContainsKey(otherobj))
                {
                    Pair<bool, float> pair = PlayerTimer[otherobj];
                    pair.Key = false;
                    PlayerTimer[otherobj] = pair;
                }
            }
            //
        }

        public override void SkillEffectOnStay(GameObject otherobj)
        {
            if (Ground)
            {
                //안들어온 객체들 등록
                if (
                    otherobj != this.ParentPlayer
                    && !ParentPlayer.CompareTag(otherobj.tag)
                    && otherobj.layer == LayerMask.NameToLayer("Player")
                )
                {
                    if (!PlayerTimer.ContainsKey(otherobj))
                    {                 
                        PlayerTimer.Add(otherobj, new Pair<bool, float>(true, 0.0f));
                    }
                    else
                    {
                        Pair<bool, float> pair = PlayerTimer[otherobj];
                        pair.Key = true;
                        PlayerTimer[otherobj] = pair;
                    }
                }


                //저주사태 부여
                if (
                    otherobj.layer == LayerMask.NameToLayer("Player") &&
                    !ParentPlayer.CompareTag(otherobj.tag) &&
                    PlayerTimer.ContainsKey(otherobj)
                    )
                {

                    Pair<bool, float> keyvalue;
                    PlayerTimer.TryGetValue(otherobj, out keyvalue);

                    if (keyvalue.Value > interval)
                    {
                        HitEnemy(otherobj);
                        GameManager.Instance.GagePus(Ultimatecharge, ParentScript.ID);
                        keyvalue.Value = 0.0f;
                        PlayerTimer[otherobj] = keyvalue;
                    }
                }
            }
        }

        //Update is called once per frame
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (Ground)
            {
                //코드 재활용
                
                Pair<bool, float> pair;
                if (PlayerTimer.Count > 0)
                {
                    //딕셔너리 특성상 벨류 수정이 어려움으로 인해 생성 후 값을 받고 수정후에 딕셔너리에 삽입
                    var keylist = new List<GameObject>(PlayerTimer.Keys);
                    for (int i = 0; i < PlayerTimer.Count; i++)
                    {
                        pair = PlayerTimer[keylist[i]];
                        pair.Value += pair.Key ? Time.fixedDeltaTime : 0.0f;
                        PlayerTimer[keylist[i]] = pair;
                    }
                }
                if(Start)
                {
                    //이펙트 연출을 위해 만들었지만 파티클의 이미 있는 기능이기에 수정필요할 수도 있다. Paticle.LifeSize
                    Effect[0].transform.localScale = new Vector3((Effect[0].transform.localScale.x + (Time.fixedDeltaTime * 11f)),
                        1, (Effect[0].transform.localScale.z + (Time.fixedDeltaTime *11f)));
                    
                    if(Effect[0].transform.localScale.x >= 3.7f)
                    {
                        Start = false;
                        Effect[0].transform.localScale = new Vector3(3.7f, 1f, 3.7f);
                    }
                }
            }
        }

        public override void Restart()
        {
            this.tag = this.ParentPlayer.tag;

            if(CoffinData != null)
            {
                //석관 정보 초기화
                if (CoffinData.CurHP <= 0)
                {
                    CoffinData.CurHP = CoffinData.MaxHP;
                }
            }
            
            //위치 설정
            if(!IshaveParent)
            {
                this.gameObject.transform.position = (ParentPlayer.transform.position
                + this.ParentPlayer.transform.forward * Range) + new Vector3(0, startvector.y, 0);
            }
            else
            {
                this.gameObject.transform.localPosition = Vector3.zero;
            }

            //값들 초기화
            timer = 0f;
            curhit = 0;
            slappedtarget.Clear();
            PlayerTimer.Clear();

            //이펙트 초기화
            Effect[0].transform.localScale = Vector3.zero;

            SetCollider(true);

            Ground = false;
            myrigidbody.isKinematic = false;
            myrigidbody.velocity = Vector3.zero;
            
        }

        public override void PlaySkillSound()
        {
            base.PlaySkillSound();
            if (HitSoundNumber != -1)
            {
                myAudio.Play(Juhyung.SoundManager.Instance.AttSound[HitSoundNumber], Juhyung.AudioManager.DEFINE.REPEAT);
            }
        }
    }
}