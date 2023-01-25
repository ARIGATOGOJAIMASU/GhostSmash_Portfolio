using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Juhyung
{
    public class LoadingManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject Progress;
        [SerializeField] private GameObject g_Start;

        [SerializeField] private Image Progress_Bar;
        [SerializeField] private Image Progress_Circle;
        [SerializeField] private Text Progress_Text;
        [SerializeField] private Text Progress_Percent;

        [SerializeField] List<string> s_Progress__Text;

        [SerializeField] float Speed;
        private bool b_Start = false;
        private float t;
        private void Update()
        {
            if(!b_Start)
            {
                t += Time.deltaTime;
                Progress_Circle.transform.rotation = Quaternion.Euler(0, 0, t * Speed * -1);
                Progress_Bar.fillAmount += Time.deltaTime / 4.0f;
                Progress_Percent.text = ((int)(Progress_Bar.fillAmount * 100)).ToString() + "%";

                if(Progress_Bar.fillAmount >= 0.45f && Progress_Bar.fillAmount <= 0.60f)
                {
                    Progress_Text.text = s_Progress__Text[0];
                }
                else if(Progress_Bar.fillAmount >= 0.61f)
                {
                    Progress_Text.text = s_Progress__Text[1];
                }

                if (Progress_Bar.fillAmount >= 1.0f)
                {
                    Progress.SetActive(false);
                    g_Start.SetActive(true);
                    b_Start = true;
                }

                if (t * Speed <= -360)
                {
                    t = 0;
                }
            }
            else
            {
                if(Input.anyKeyDown)
                {
                    switch(GameManager.Instance.MainLobby_BGM)
                    {
                        case 0:
                            SoundManager.Instance.Play("Sounds/SoundSource/99. BGM/BGM_MainLobby", SoundManager.DEFINE.BGM);

                            break;

                        case 1:
                            SoundManager.Instance.Play("Sounds/SoundSource/99. BGM/BGM_TTD", SoundManager.DEFINE.BGM);

                            break;

                        case 2:
                            SoundManager.Instance.Play("Sounds/SoundSource/99. BGM/BGM_Duel", SoundManager.DEFINE.BGM);

                            break;
                    }

                    PhotonNetwork.LoadLevel("02_0. MainLobby");
                }
            }
        }
    }
}
