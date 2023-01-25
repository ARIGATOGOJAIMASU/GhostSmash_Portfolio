using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Junpyo
{
    public class SkillUI : MonoBehaviour
    {
        [SerializeField] private Image SkillImage;
        [SerializeField] private Text SkillText;
        float Timer;

        public void UseSkill(float cooltime)
        {
            Timer = cooltime;
            StartCoroutine(SkillCoolTimer());
        }

        IEnumerator SkillCoolTimer()
        {
            SkillImage.enabled = false;
            SkillText.enabled = true;

            while (true)
            {
                //0이 되기전까지 유지
                if (Timer - Time.deltaTime > 1)
                {
                    Timer -= Time.deltaTime;
                    SkillText.text = ((int)Timer).ToString();
                }
                else
                {
                    Timer -= Time.deltaTime;
                    SkillText.text = Timer.ToString();
                }

                //코루틴 나가기
                if (Timer - Time.deltaTime < 0)
                {
                    Timer = 0;
                    SkillImage.enabled = true;
                    SkillText.enabled = false;
                    yield break;
                }

                yield return null;
            }
        }
    }
}
