using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Junpyo
{
    public class TTD_UI_Management : UIManagement
    {
        [SerializeField] Juhyung.TTDLogic[] Ghost;
        protected override void SetUI()
        {
            foreach (Juhyung.TTDLogic ghost in Ghost)
            {
                ghost.ChangeColorUI();
            }

            for(int i = 0; i < Red_SpriteChange_UI.Length; ++i)
            {
                SpriteChange(Red_SpriteChange_UI[i], Blue_SpriteChange_UI[i]);
            }

            for (int i = 0; i < Red_ColorChange_UI.Length; ++i)
            {
                ColorChange(Red_ColorChange_UI[i], Blue_ColorChange_UI[i]);
            }
        }
    }
}
