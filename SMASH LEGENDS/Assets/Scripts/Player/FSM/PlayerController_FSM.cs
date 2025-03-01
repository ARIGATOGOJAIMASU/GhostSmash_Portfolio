using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Wooseok;
using UnityEngine.SceneManagement;

namespace Junpyo
{
    public class PlayerController_FSM : MonoBehaviourPunCallbacks, IPunObservable
    {
        //Player Componenet
        [HideInInspector] public Rigidbody playerRigidbody;
        [HideInInspector] public Animator playerAnimator;
        [HideInInspector] public Juhyung.Sound3DManager Sound3DManager;

        //FSM기법을 활용한 StateMachine
        [HideInInspector] public State_Machine state_Machine;

        //그라운드 체크
        public Transform GroundPos;
        private GroundCheack GroundScript;

        //캐릭터의 Stat
        public PlayerInformation playerInformation;

        //Player Lookat
        [HideInInspector] public Vector3 PlayerLook;

        //Camera
        [SerializeField] GameObject CameraManager;
        [HideInInspector] public CameraTarget CameraTarget;
        [HideInInspector] public CameraManager CamManager;

        //Ik_Controller
        [HideInInspector] public Ik_Controller IK_Controller;

        //플레이어상태UI
        [Header("PlayerConditionUI")]
        [SerializeField] public Canvas playerCanvas;
        [SerializeField] public HP_Value hp_Value;
        [SerializeField] public Image StaminaUI;
        [SerializeField] public Image HP_Bar;
        [Header("")]

        //적팀 HP바 이미지
        [SerializeField] public Sprite Enemy_HP_Bar_Sprite;

        //플레이어 UI
        [HideInInspector] public GameObject PlayerUI;
        [HideInInspector] public Transform RevivalUI;
        [HideInInspector] public Image RevivalWaitingImage;

        //AttackUI
        [HideInInspector] public SkillUI Skill_UI;
        [HideInInspector] public UltimateUI Ultimate_UI;

        //Dodge_Ui
        [HideInInspector] public Dodge_UI dodge_UI;

        //SpawnPos
        [HideInInspector] public Vector3 SpawnPos;

        //DeadFlyState시 날라가는 방향을 저장
        [HideInInspector] public Vector3 DeadFlyDirection;

        [HideInInspector] public bool IsUpdate;

        //상태이상 지속시간
        float FearTime = 5.0f;
        float CurceTime = 5.0f;

        //Netework 지연시간 연산용 변수
        [HideInInspector] public Vector3 networkPosition;
        [HideInInspector] public Quaternion networkRotation;
        [HideInInspector] public Vector3 networkVelocity;

        //자신의 바라보는 방향을 나타내는 UI
        [SerializeField] public GameObject LookCheckFrefab;
        [HideInInspector] public GameObject LookCheck;

        [HideInInspector] public int ID;
        [HideInInspector] public bool IsMine = false;

        //회피기
        [HideInInspector] float DodgeCoolTime = 30.0f;
        //회피기 사용가능 여부
        [HideInInspector] public bool isDodge = true;

        //적감지 콜라이더
        [HideInInspector]  public EenemyCheck enemyCheck;

        //BounsHurt처리 변수
        [HideInInspector] public Vector3 BounsDir;
        [HideInInspector] public float BounsStrong;
        [HideInInspector] public bool AirborneRebound = true;
        [HideInInspector] public Vector3 AirborneDir;

        //스테이트 바꾸기용
        public PLAYERSTATE WantState;

        //PlayerSkill
        [SerializeField] public PlayerSkill playerSkill;

        public CHARACTERNAME CharacterName;

        //ClinetNicName
        [SerializeField] public string Nicname;

        //자신을 처치한 대상의 캐릭터이름
        [HideInInspector] public CHARACTERNAME EnemyCharacter;

        //자신의 죽을 시 추락사 일 경우 활성화 시킴
        [HideInInspector] public bool fallDead;

        //True일 경우에 GroundDownState로 전이 가능하다.
        [HideInInspector] public bool OnGround = false;

        //아이템
        [HideInInspector] public bool ItemTrigger;
        [HideInInspector] public bool GetBoom;
        public GameObject Boom;

        //점프 스테이션에 닿을시 날라가는 방향
        [HideInInspector] public Vector3 UseJumpDirection;

