using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Junpyo
{
    public enum PLAYERSTATE { IDLE, RUN, BASEATTACK, JUMP, HURT, HANG, LAND, FALL, HANGATTACK, HEAVYHURT, JUMPATTACK, AI, AIRBORNE, GROUNDDOWN, ROLLING,
        DEAD, SMASH, DEADHIGHLIGHT, DEADFLY, REVIVAL, SKILL, JUMPSKILL, ULTIMATE, DODGE, STUN, CURCE, AIR, HANGATTACKPREPARE, BOUNSHURT, DROP, CHEPESYULTIMATE,
    ULTIMATEPREPARE, STANDUP, STANDUPATTACK, JUMPSKILLPREPARE, GANGNIM_JUMPSKILLSTATE, DUSEONIN_JUMPATTACK, TRUELOVE_JUMPATTACK, TRUELOVE_JUMPATTACKFAILL, TRUELOVE_JUMPATTACKSUCCESS,
    DODGEAIR, USE_JUMPSTATION, GETITEM, THROW, PLAYERSTATE_END}
    
    public abstract class State_Base
    {
        //Trnasform
        protected Transform Owner;
        protected Transform GroundPos;
        protected GameObject DustEffect;
        //Componenet
        protected Animator Owner_animator;
        protected Rigidbody Owner_rigidbody;

        //Script
        protected PlayerController_FSM Owner_Script;
        protected State_Machine state_Machine;

        //자신의 State를 가리킴
        protected PLAYERSTATE StateType;

        //바라보는 방향
        protected Vector3 Owner_Look;

        //키값으로 인해 향하는 방향
        protected Vector3 MoveDirection;

        //PlayerUI
        protected Canvas Owner_Canvas;
        protected Image HP_Bar;

        //PlayerIK
        public Ik_Controller OwnerIK;

        //PlayerPhotonviewID
        protected int Pv_ID;

        public PLAYERSTATE _StateType
        {
            get { return StateType; }
            set { StateType = value; }
        }
        //초기화
        public void Init(Transform owner, PLAYERSTATE type, Junpyo.State_Machine machine)
        {
            //Owner관련 변수들 할당
            Owner = owner;
            Owner_animator = owner.GetComponent<Animator>();
            Owner_rigidbody = owner.GetComponent<Rigidbody>();
            state_Machine = owner.GetComponent<PlayerController_FSM>().state_Machine;
            Owner_Script = owner.GetComponent<PlayerController_FSM>();
            Owner_Canvas = Owner_Script.playerCanvas;
            HP_Bar = Owner_Script.HP_Bar;
            OwnerIK = Owner_Script.IK_Controller;

            Pv_ID = Owner.GetComponent<PhotonView>().ViewID;

            //MainCamera 할당
            GroundPos = Owner_Script.GroundPos;
            StateType = type;
            Setting();
        }

        public abstract void Setting();

        //State처음 진입할 때
        public virtual void StateEnter() { }
        public virtual void Update() { }

        public virtual void FixedUpdate() { }
        //State가 바뀔 때
        public virtual void StateExit() { }
    }
}