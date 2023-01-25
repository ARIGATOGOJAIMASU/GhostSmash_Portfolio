using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Junpyo
{
    public class Dodge_UI : MonoBehaviour
    {
        [SerializeField] private Text OKText;
        [SerializeField] private Text TimerText;
        private float Timer = 30.0f;

        public void UseDodge()
        {
            OKText.enabled = false;
            TimerText.enabled = true;

            StartCoroutine(DodgeDelay());
        }
        
        IEnumerator DodgeDelay()
        {
            while(true)
            {
                if(Timer - Time.deltaTime > 0.0f)
                {
                    Timer -= Time.deltaTime;
                    TimerText.text = Timer.ToString();
                }
                //타이머가 끝나고 나서
                else
                {
                    Timer = 30.0f;
                    OKText.enabled = true;
                    TimerText.enabled = false;

                    yield break;
                }

                yield return null;
            }
        }
    }
}
