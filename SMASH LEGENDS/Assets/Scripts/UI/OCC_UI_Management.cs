using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Junpyo
{
    public class OCC_UI_Management : UIManagement
    {
        [SerializeField] protected Sprite redScoreImage;
        [SerializeField] protected Sprite blueScoreImage;

        [SerializeField] protected Image redScoreUI;
        [SerializeField] protected Image blueScoreUI;

        [SerializeField] Juhyung.OccupationGage GageUI;

        protected override void SetUI()
        {
            redScoreUI.sprite = blueScoreImage;
            blueScoreUI.sprite = redScoreImage;

            GageUI.BlueGage.GetComponent<Image>().color = Color.red;
            GageUI.RedGage.GetComponent<Image>().color = Color.white;

            ColorChange(GageUI.Red1stGage_Fill, GageUI.Blue1stGage_Fill);

            SpriteChange(GageUI.Red2stGage_Fill, GageUI.Blue2stGage_Fill);

            ColorChange(GageUI.Red3stGage_Fill, GageUI.Blue3stGage_Fill);
            ColorChange(GageUI.RedKDGage_Fill, GageUI.BlueKDGage_Fill);

            Sprite RedImage = GageUI.RedScore_Fill;

            GageUI.RedScore_Fill = GageUI.BlueScore_Fill;

            GageUI.BlueScore_Fill = RedImage;

            ColorChange(GageUI.Red_MatchPoint.GetComponent<Image>(), GageUI.Blue_MatchPoint.GetComponent<Image>());
            SpriteChange(GageUI.Red_MatchOver.GetComponent<Image>(), GageUI.Blue_MatchOver.GetComponent<Image>());

            SpriteChange(GageUI.Red_MatchOverText, GageUI.Blue_MatchOverText);
            SpriteChange(GageUI.Red_MatchText, GageUI.Blue_MatchText);

            SpriteChange(GageUI.RedKDGageText, GageUI.BlueKDGageText);
            ColorChange(GageUI.RedBase_Fill, GageUI.BlueBase_Fill);
            ColorChange(GageUI.Base_Red.GetComponent<Image>(), GageUI.Base_Blue.GetComponent<Image>());
        }
    }
}
