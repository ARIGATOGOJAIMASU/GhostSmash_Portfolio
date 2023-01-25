using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Wooseok;

namespace Junpyo
{
    public enum SKILLTYPE {BASE_1, BASE_2, BASE_3, JUMPATTACK, SKILL, JUMPSKILL, ULTIMATE, JUMPULTIMATE, HANGATTACK, STANDUPSKILL, BOMB, STANDUPATTACK, CURSKILL }
    public class PlayerSkill : MonoBehaviourPun
    {
        //Player 관련변수
        [SerializeField] protected Transform playertransform;
        [SerializeField] protected Rigidbody playerRig;
        [SerializeField] protected PlayerController_FSM playerController;

        //Sword 관련 변수
        [SerializeField] public List<GameObject> SkillCombos;

        [SerializeField] protected float range;

        [SerializeField] Transform Sword;

        [SerializeField] private Wooseok.Skill Base_1;
        [SerializeField] private Wooseok.Skill Base_2;
        [SerializeField] private Wooseok.Skill Base_3;
        [SerializeField] private Wooseok.Skill JumpAttack;
        [SerializeField] private Wooseok.Skill HangAttack;
        [SerializeField] private Wooseok.Skill Skill;
        [SerializeField] private Wooseok.Skill JumpSkill;
        [SerializeField] private Wooseok.Skill Ultimate;
        [SerializeField] private Wooseok.Skill StandUpAttack;
        [SerializeField] private Wooseok.Skill Bomb;


        [HideInInspector] public GameObject myBase_1;
        [HideInInspector] public GameObject myBase_2;
        [HideInInspector] public GameObject myBase_3;
        [HideInInspector] public GameObject myJumpAttack;
        [HideInInspector] public GameObject myHangAttack;
        [HideInInspector] public GameObject mySkill;
        [HideInInspector] public GameObject myJumpSkill;
        [HideInInspector] public GameObject myUltimate;
        [HideInInspector] public GameObject myStandUpAttack;
        [HideInInspector] public GameObject myBomb;

        [HideInInspector] public GameObject CurSkill;
        /*public GameObject UsingSkill;
        public GameObject coffin;*/

