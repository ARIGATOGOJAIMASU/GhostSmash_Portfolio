using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wooseok
{
    public class Skill_Area : Skill
    {
        [SerializeField]
        float HitInterval;
        float hittimer;

        bool hit;

        Skill_Area(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }

        public override void FollowUp()
        {
            
        }

        public override void Restart()
        {
            timer = 0f;
            curhit = 0;
            slappedtarget.Clear();
            Start();
            SetCollider(true);
        }

        public override void SkillEffectOnEnter(GameObject otherobj)
        {
        }

        public override void SkillEffectOnExit(GameObject otherobj)
        {
        }

        public override void SkillEffectOnStay(GameObject otherobj)
        {
            if (otherobj.CompareTag("Player"))
            {
                Debug.Log("태그 플레이어인 무언가랑 부딪힘");
            }
            if (
                otherobj.CompareTag("Player")
                && ParentPlayer != otherobj
                && hittimer > HitInterval
                )
            {
                //otherobj.gameObject.GetComponent<Player>().HP -= 100;
                //slappedtarget.Add(otherobj);
                Debug.Log("장판기 타격!");
                hit = true;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            hit = false;
        }

        // Update is called once per frame
        void Update()
        {
            hittimer += Time.deltaTime;

            if(hit)
            {
                hittimer = 0f;
                hit = false;
            }
            
        }

    }
}