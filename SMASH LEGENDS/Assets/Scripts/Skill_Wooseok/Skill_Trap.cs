using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{

    public class Skill_Trap : Skill
    {
        [SerializeField]
        private int damage;

        bool isdestroy;

        Skill_Trap(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }
        public override void SkillEffectOnEnter(GameObject otherobj)
        {
            if (
                otherobj.gameObject.CompareTag("Player") &&
                this.curhit < this.MaxHit &&
                this.MaxTargetNumber > this.slappedtarget.Count &&
                !GameObjectChecker(slappedtarget, otherobj)
                )
            {
                Debug.Log("함정 발동!");

                isdestroy = true;
                this.slappedtarget.Add(new Pair<GameObject, int>( otherobj, 1));
            }
        }

        public override void SkillEffectOnExit(GameObject otherobj)
        {
            if(
                otherobj.gameObject.CompareTag("Player") &&
                GameObjectChecker(slappedtarget, otherobj)
              )
            {
                this.curhit++;
                //otherobj.GetComponent<Player>().HP -= damage;
                Debug.Log("함정 탈출 발동!");
            }


        }

        public override void SkillEffectOnStay(GameObject otherobj)
        {
        }

        // Start is called before the first frame update
        void Start()
        {
            isdestroy = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void FixedUpdate()
        {
            if(isdestroy)
            {
                Destroy(this.gameObject);
            }
        }

        public override void FollowUp()
        {
            //
        }

        public override void Restart()
        {
            timer = 0f;
            curhit = 0;
            slappedtarget.Clear();
            Start();
        }
    }
}