        [Header("상태이상")]
        [SerializeField] GameObject HillEffect;
        [SerializeField] GameObject FearEffect;
        [SerializeField] GameObject CurceEffect;
        [SerializeField] GameObject StunEffect;
        [SerializeField] GameObject DamageEffect;
        [HideInInspector] GameObject CurEffect;
        [Header("")]

        [SerializeField] PLAYERSTATE CUrState;

        private Vector3 PreVelocity;

        [SerializeField] Sprite[] AttackSprite;

        [HideInInspector] Juhyung.AudioManager myAudio;

        protected void Awake()
        {
            //Character Setting
            playerRigidbody = GetComponent<Rigidbody>();
            playerAnimator = GetComponent<Animator>();
            IK_Controller = GetComponent<Ik_Controller>();
            GroundScript = GroundPos.GetComponent<GroundCheack>();
            playerInformation.Initialization(CharacterName);
            hp_Value.SetHP_Value((int)playerInformation.Cur_HP);
            ID = photonView.ViewID;
            myAudio = GetComponent<Juhyung.AudioManager>();
            IsUpdate = true;

            if (PhotonNetwork.IsConnected)
            {
                Nicname = photonView.Owner.NickName;
                IsUpdate = false;
            }

            gameObject.name = photonView.ViewID.ToString();

            // 내 캐릭터가 아닌경우
            if (photonView.IsMine == false)
                return;

            IsMine = true;

            //Camera생성
            GameObject Camera = GameObject.Instantiate(CameraManager);
            Camera.name = "CameraManager";

            //카메라타겟팅 지정
            CameraTarget = GameObject.Find("CameraTarget").GetComponent<CameraTarget>();
            CameraTarget.TagetObj = transform;

            //LookChack생성
            LookCheck = Instantiate<GameObject>(LookCheckFrefab, transform);

            enemyCheck = transform.GetChild(5).GetComponent<EenemyCheck>();

            //UI설정
            PlayerUI = GameObject.Find("PlayerUI");
            RevivalUI = PlayerUI.transform.GetChild(3);
            RevivalWaitingImage = RevivalUI.GetChild(2).GetComponent<Image>();
            dodge_UI = PlayerUI.transform.GetChild(4).GetComponent<Dodge_UI>();

            //AttackSprite변경
            PlayerUI.transform.GetChild(0).transform.GetChild(2).GetComponent<Image>().sprite = AttackSprite[0];

            Skill_UI = PlayerUI.transform.GetChild(1).GetComponent<SkillUI>();
            Skill_UI.transform.GetChild(2).GetComponent<Image>().sprite = AttackSprite[1];

            Ultimate_UI = PlayerUI.transform.GetChild(2).GetComponent<UltimateUI>();
            Ultimate_UI.MaxGage = playerInformation.UltGage_Max;
            Ultimate_UI.transform.GetChild(2).GetComponent<Image>().sprite = AttackSprite[2];

            //StateMachin 필요한 State들 할당
            state_Machine = new State_Machine();
            state_Machine._Owner = transform;

            //기본 State들 StateMachine에 할당
            state_Machine.StateAdd(new IdleState(), PLAYERSTATE.IDLE);
            state_Machine.StateAdd(new RunState(), PLAYERSTATE.RUN);
            state_Machine.StateAdd(new BaseAttackState(), PLAYERSTATE.BASEATTACK);
            state_Machine.StateAdd(new JumpState(), PLAYERSTATE.JUMP);
            state_Machine.StateAdd(new LandState(), PLAYERSTATE.LAND);
            state_Machine.StateAdd(new HangState(), PLAYERSTATE.HANG);
            state_Machine.StateAdd(new HangAttackPrepare(), PLAYERSTATE.HANGATTACKPREPARE);
            state_Machine.StateAdd(new HangAttackState(), PLAYERSTATE.HANGATTACK);
            state_Machine.StateAdd(new HurtState(), PLAYERSTATE.HURT);
            state_Machine.StateAdd(new AirborneState(), PLAYERSTATE.AIRBORNE);
            state_Machine.StateAdd(new AirState(), PLAYERSTATE.AIR);
            state_Machine.StateAdd(new JumpAttackState(), PLAYERSTATE.JUMPATTACK);
            state_Machine.StateAdd(new GroundDownState(), PLAYERSTATE.GROUNDDOWN);
            state_Machine.StateAdd(new RollingState(), PLAYERSTATE.ROLLING);
            state_Machine.StateAdd(new DeadHighlightState(), PLAYERSTATE.DEADHIGHLIGHT);
            state_Machine.StateAdd(new DeadFlyState(), PLAYERSTATE.DEADFLY);
            state_Machine.StateAdd(new DeadState(), PLAYERSTATE.DEAD);
            state_Machine.StateAdd(new SmashState(), PLAYERSTATE.SMASH);
            state_Machine.StateAdd(new SkillState(), PLAYERSTATE.SKILL);
            state_Machine.StateAdd(new JumpSkillState(), PLAYERSTATE.JUMPSKILL);
            state_Machine.StateAdd(new DodgeState(), PLAYERSTATE.DODGE);
            state_Machine.StateAdd(new StunState(), PLAYERSTATE.STUN);
            state_Machine.StateAdd(new CurceState(), PLAYERSTATE.CURCE);
            state_Machine.StateAdd(new UltimateState(), PLAYERSTATE.ULTIMATE);
            state_Machine.StateAdd(new DropState(), PLAYERSTATE.DROP);
            state_Machine.StateAdd(new BounsHurtState(), PLAYERSTATE.BOUNSHURT);
            state_Machine.StateAdd(new StandUpState(), PLAYERSTATE.STANDUP);
            state_Machine.StateAdd(new StandUpAttackState(), PLAYERSTATE.STANDUPATTACK);
            state_Machine.StateAdd(new DodgeAirState(), PLAYERSTATE.DODGEAIR);
            state_Machine.StateAdd(new UseJumpStationState(), PLAYERSTATE.USE_JUMPSTATION);
            state_Machine.StateAdd(new GetItemState(), PLAYERSTATE.GETITEM);
            state_Machine.StateAdd(new ThrowState(), PLAYERSTATE.THROW);

            state_Machine.ChangeState(PLAYERSTATE.IDLE);

            //카메라 설정
            CamManager = Camera.GetComponent<CameraManager>();
            CamManager.target = CameraTarget.transform;
            CamManager.transform.position = CamManager.transform.forward * -9.0f + transform.position;
        }

