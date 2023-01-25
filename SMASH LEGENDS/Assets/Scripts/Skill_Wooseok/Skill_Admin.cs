using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{
    public class Skill_Admin : Skill
    {
        public Skill_Admin(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }
        [SerializeField]
        List<Skill> Skills;

        int count;

        public override void FollowUp()
        {

            if (Skills.Count != 0)
            {
                Skills[count].gameObject.SetActive(true);
                Skills[count].Restart();
                count++;
                if(count >= Skills.Count)
                {
                    count = 0;
                }
            }
        }

        private void Start()
        {
            count = 0;
            foreach (Skill skill in Skills)
            {
                skill.ParentPlayer = this.ParentPlayer;
                skill.ParentScript = this.ParentScript;
            }
        }

        public override void Restart()
        {
            timer = 0f;
            curhit = 0;
            slappedtarget.Clear();

            if (!IshaveParent)
            {
                this.transform.position = ParentPlayer.transform.position + new Vector3(startvector.x * ParentPlayer.transform.forward.x,
                                                                                            startvector.y * ParentPlayer.transform.up.y,
                                                                                            startvector.x * ParentPlayer.transform.forward.z);
                this.transform.LookAt(this.transform.position + ParentPlayer.transform.forward);
            }
            else
            {
                this.transform.position = ParentPlayer.transform.position + new Vector3(startvector.x * transform.forward.x,
                                                                                            startvector.y * transform.up.y,
                                                                                            startvector.x * transform.forward.z);
                this.transform.rotation = ParentPlayer.transform.rotation;
            }
            Start();
            foreach (Skill skill in Skills)
            {
                skill.gameObject.SetActive(true);
                skill.Restart();
            }
            SetCollider(true);
        }

        public override void SkillEffectOnEnter(GameObject otherobj)
        {
            return;
        }

        public override void SkillEffectOnExit(GameObject otherobj)
        {
            return;
        }

        public override void SkillEffectOnStay(GameObject otherobj)
        {
            return;
        }
       
    }
}