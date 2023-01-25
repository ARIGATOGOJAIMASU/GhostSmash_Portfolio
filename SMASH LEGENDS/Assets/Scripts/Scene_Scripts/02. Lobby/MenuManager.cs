using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Juhyung
{
    public class MenuManager : MonoBehaviourPun
    {
        [SerializeField] private Slider BGM_Volume;
        [SerializeField] private Slider SFX_Volume;

        [SerializeField] private Text BGM_Volume_Text;
        [SerializeField] private Text SFX_Volume_Text;

        [SerializeField] private GameObject Menu_UI;

        private void Start()
        {
            BGM_Volume.value = SoundManager.Instance.BGM_Volume;
            SFX_Volume.value = SoundManager.Instance.SFX_Volume;
        }
        private void Update()
        {
            SoundManager.Instance.BGM_Volume = BGM_Volume.value;
            SoundManager.Instance.SFX_Volume = SFX_Volume.value;

            BGM_Volume_Text.text = ((int)(BGM_Volume.value * 100.0f)).ToString();
            SFX_Volume_Text.text = ((int)(SFX_Volume.value * 100.0f)).ToString();

            if (Menu_UI.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SoundManager.Instance.Play("SoundSource/00. Default/SFX_Button_Default");
                    Menu_UI.SetActive(true);
                }
            }
        }

        public void OnClickExitBtn()
        {
            SoundManager.Instance.Play("Sounds/SoundSource/00. Default/SFX_Button_Default");

            Menu_UI.SetActive(false);
        }

        public void OnClickGameQuitBtn()
        {
            Application.Quit();
        }

        public void OnClickBGM1()
        {
            GameManager.Instance.MainLobby_BGM = 0;

            SoundManager.Instance.Play("Sounds/SoundSource/99. BGM/BGM_MainLobby", SoundManager.DEFINE.BGM);
        }

        public void OnClickBGM2()
        {
            GameManager.Instance.MainLobby_BGM = 1;

            SoundManager.Instance.Play("Sounds/SoundSource/99. BGM/BGM_TTD", SoundManager.DEFINE.BGM);
        }

        public void OnClickBGM3()
        {
            GameManager.Instance.MainLobby_BGM = 2;

            SoundManager.Instance.Play("Sounds/SoundSource/99. BGM/BGM_Duel", SoundManager.DEFINE.BGM);
        }
    }
}