        protected void Update()
        {
            if (photonView.IsMine == false)
                return;

            if (IsUpdate)
            {
                //현재 자신의 State를 확인하는 용도
                CUrState = state_Machine._CurState;

                //아이템 상호작용
                if (ItemTrigger && 
                    Input.GetKeyDown(KeyCode.Z))
                {
                    if (((state_Machine._CurState == PLAYERSTATE.IDLE) ||
                    (state_Machine._CurState == PLAYERSTATE.RUN)))
                    {
                        ItemTrigger = false;
                        Juhyung.ItemManager.Instance.s_PlayerNum = this;
                        state_Machine.ChangeState(PLAYERSTATE.GETITEM);
                    }
                }

                state_Machine.Update();

                //회피기
                if (Input.GetKeyDown(KeyCode.LeftShift) &&
                    (state_Machine._CurState != PLAYERSTATE.ULTIMATE) &&
                    (state_Machine._CurState != PLAYERSTATE.ULTIMATEPREPARE))
                {
                    //행동 초기화 후 모든 딜레이 체크시작
                    ReSet();
                    StartCoroutine(DodgeDelay());

                    //사용중인 스킬 캔슬
                    if ((state_Machine._CurState == PLAYERSTATE.SKILL) && playerInformation.SkillCansle)
                    {
                        GameManager.Instance.Attack((int)SKILLTYPE.SKILL, false, ID);
                    }
                    else if (((state_Machine._CurState == PLAYERSTATE.JUMPSKILL) || (state_Machine._CurState == PLAYERSTATE.GANGNIM_JUMPSKILLSTATE))
                        && playerInformation.JumpSkillCansle)
                    {
                        GameManager.Instance.Attack((int)SKILLTYPE.CURSKILL, false, ID);
                    }
                    else if ((state_Machine._CurState == PLAYERSTATE.BASEATTACK))
                    {
                        GameManager.Instance.Attack((int)SKILLTYPE.CURSKILL, false, ID);
                    }

                    state_Machine.ChangeState(PLAYERSTATE.DODGE);
                }

                //State확인 용으로 만든 코드
                if (Input.GetKeyDown(KeyCode.P))
                {
                    WantStateChnage();
                }
            }
        }

