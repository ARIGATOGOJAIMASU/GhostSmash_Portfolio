using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    class AiState : State_Base
    {
        //private bool IsHurt;
        private bool StartAttack = false;
        public override void Setting() { StateType = PLAYERSTATE.AI; }
        public override void StateEnter()
        {
            //기본 공격
            /* StartAttack = true;
             Owner_animator.SetBool("IsAttack", true);
             Owner_animator.SetTrigger("StartAttack");
             Owner.LookAt(Owner.position + new Vector3(-1, 0, 0));*/

            //트로러브 스킬
            CoroutineHelper.StartCoroutine(SkillAttack());
            Owner.LookAt(Owner.position + new Vector3(-1, 0, 0));
        }

        public override void Update()
        {
            //기본공격
          /*  if (StartAttack == false)
            {
                Owner_animator.SetTrigger("StartAttack");
            }
            if (Owner_animator.GetCurrentAnimatorStateInfo(0).IsName("AttackCombo1"))
            {
                Owner_animator.SetTrigger("ComboAttack");
            }
            else if (Owner_animator.GetCurrentAnimatorStateInfo(0).IsName("AttackCombo2"))
            {
                Owner_animator.SetTrigger("ComboAttack");
            }
            else if (Owner_animator.GetCurrentAnimatorStateInfo(0).IsName("AttackCombo3"))
            {
                StartAttack = false;
            }*/

            //트루러브 스킬


        }

        public override void StateExit()
        {

        }

        IEnumerator SkillAttack()
        {
            Owner_animator.SetTrigger("Skill");
            yield return new WaitForSeconds(3.0f);

            CoroutineHelper.StartCoroutine(SkillAttack());
        }
    }
}
