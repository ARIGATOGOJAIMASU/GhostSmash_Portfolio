using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Junpyo
{
    public class RevivalUI : MonoBehaviour
    {
        public Sprite[] CharacterImage;
        public Image MyImage;
        public Image EnemyImage;

        public void SetRevivalUI(CHARACTERNAME my, CHARACTERNAME enemy)
        {
            MyImage.sprite = CharacterImage[(int)my];
            EnemyImage.sprite = CharacterImage[(int)enemy];
        }
    }
}