        protected void FixedUpdate()
        {
            if (photonView.IsMine)
            {
                state_Machine.FixedUpdate();
            }
            else
            {
                playerRigidbody.velocity = networkVelocity;

                //position네트워크 지연시간을 연산하여 Pos수정
                if ((playerRigidbody.position - networkPosition).magnitude < 0.5f)
                {
                    playerRigidbody.position = Vector3.Lerp(GetComponent<Rigidbody>().position, networkPosition, 10 * Time.fixedDeltaTime);
                }
                else
                {
                    playerRigidbody.position = networkPosition;
                }

                playerRigidbody.rotation = networkRotation;
            }

        }

        //매달리기 시스템
        public void Hang(Vector3 HangPosition, Vector3 Angel)
        {
            if (photonView.IsMine)
            {
                transform.position = HangPosition;
                transform.rotation = Quaternion.Euler(Angel);
                state_Machine.ChangeState(PLAYERSTATE.HANG);          
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (photonView.IsMine)
            {
                //점프대 일 경우
                if (other.transform.CompareTag("JumpStation"))
                {
                    UseJumpDirection = other.transform.up;
                    state_Machine.ChangeState(PLAYERSTATE.USE_JUMPSTATION);
                }
                //아이템인 경우
                else if ((other.gameObject.layer == LayerMask.NameToLayer("Item")))
                {
                    ItemTrigger = true;
                    Juhyung.ItemManager.Instance.s_Getitem = other.name;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (photonView.IsMine)
            {
                //아이템콜라이더하고 떨어질때
                if ((other.gameObject.layer == LayerMask.NameToLayer("Item")))
                {
                    ItemTrigger = false;
                    Juhyung.ItemManager.Instance.s_Getitem = null;
                }
            }
        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (photonView.IsMine)
            {
                //땅에 닿고 있는 상태에서 에어본 일때 
                if (collision.transform.tag == "Ground" || collision.transform.CompareTag("Stairs"))
                {
                    //Ground의 y값보다 자신의 y값이 높으면 들어감.
                    if ((collision.collider.bounds.max.y - 0.1f) <= transform.position.y)
                    {
                        //공중에 있는 State들일때만 검사
                        if ((state_Machine._CurState == PLAYERSTATE.JUMPATTACK) ||
                            (state_Machine._CurState == PLAYERSTATE.AIR) ||
                            (state_Machine._CurState == PLAYERSTATE.HURT) ||
                            (state_Machine._CurState == PLAYERSTATE.JUMP) ||
                            (state_Machine._CurState == PLAYERSTATE.DODGEAIR) ||
                            (state_Machine._CurState == PLAYERSTATE.USE_JUMPSTATION) ||
                            (state_Machine._CurState == PLAYERSTATE.THROW) ||
                            (state_Machine._CurState == PLAYERSTATE.TRUELOVE_JUMPATTACKSUCCESS))
                        {
                            transform.position = new Vector3(transform.position.x, collision.collider.bounds.max.y + 0.1f, transform.position.z);
                            state_Machine.ChangeState(PLAYERSTATE.LAND);
                        }

                        //Airborn
                        if ((state_Machine._CurState == PLAYERSTATE.AIRBORNE))
                        {
                            if (OnGround)
                            {
                                state_Machine.ChangeState(PLAYERSTATE.GROUNDDOWN);
                            }
                            else
                            {
                                AirborneRebound = false;
                                playerRigidbody.velocity = new Vector3(-AirborneDir.x, AirborneDir.y, -AirborneDir.z);
                            }
                        }
                    }
                }

                //계단과의 충돌처리 Check
                if (collision.transform.CompareTag("Stairs") &&
                    (state_Machine._CurState == PLAYERSTATE.RUN))
                {
                    if ((collision.transform.position.y + 0.4f) > transform.position.y)
                    {
                        transform.position += new Vector3(0, 0.3f, 0);
                    }
                }
            }
        }

        public void OnCollisionStay(Collision collision)
        {
            if (photonView.IsMine)
            {
                //
                if (collision.transform.CompareTag("Ground") || collision.transform.CompareTag("Stairs"))
                {
                    //회피기 사용하고 나서 땅에 닿았을때의 처리
                    if((state_Machine._CurState == PLAYERSTATE.DODGEAIR))
                    {
                        transform.position = new Vector3(transform.position.x, collision.collider.bounds.max.y + 0.1f, transform.position.z);
                        state_Machine.ChangeState(PLAYERSTATE.LAND);
                    }

                    if (OnGround)
                    {
                        //Ground의 y값보다 자신의 y값이 낮을 경우 pos를 계속해서 낮춤.
                        if (collision.collider.bounds.max.y > transform.position.y)
                        {
                            if ((state_Machine._CurState == PLAYERSTATE.JUMP) ||
                                (state_Machine._CurState == PLAYERSTATE.DODGEAIR))
                            {
                                transform.position += new Vector3(0, -0.03f, 0);
                            }
                        }
                        else if(state_Machine._CurState == PLAYERSTATE.AIRBORNE)
                        {
                            state_Machine.ChangeState(PLAYERSTATE.GROUNDDOWN);
                        }
                    }
                    //날라가는 상황시 바로 다른 벽에 충돌하면 부딪힌 방향의 반대방향으로 튕김
                    else if ((state_Machine._CurState == PLAYERSTATE.AIRBORNE) && AirborneRebound)
                    {
                        AirborneRebound = false;
                        playerRigidbody.velocity = new Vector3(-AirborneDir.x, AirborneDir.y, -AirborneDir.z);
                    }

                    //계단과의 충돌처리 Check
                    if (collision.transform.CompareTag("Stairs") &&
                        (state_Machine._CurState == PLAYERSTATE.RUN))
                    {
                        if ((collision.transform.position.y + 0.4f) > transform.position.y)
                        {
                            transform.position += new Vector3(0, 0.3f, 0);
                        }
                    }
                }
            }
        }

        public void Hurt(Vector3 direction, float debuff, ATTACKTYPE type, float damage)
        {
            if (photonView.IsMine)
            {
                //슈퍼 아머일 경우 데미지만 받고 피격은 되지 않는다.
                //기본상태로 초기화
                ReSet();

                //HP가 0일시 죽는 연출
                if (playerInformation.Cur_HP <= 0)
                {
                    gameObject.layer = LayerMask.NameToLayer("Dead");
                    DeadFlyDirection = direction;
                    state_Machine.ChangeState(PLAYERSTATE.DEADHIGHLIGHT);
                }
                else
                {
                    if (damage > 0)
                    {
                        //타격감 연출
                        EffectManager.Instance.EffectInst(EFFECT.DAMAGE, transform.position, (int)damage);
                        CamManager.CameraShaking();

                        //슈퍼아머일 경우 데미지처리만 받기 때문에 탈출
                        if (gameObject.layer == LayerMask.NameToLayer("SuperArmer"))
                        {
                            return;
                        }

                        playerRigidbody.velocity = Vector3.zero;

                        //스킬이 사용중이였을때 스킬사용을 중지시킴
                        if ((state_Machine._CurState == PLAYERSTATE.SKILL) && playerInformation.SkillCansle)
                        {
                            GameManager.Instance.Attack((int)SKILLTYPE.SKILL, false, ID);
                        }
                        else if(((state_Machine._CurState == PLAYERSTATE.JUMPSKILL) || (state_Machine._CurState == PLAYERSTATE.GANGNIM_JUMPSKILLSTATE))
                            && playerInformation.JumpSkillCansle)
                        {
                            GameManager.Instance.Attack((int)SKILLTYPE.CURSKILL, false, ID);
                        }
                        else if((state_Machine._CurState == PLAYERSTATE.BASEATTACK))
                        {
                            GameManager.Instance.Attack((int)SKILLTYPE.CURSKILL, false, ID);
                        }

                        //Ult_Gage Puse
                        if (!playerInformation.UltimateOn)
                        {
                            UtimateGageUp(5);
                        }

                        //땅과의 충돌처리를 벗어나게 하기위해 임의로 위로 올림
                        playerRigidbody.MovePosition(transform.position + new Vector3(0, 0.2f, 0));
                    }

                    //Ataacktype따라 받는 효과를 다르게 설정
                    switch (type)
                    {
                        case ATTACKTYPE.LIGHTATTACK:
                            {
                                playerRigidbody.MovePosition(transform.position + new Vector3(0, 0.2f, 0));
                                playerRigidbody.velocity = (direction * 2.0f) + new Vector3(0, 3.0f, 0);
                                state_Machine.ChangeState(PLAYERSTATE.HURT);
                            }
                            break;

                        case ATTACKTYPE.MIDDLEATTACK:
                            {
                                playerRigidbody.MovePosition(transform.position + new Vector3(0, 0.2f, 0));
                                playerRigidbody.velocity = (direction * 10.0f) + new Vector3(0, 6.0f, 0);
                                state_Machine.ChangeState(PLAYERSTATE.AIRBORNE);
                            }
                            break;

                        case ATTACKTYPE.HEAVYATTACK:
                            { 
                            playerRigidbody.MovePosition(transform.position + new Vector3(0, 0.2f, 0));
                            playerRigidbody.velocity = (direction * 15.0f) + new Vector3(0, 7.0f, 0);
                            state_Machine.ChangeState(PLAYERSTATE.AIRBORNE);
                            }
                            break;

                        case ATTACKTYPE.STUN:
                            {
                                playerRigidbody.MovePosition(transform.position + new Vector3(0, 0.2f, 0));
                                playerRigidbody.velocity = (direction * 2.0f) + new Vector3(0, 3.0f, 0);
                                state_Machine.ChangeState(PLAYERSTATE.STUN);
                            }
                            break;

                        case ATTACKTYPE.CURSE:
                            {
                                if (playerInformation.Curce)
                                {
                                    StopCoroutine(CurceDuration());
                                    playerInformation.CurRunSpeed = playerInformation.RunSpeed;
                                    playerInformation.CurJumpDistance = playerInformation.JumpDistance;
                                    playerInformation.Curce = false;
                                }

                                StartCoroutine(CurceDuration());
                            }
                            break;

                        case ATTACKTYPE.BOUNCE:
                            {
                                BounsDir = direction;
                                BounsDir.y = -debuff * 3;
                                BounsStrong = debuff;
                                playerRigidbody.velocity = (BounsDir * debuff);
                                state_Machine.ChangeState(PLAYERSTATE.BOUNSHURT);
                            }
                            break;

                        case ATTACKTYPE.STIFF:
                            {
                                playerRigidbody.velocity = new Vector3(0, 0.5f, 0);
                                state_Machine.ChangeState(PLAYERSTATE.HURT);
                            }
                            break;

                        case ATTACKTYPE.FEAR:
                            {
                                if (playerInformation.Fear)
                                {
                                    StopCoroutine(FearDuration());
                                    playerInformation.Fear = false;
                                }

                                StartCoroutine(FearDuration());

                                playerRigidbody.MovePosition(transform.position + new Vector3(0, 0.2f, 0));
                                playerRigidbody.velocity = (direction * 15.0f) + new Vector3(0, 7.0f, 0);
                                state_Machine.ChangeState(PLAYERSTATE.AIRBORNE);
                            }
                            break;

                        case ATTACKTYPE.HEAL:
                            {
                                StartCoroutine(HellDuration());
                            }
                            break;
                    }
                }
            }
        }

        public void CameraChange()
        {
            if (photonView.IsMine)
            {
                CamManager.CameraChange();
            }
        }     

        //트루러브에서만 사용중인 함수라서 어떻게 해결할지 고민해야한다.
        public void ChangeState(PLAYERSTATE type)
        {
            if (photonView.IsMine)
            {
                state_Machine.ChangeState(type);
            }
        }

        //상대방을 처치할 시 호출
        public void SmashTrigger()
        {
            if (photonView.IsMine)
            {
                //캐릭터가 멈추기전 자신의 Velocity를 저장
                PreVelocity = playerRigidbody.velocity;

                //Player 피격당하지 않게 변경
                gameObject.layer = LayerMask.NameToLayer("Smash");
                playerRigidbody.isKinematic = true;

                //업데이트 정지
                IsUpdate = false;

                //카메라 타겟 자신으로 변경
                CameraTarget.transform.position = transform.position;

                //Smash카메라로 전환하면서 화면 연출
                CameraChange();

                //에니메이션 정지
                GameManager.Instance.AnimationPlay(photonView.ViewID,false);

                //Smash시간을 재고 일정시간 이후 종료해줄 코루틴실행
                StopCoroutine("SetInitialization");
                StartCoroutine(SetInitialization());
            }
        }

        IEnumerator SetInitialization()
        {
            yield return new WaitForSeconds(0.5f);

            //애니메이션 가능 재활성화 
            GameManager.Instance.AnimationPlay(photonView.ViewID, true);

            //물리작용 활성화
            playerRigidbody.isKinematic = false;

            //카메라 다시 되돌리기
            CameraChange();

            //Layer다시 되돌리기
            gameObject.layer = LayerMask.NameToLayer("Player");

            //업데이트 재가동
            IsUpdate = true;

            //저장한 Velocity값을 대입하여 자연스럽게 연출
            playerRigidbody.velocity = PreVelocity;
        }

        //공포
        IEnumerator FearDuration()
        {
            GameManager.Instance.ConditionEffectOn(ATTACKTYPE.FEAR, true, ID);
            playerInformation.Fear = true;
            yield return new WaitForSeconds(FearTime);

            GameManager.Instance.ConditionEffectOn(ATTACKTYPE.FEAR, false, ID);
            playerInformation.Fear = false;
        }

        //저주
        IEnumerator CurceDuration()
        {
            playerInformation.CurRunSpeed *= 0.5f;
            playerInformation.CurJumpDistance *= 0.5f;
            playerInformation.Curce = true;
            GameManager.Instance.ConditionEffectOn(ATTACKTYPE.CURSE, true, ID);

            yield return new WaitForSeconds(CurceTime);

            playerInformation.CurRunSpeed = playerInformation.RunSpeed;
            playerInformation.CurJumpDistance = playerInformation.JumpDistance;
            playerInformation.Curce = false;
            GameManager.Instance.ConditionEffectOn(ATTACKTYPE.CURSE, false, ID);
        }

        //회복
        IEnumerator HellDuration()
        {
            GameManager.Instance.ConditionEffectOn(ATTACKTYPE.HEAL, true, ID);

            yield return new WaitForSeconds(0.45f);

            GameManager.Instance.ConditionEffectOn(ATTACKTYPE.HEAL, false, ID);
        }

        //동기화가 필요한 사항들은 IsMine객체에서 다른 클라이언트에 있는 객체들에게 값들을 전달한다.
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                //위치동기화
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
                stream.SendNext(GetComponent<Rigidbody>().velocity);

                //스태미나
                stream.SendNext(StaminaUI.enabled);
                stream.SendNext(StaminaUI.fillAmount);
                stream.SendNext(playerInformation.Curce);
                stream.SendNext(playerInformation.UltimateOn);
            }
            else
            {
                networkPosition = (Vector3)stream.ReceiveNext();
                networkRotation = (Quaternion)stream.ReceiveNext();
                networkVelocity = (Vector3)stream.ReceiveNext();

                //스태미나
                StaminaUI.enabled = (bool)stream.ReceiveNext();
                StaminaUI.fillAmount = Mathf.MoveTowards((float)stream.ReceiveNext(), StaminaUI.fillAmount, Time.deltaTime);
                playerInformation.Curce = (bool)stream.ReceiveNext();
                playerInformation.UltimateOn = (bool)stream.ReceiveNext();
            }
        }
        public void Dead(bool b)
        {
            //처치당하면 캐릭터 모델 및 UI들을 비활성화
            //반대로 살아나는 경우 활성화
            transform.GetChild(0).gameObject.SetActive(b);
            playerCanvas.enabled = b;
            GroundPos.gameObject.SetActive(b);
            playerRigidbody.useGravity = false;

            if (photonView.IsMine)
            {
                LookCheck.SetActive(false);
            }
        }

        public void Fall()
        {
            if(photonView.IsMine)
            {
                //추락하는 이펙트 실행 후 DeadState돌입
                GameManager.Instance.Dead(false, photonView.ViewID);
                playerInformation.Cur_HP = 0;
                GameManager.Instance.HP(playerInformation.Cur_HP, ID);
                fallDead = true;
                state_Machine.ChangeState(PLAYERSTATE.DEAD);
            }
        }

        IEnumerator DodgeDelay()
        {
            dodge_UI.UseDodge();
            isDodge = false;

            yield return new WaitForSeconds(DodgeCoolTime);

            isDodge = true;
        }

        //팀 구분
        public void TemmDivision(string tag)
        {
            if (!CompareTag(tag))
            {
                ChangeLayersRecursively(transform.GetChild(0), "Red");

                GroundScript.ColorChange();

                HP_Bar.sprite = Enemy_HP_Bar_Sprite;
            }
        }

        //자식 레이어까지 바꾸는 함수
        public static void ChangeLayersRecursively(Transform trans, string name)
        {
            trans.gameObject.layer = LayerMask.NameToLayer(name);

            foreach (Transform child in trans)
            {
                ChangeLayersRecursively(child, name);
            }
        }

        public void UseSkill()
        {
            Skill_UI.UseSkill(playerInformation.Skill_Time);
            playerInformation.SkillOn = false;      
            StartCoroutine(SkillCootime());
        }

        IEnumerator SkillCootime()
        {
            yield return new WaitForSeconds(playerInformation.Skill_Time);
            playerInformation.SkillOn = true;
        }

        public void UseUtimate()
        {
            playerInformation.Cur_UltGage = 0;
            playerInformation.UltimateOn = false;
            Ultimate_UI.UseUtimate();
        }

        public void UtimateGageUp(int gage)
        {
            if (IsMine)
            {
                if (!playerInformation.UltimateOn)
                {
                    if (playerInformation.Cur_UltGage + gage >= playerInformation.UltGage_Max)
                    {
                        playerInformation.Cur_UltGage = 0;
                        playerInformation.UltimateOn = true;
                    }
                    else
                    {
                        playerInformation.Cur_UltGage += gage;
                    }

                    Ultimate_UI.UtimateGageUp(gage);
                }
            }
        }

        //개발자용 함수 지정한 State로 즉시 전환
        public void WantStateChnage()
        {
            state_Machine.ChangeState(WantState);
        }
        
        public void LineSet()
        {
            GroundScript.LineSet();
        }

        //초기상태로 초기화
        void ReSet()
        {
            playerAnimator.StopPlayback();
            playerRigidbody.isKinematic = false;
            playerRigidbody.useGravity = true;
        }

        //자신의 상태에 따라서 활성화 되는 Effect호출 함수
        public void ConditionEffectOn(ATTACKTYPE condition, bool on)
        {
            if (on && (CurEffect != null))
            {
                CurEffect.SetActive(false);
            }

            switch (condition)
            {
                case ATTACKTYPE.CURSE:
                    CurceEffect.SetActive(on);
                    CurEffect = CurceEffect;
                    break;
                case ATTACKTYPE.STUN:
                    StunEffect.SetActive(on);
                    CurEffect = StunEffect;
                    break;
                case ATTACKTYPE.FEAR:
                    FearEffect.SetActive(on);
                    CurEffect = FearEffect;
                    break;
                case ATTACKTYPE.HEAL:
                    HillEffect.SetActive(on);
                    CurEffect = HillEffect;
                    break;
            }
        }

        //던지는 타이밍의 폭탄을 없애는 용도로 만듬
        public void ThrowBomb()
        {
            Boom.SetActive(false);
            GetBoom = false;
        }

        public void Heal(int h)
        {
            if (IsMine)
            {
                StartCoroutine(HellDuration());

                //Hp회복
                if (h == 0)
                {
                    if (playerInformation.Cur_HP + 1500.0f >= playerInformation.HP_Max)
                    {
                        playerInformation.Cur_HP = playerInformation.HP_Max;
                        HP_Bar.fillAmount = playerInformation.Cur_HP / playerInformation.HP_Max;
                        hp_Value.SetHP_Value((int)playerInformation.HP_Max);
                    }
                    else
                    {
                        playerInformation.Cur_HP += 1500.0f;
                        HP_Bar.fillAmount = playerInformation.Cur_HP / playerInformation.HP_Max;
                        hp_Value.SetHP_Value((int)playerInformation.Cur_HP);
                    }

                    GameManager.Instance.HP(playerInformation.Cur_HP, ID);
                }
                //UltGage 회복
                else
                {
                    UtimateGageUp(40);
                }

                ItemTrigger = false;
            }
        }

        public void PlayHitSound(int number)
        {
            if(number != -1)
            {
                myAudio.Play(Juhyung.SoundManager.Instance.HitSound[number]);
            }
        }

    }
}

