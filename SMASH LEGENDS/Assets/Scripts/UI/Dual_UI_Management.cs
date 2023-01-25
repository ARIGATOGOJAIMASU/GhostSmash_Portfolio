using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Junpyo
{
    public class Dual_UI_Management : UIManagement
    {
        [SerializeField] Juhyung.DuelLogic logic;

        protected override void SetUI()
        {
            Sprite sprite = logic.RedTeamScoreFill;
            logic.RedTeamScoreFill = logic.BlueTeamScoreFill;
            logic.BlueTeamScoreFill = sprite;
        }
    }
}
