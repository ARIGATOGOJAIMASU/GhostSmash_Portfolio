using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wooseok;

namespace Junpyo
{
    public class Duseonin_Skill : PlayerSkill
    {
        [SerializeField]
        Skill JumpAttack2;
        [SerializeField]
        Skill JumpAttack3;

        public GameObject MyJumpAttack2;
        public GameObject MyJumpAttack3;

        [SerializeField]
        private float BackRebound;

        [SerializeField]
        private float UpRebound;

        public override void Start()
        {
            base.Start();
            if (JumpAttack2 != null)
            {
                MyJumpAttack2 = Instantiate(JumpAttack2.gameObject, this.transform);
                MyJumpAttack2.GetComponent<Wooseok.Skill>().SetParent(this.gameObject);
                if (MyJumpAttack2.GetComponent<Wooseok.Skill>().FollowUpSkill != null)
                {
                    MyJumpAttack2.GetComponent<Wooseok.Skill>().FollowUpSkill.SetParent(this.gameObject);
                }
                MyJumpAttack2.SetActive(false);
            }
            if (JumpAttack3 != null)
            {
                MyJumpAttack3 = Instantiate(JumpAttack3.gameObject, this.transform);
                MyJumpAttack3.GetComponent<Wooseok.Skill>().SetParent(this.gameObject);
                if (MyJumpAttack3.GetComponent<Wooseok.Skill>().FollowUpSkill != null)
                {
                    MyJumpAttack3.GetComponent<Wooseok.Skill>().FollowUpSkill.SetParent(this.gameObject);
                }
                MyJumpAttack3.SetActive(false);
            }
        }

        public void DuseoninJumpAttack()
        {
            playerRig.velocity = Vector3.zero;
            playerRig.velocity = -(playertransform.forward * BackRebound) + (playertransform.up * UpRebound);
        }

        public override void AttackInst(int type, bool On)
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
                case 97:
                    usejumpatk2(On);
                    break;
                case 98:
                    usejumpatk3(On);
                    break;
                case (int)SKILLTYPE.BOMB:
                    usebomb(On);
                    break;
            }
        }

        protected virtual void usejumpatk2(bool On)
        {
            MyJumpAttack2.SetActive(On);

            if (On)
            {
                MyJumpAttack2.GetComponent<Wooseok.Skill>().Restart();
            }
        }

        protected virtual void usejumpatk3(bool On)
        {
            MyJumpAttack3.SetActive(On);

            if (On)
            {
                MyJumpAttack3.GetComponent<Wooseok.Skill>().Restart();
            }
        }
    }
}
