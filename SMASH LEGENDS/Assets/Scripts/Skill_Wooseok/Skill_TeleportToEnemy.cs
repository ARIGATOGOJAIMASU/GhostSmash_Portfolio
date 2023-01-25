using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{
    public class Skill_TeleportToEnemy : Skill
    {
        public enum State
        {
            NOTP = 1,
            TPED = 2,
            END = 3
        }

        [SerializeField]
        float SkillRange;
        float mindist;
        GameObject closestenemy = null;
        public State Curstate = State.NOTP;
        public bool isdestroy = false;
        public Skill_TeleportToEnemy(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp) { }
        public override void SkillEffectOnEnter(GameObject otherobj)
        {
            //닿은 게임 오브젝트가 적이면, 리스트에 넣기
            if (Curstate == State.NOTP)
            {
                Debug.Log(otherobj);
                if (LayerMask.NameToLayer("Player") == otherobj.layer &&
                    !ParentPlayer.CompareTag(otherobj.tag) &&
                    otherobj != ParentPlayer &&
                    !GameObjectChecker<GameObject,int>(slappedtarget,otherobj)
                    //준표 오면 해당 적이 사망했는지, 아니면 사망하지 않았는지 확인하는 방법 물어보기
                    )
                {
                    GameManager.Instance.EnemyListAdd(ParentScript.ID, otherobj.GetComponent<Junpyo.PlayerController_FSM>().ID);
                }
            }
        }

        public override void SkillEffectOnExit(GameObject otherobj)
        {
        }

        public override void SkillEffectOnStay(GameObject otherobj)
        {
            //닿은 게임 오브젝트가 적이면, 리스트에 넣기
            if (Curstate == State.NOTP)
            {
                Debug.Log(otherobj);
                if (
                    LayerMask.NameToLayer("Player") == otherobj.layer &&
                    !ParentPlayer.CompareTag(otherobj.tag) &&
                    otherobj != ParentPlayer &&
                    !GameObjectChecker<GameObject, int>(slappedtarget, otherobj)
                    //준표 오면 해당 적이 사망했는지, 아니면 사망하지 않았는지 확인하는 방법 물어보기
                    )
                {
                    GameManager.Instance.EnemyListAdd(ParentScript.ID, otherobj.GetComponent<Junpyo.PlayerController_FSM>().ID);
                    
                    //slappedtarget.Add(new Pair<GameObject, int>(otherobj, 0));
                }
            }
        }

        public override void FollowUp()
        {
            Debug.Log(Curstate);
            if (Curstate == State.TPED)
            {
                FollowUpSkill.gameObject.SetActive(true);
                FollowUpSkill.Restart();
                FollowUpSkill.SetParent(ParentPlayer);
                isdestroy = true;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            mindist = SkillRange;
            closestenemy = null;
        }

        // Update is called once per frame
        void Update()
        {
            if (Curstate == State.NOTP && timer <= 0.5f)
            {
                timer += Time.deltaTime;
            }
            else if (Curstate == State.END)
            {
                timer += Time.deltaTime;
            }

            //Debug.Log(Curstate.ToString());
            //Debug.Log("텔포못함");
            if (Curstate == State.NOTP && timer >= 0.5f)
            {   
                foreach (var enemyPair in slappedtarget)
                {
                    Debug.Log(enemyPair);
                    GameObject enemy = enemyPair.Key; 
                    float thisenemydist = Vector3.Distance(enemy.transform.position, ParentPlayer.transform.position);
                    if (thisenemydist < mindist)
                    {
                        mindist = thisenemydist;
                        closestenemy = enemy;
                    }
                    //리스트에 적이 있으면, 가장 가까운 적에게 돌진
                    //적이 없으면 10m 정면으로 돌격 후 땅찍기
                }
                
                if (closestenemy != null)
                {
                    ParentPlayer.transform.position = new Vector3(closestenemy.transform.position.x - closestenemy.transform.forward.x,
                                                                    ParentPlayer.transform.position.y,
                                                                    closestenemy.transform.position.z - closestenemy.transform.forward.z);
                    // 플레이어를 지정된 적의 포지션에서 적의 정면 * (플레이어 덩치 + 적 덩치) 만큼 뒤의 좌표로 순간이동.                  
                    ParentPlayer.transform.LookAt(new Vector3(closestenemy.transform.position.x , ParentPlayer.transform.position.y, closestenemy.transform.position.z));
                    Curstate = State.TPED;
                    SetCollider(false);
                }
                else
                {
                    ParentPlayer.transform.position += ParentPlayer.transform.forward * SkillRange;
                    ParentPlayer.transform.Rotate(new Vector3(0, 180, 0));
                    Curstate = State.TPED;
                    SetCollider(false);
                }
            }
            if(FollowUpSkill.gameObject.activeSelf)
            {
                Curstate = State.END;
            }

            if(Curstate == State.END &&
                FollowUpSkill.gameObject.activeSelf == false &&
                isdestroy &&
                timer > LifeTime)
            {
                slappedtarget.Clear();
                this.gameObject.SetActive(false);
            }
        }

        public override void Restart()
        {
            timer = 0f;
            curhit = 0;
            slappedtarget.Clear();
            Curstate = State.NOTP;
            isdestroy = false;
            SetCollider(true);
            FollowUpSkill.gameObject.SetActive(false);
            Start();
            PlaySkillSound();
        }

        protected override void FixedUpdate()
        {
            if (timer < LifeTime)
            {
                timer += Time.fixedDeltaTime;
            }
            else
            {
                isdestroy = true;
            }
        }
    }
}