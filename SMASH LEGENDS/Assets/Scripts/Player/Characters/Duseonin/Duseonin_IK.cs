using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class Duseonin_IK : Ik_Controller
    {
        protected override void HangAttack(bool on)
        {
            if(on)
            {
                HeaddistanceRange = 1.0f;
            }
            else
            {
                HeaddistanceRange = 0.0f;
            }
        }

        protected override void StandUpAttack(bool on)
        {
            if (on)
            {
                HeaddistanceRange = 1.0f;
            }
            else
            {
                HeaddistanceRange = 0.0f;
            }
        }

        protected override void JumpAttackIK(bool on)
        {
            if (on)
            {
                //Head
                HeadlookObj.localPosition = new Vector3(0, 0.5f, 1.5f);
                HeaddistanceRange = 1.0f;
            }
            else
            {
                //Head
                HeadlookObj.localPosition = new Vector3(0, 1.2f, 1.5f);
                HeaddistanceRange = 0.0f;
            }
        }

        protected override void HnagIK(bool on)
        {
            if (on)
            {
                RightdistanceRange = 1.0f;
                LeftdistanceRange = 1.0f;
            }
            else
            {
                RightdistanceRange = 0.0f;
                LeftdistanceRange = 0.0f;
            }
        }
    }
}
