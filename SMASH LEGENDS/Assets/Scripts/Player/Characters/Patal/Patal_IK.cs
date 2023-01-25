using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class Patal_IK : Ik_Controller
    {
        protected override void HnagIK(bool on)
        {
            if(on)
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