        //--------------------------------------------------------------------------------------BaseAttack---------------------------------------------------------------------------------------
        public virtual void Start()
        {
            //mySkill.GetComponent<Skill>();

            if (Base_1 != null)
            {
                myBase_1 = Instantiate(Base_1.gameObject, this.transform);
                myBase_1.GetComponent<Wooseok.Skill>().SetParent(this.gameObject);
                if (myBase_1.GetComponent<Wooseok.Skill>().FollowUpSkill != null)
                {
                    myBase_1.GetComponent<Wooseok.Skill>().FollowUpSkill.SetParent(this.gameObject);
                }
                myBase_1.SetActive(false);
            }

            if (Base_2 != null)
            {
                myBase_2 = Instantiate(Base_2.gameObject, this.transform);
                myBase_2.GetComponent<Wooseok.Skill>().SetParent(this.gameObject);
                if (myBase_2.GetComponent<Wooseok.Skill>().FollowUpSkill != null)
                {
                    myBase_2.GetComponent<Wooseok.Skill>().FollowUpSkill.SetParent(this.gameObject);
                }
                myBase_2.SetActive(false);
            }

            if (Base_3 != null)
            {
                myBase_3 = Instantiate(Base_3.gameObject, this.transform);
                myBase_3.GetComponent<Wooseok.Skill>().SetParent(this.gameObject);
                if (myBase_3.GetComponent<Wooseok.Skill>().FollowUpSkill != null)
                {
                    myBase_3.GetComponent<Wooseok.Skill>().FollowUpSkill.SetParent(this.gameObject);
                }
                myBase_3.SetActive(false);
            }

            if (JumpAttack != null)
            {
                myJumpAttack = Instantiate(JumpAttack.gameObject, this.transform);
                myJumpAttack.GetComponent<Wooseok.Skill>().SetParent(this.gameObject);
                if (myJumpAttack.GetComponent<Wooseok.Skill>().FollowUpSkill != null)
                {
                    myJumpAttack.GetComponent<Wooseok.Skill>().FollowUpSkill.SetParent(this.gameObject);
                }
                myJumpAttack.SetActive(false);
            }

            if (JumpSkill != null)
            {
                myJumpSkill = Instantiate(JumpSkill.gameObject, this.gameObject.transform.position + 1.0f * this.gameObject.transform.forward + 1f * this.gameObject.transform.up, this.gameObject.transform.rotation, this.transform);
                myJumpSkill.GetComponent<Wooseok.Skill>().SetParent(this.gameObject);
                if (myJumpSkill.GetComponent<Wooseok.Skill>().FollowUpSkill != null)
                {
                    myJumpSkill.GetComponent<Wooseok.Skill>().FollowUpSkill.SetParent(this.gameObject);
                }
                myJumpSkill.SetActive(false);
            }

            if (HangAttack != null)
            {
                myHangAttack = Instantiate(HangAttack.gameObject, this.transform);
                myHangAttack.GetComponent<Wooseok.Skill>().SetParent(this.gameObject);
                if (myHangAttack.GetComponent<Wooseok.Skill>().FollowUpSkill != null)
                {
                    myHangAttack.GetComponent<Wooseok.Skill>().FollowUpSkill.SetParent(this.gameObject);
                }
                myHangAttack.SetActive(false);
            }

            if (Skill != null)
            {
                mySkill = Instantiate(Skill.gameObject, this.gameObject.transform.position + 1.0f * this.gameObject.transform.forward + 1f * this.gameObject.transform.up, this.gameObject.transform.rotation, this.transform);
                mySkill.GetComponent<Wooseok.Skill>().SetParent(this.gameObject);
                if (mySkill.GetComponent<Wooseok.Skill>().FollowUpSkill != null)
                {
                    mySkill.GetComponent<Wooseok.Skill>().FollowUpSkill.SetParent(this.gameObject);
                }
                mySkill.SetActive(false);
            }

            if (Ultimate != null)
            {
                myUltimate = Instantiate(Ultimate.gameObject, this.transform);
                myUltimate.GetComponent<Wooseok.Skill>().SetParent(this.gameObject);
                if (myUltimate.GetComponent<Wooseok.Skill>().FollowUpSkill != null)
                {
                    myUltimate.GetComponent<Wooseok.Skill>().FollowUpSkill.SetParent(this.gameObject);
                }
                myUltimate.SetActive(false);
            }

            if (StandUpAttack != null)
            {
                myStandUpAttack = Instantiate(StandUpAttack.gameObject, this.transform);
                myStandUpAttack.GetComponent<Wooseok.Skill>().SetParent(this.gameObject);
                if (myStandUpAttack.GetComponent<Wooseok.Skill>().FollowUpSkill != null)
                {
                    myStandUpAttack.GetComponent<Wooseok.Skill>().FollowUpSkill.SetParent(this.gameObject);
                }
                myStandUpAttack.SetActive(false);
            }

            if(Bomb != null)
            {
                myBomb = Instantiate(Bomb.gameObject, this.transform);
                myBomb.GetComponent<Wooseok.Skill>().SetParent(this.gameObject);
                if (myBomb.GetComponent<Wooseok.Skill>().FollowUpSkill != null)
                {
                    myBomb.GetComponent<Wooseok.Skill>().FollowUpSkill.SetParent(this.gameObject);
                }
                myBomb.SetActive(false);
            }
        }

        public void FowardMove(float parameters)
        {
            playerRig.velocity = playertransform.forward * parameters;
        }

        public void UpMove(float parameters)
        {
            playerRig.velocity = playertransform.up * parameters;
        }

        public virtual void BaseAttackCombo1()
        {
            //방향변경 및 위치변경
            //transform.LookAt(transform.position + playerController.PlayerLook);
            //playerRig.velocity = playertransform.forward * 3f;
        }

        public virtual void BaseAttackCombo2()
        {
        }

        public virtual void BaseAttackCombo3_1()
        {
        }

        public virtual void BaseAttackCombo3_2()
        {
        }

        public virtual void SlideAttack()
        {
        }

        public virtual void JumpSkillPrepareAction()
        {

        }

        public virtual void HangAttackPrepare()
        {
            playerRig.velocity = playertransform.up * 6f;
        }

        public virtual void HangAttackAction()
        {
            playertransform.position = new Vector3(
                     playertransform.position.x + (playertransform.forward.x * 0.3f),
                     playertransform.position.y,
                     playertransform.position.z + (playertransform.forward.z * 0.3f));

            //playerRig.velocity = playertransform.forward * 3f;
        }

        public void AttackInstStart(int type)
        {
            if (playerController.IsMine)
            {
                if (PhotonNetwork.IsConnected)
                {
                    GameManager.Instance.Attack(type,true, photonView.ViewID);
                }
                else
                {
                    AttackInst(type, true);
                }
            }
        }

