using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Junpyo
{
    public class HP_Value : MonoBehaviour
    {
        [SerializeField] Sprite[] NumberImage;
        [SerializeField] Image[] Hp_Value_Image;

        public void SetHP_Value(int hp)
        {
            if(hp < 0)              
            {
                hp = 0;
            }

            for (int i = 0; i < 4; ++i)
            {
                if (hp != 0)
                {
                    Hp_Value_Image[i].sprite = NumberImage[hp % 10];
                }
                else
                {
                    Hp_Value_Image[i].sprite = NumberImage[10];
                }

                hp /= 10;
            }
        }
    }
}
