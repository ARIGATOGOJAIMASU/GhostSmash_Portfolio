using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class Gangnim_IK : Ik_Controller
    {
        protected override void BaseAttackIK(bool on)
        {
            if (on)
            {
                LeftdistanceRange = 1.0f;
            }
            else
            {
                LeftdistanceRange = 0.0f;
            }
        }

        protected override void JumpAttackIK(bool on)
        {
            if (on)
            {
                LeftdistanceRange = 1.0f;
            }
            else
            {
                LeftdistanceRange = 0.0f;
            }
        }

        protected override void JumpSkillIK(bool on)
        {
            if (on)
            {
                LeftdistanceRange = 1.0f;
            }
            else
            {
                LeftdistanceRange = 0.0f;
            }
        }

        protected override void SkillIK(bool on)
        {
            if (on)
            {
                LeftdistanceRange = 1.0f;
            }
            else
            {
                LeftdistanceRange = 0.0f;
            }
        }

        protected override void DownIK(bool on)
        {
            if (on)
            {
                //RightrotationRange = 1.0f;
                SetRightHandRotation = new Vector3(-20, -100, 10);
            }
            else
            {
                //RightrotationRange = 0.0f;
                SetRightHandRotation = new Vector3(0, 0, 0);
            }
        }

        protected override void HnagIK(bool on)
        {
            if (on)
            {
                RightdistanceRange = 1.0f;

                LeftlookObj.localPosition = new Vector3(-0.068f, 0.046f, 0.107f);
                LeftdistanceRange = 1.0f;
            }
            else
            {
                RightdistanceRange = 0.0f;

                LeftlookObj.localPosition = new Vector3(0,0,0);
                LeftdistanceRange = 0.0f;
            }
        }

        protected override void Idle(bool on)
        {
            if (on)
            {
                RightrotationRange = 1.0f;
                SetRightHandRotation = new Vector3(10, -130, 0);
            }
            else
            {
                RightrotationRange = 0.0f;
                SetRightHandRotation = Vector3.zero;
            }
        }
    }
}