        public virtual void FollowUpSkill(int type)
        {
            switch (type)
            {
                case (int)SKILLTYPE.BASE_1:
                    myBase_1.GetComponent<Skill>().FollowUp();
                    break;

                case (int)SKILLTYPE.BASE_2:
                    myBase_2.GetComponent<Skill>().FollowUp();
                    break;

                case (int)SKILLTYPE.BASE_3:
                    myBase_3.GetComponent<Skill>().FollowUp();
                    break;

                case (int)SKILLTYPE.JUMPATTACK:
                    myJumpAttack.GetComponent<Skill>().FollowUp();
                    break;


                case (int)SKILLTYPE.SKILL:
                    mySkill.GetComponent<Skill>().FollowUp();
                    break;

                case (int)SKILLTYPE.JUMPSKILL:
                    myJumpSkill.GetComponent<Skill>().FollowUp();
                    break;

                case (int)SKILLTYPE.ULTIMATE:
                    myUltimate.GetComponent<Skill>().FollowUp();
                    break;

                case (int)SKILLTYPE.HANGATTACK:
                    myHangAttack.GetComponent<Skill>().FollowUp();
                    break;

                case (int)SKILLTYPE.STANDUPSKILL:
                    myStandUpAttack.GetComponent<Skill>().FollowUp();
                    break;
                case (int)SKILLTYPE.BOMB:
                    myBomb.GetComponent<Skill>().FollowUp();
                    break;
            }
        }
        public virtual void AttackInst(int type, bool On)
        {
            switch (type)
            {
                case (int)SKILLTYPE.BASE_1:
                    usebase1(On);
                    break;

                case (int)SKILLTYPE.BASE_2:
                    usebase2(On);
                    break;

                case (int)SKILLTYPE.BASE_3:
                    usebase3(On);
                    break;

                case (int)SKILLTYPE.JUMPATTACK:
                    usejumpatk(On);
                    break;

                case (int)SKILLTYPE.SKILL:
                    useskill(On);
                    break;

                case (int)SKILLTYPE.JUMPSKILL:
                    usejumpskill(On);
                    break;

                case (int)SKILLTYPE.ULTIMATE:
                    useult(On);
                    break;

                case (int)SKILLTYPE.HANGATTACK:
                    usehangattack(On);
                    break;

                case (int)SKILLTYPE.STANDUPATTACK:
                    usestandupattack(On);
                    break;

                case (int)SKILLTYPE.BOMB:
                    usebomb(On);
                    break;

                case (int)SKILLTYPE.CURSKILL:
                    CurSkill.SetActive(On);
                    break;
            }
        }

        public void AttackOff(int type)
        {
            AttackInst(type, false);
        }

        protected void usebase1(bool On)
        {
            myBase_1.SetActive(On);

            if (On)
            {
                myBase_1.GetComponent<Wooseok.Skill>().Restart();
                CurSkill = myBase_1;
            }
        }

        protected void usebase2(bool On)
        {
            myBase_2.SetActive(On);

            if (On)
            {
                myBase_2.GetComponent<Wooseok.Skill>().Restart();
                CurSkill = myBase_2;
            }
        }

        protected void usebase3(bool On)
        {
            myBase_3.SetActive(On);
            if (On)
            {
                myBase_3.GetComponent<Wooseok.Skill>().Restart();
                CurSkill = myBase_3;
            }
        }

        protected void usejumpatk(bool On)
        {
            myJumpAttack.SetActive(On);
            if (On)
            {
                myJumpAttack.GetComponent<Wooseok.Skill>().Restart();
                CurSkill = myJumpAttack;
            }
        }

        protected void useskill(bool On)
        {
            mySkill.SetActive(On);
            if (On)
            {
                mySkill.GetComponent<Wooseok.Skill>().Restart();
                CurSkill = mySkill;
            }
        }

        protected void usejumpskill(bool On)
        {
            myJumpSkill.SetActive(On);
            if (On)
            {
                myJumpSkill.GetComponent<Wooseok.Skill>().Restart();
                CurSkill = myJumpSkill;
            }
        }

        protected void useult(bool On)
        {
            myUltimate.SetActive(On);
            if (On)
            {
                myUltimate.GetComponent<Wooseok.Skill>().Restart();
                CurSkill = myUltimate;
            }
        }

        protected void usehangattack(bool On)
        {
            myHangAttack.SetActive(On);
            if (On)
            {
                myHangAttack.GetComponent<Wooseok.Skill>().Restart();
                CurSkill = myHangAttack;
            }
        }

        protected void usestandupattack(bool On)
        {
            myStandUpAttack.SetActive(On);
            if (On)
            {
                myStandUpAttack.GetComponent<Wooseok.Skill>().Restart();
                CurSkill = myStandUpAttack;
            }
        }

        protected void usebomb(bool On)
        {
            myBomb.SetActive(On);
            if (On)
            {
                myBomb.GetComponent<Wooseok.Skill>().Restart();
                CurSkill = myBomb;
            }

        }

        protected void base1ADJ()
        {

        }

        protected void base2ADJ()
        {

        }

        protected void base3ADJ()
        {

        }

        protected void jumpatkADJ()
        {

        }
        protected void skillADJ()
        {

        }
        protected void jumpskillADJ()
        {

        }
        protected void ultADJ()
        {

        }
    }
}