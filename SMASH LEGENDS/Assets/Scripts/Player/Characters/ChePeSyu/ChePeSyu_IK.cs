using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class ChePeSyu_IK : Ik_Controller
    {
        protected override void JumpSkillIK(bool Ison)
        {
            if (Ison)
            {
                RightdistanceRange = 1.0f;
                HeaddistanceRange = 1.0f;
            }
            else
            {
                RightdistanceRange = 0.0f;
                HeaddistanceRange = 0.0f;
            }
        }
    }
}
