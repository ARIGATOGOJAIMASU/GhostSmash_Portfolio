using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Junpyo
{
    public class UltimateUI : MonoBehaviour
    {
        [SerializeField] private Image GageImage;
        [SerializeField] private Image AttackImage;
        [HideInInspector] public float CurGage;
        [HideInInspector] public float MaxGage;

        public void UseUtimate()
        {
            CurGage = 0;
            GageImage.color = Color.white;
            AttackImage.color = new Vector4(0, 0, 0, 120.0f / 255.0f);
        }

        public void UtimateGageUp(int gage)
        {
            CurGage += gage;

            if (CurGage < MaxGage)
            {
                GageImage.fillAmount = CurGage / MaxGage;
            }
            else
            {
                GageImage.fillAmount = 1.0f;
                GageImage.color = new Color(1f, 0.498f, 0f);
                AttackImage.color = new Color(1f, 0.498f, 0f);
            }
        }
    }
}

