using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class Patal_Skill : PlayerSkill
    {
        public override void HangAttackPrepare()
        {
            playerRig.velocity = playertransform.up * 6f;
        }
    }
}
