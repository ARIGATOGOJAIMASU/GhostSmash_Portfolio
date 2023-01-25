using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Juhyung
{
    public class SelectBGMManager : MonoBehaviourPun
    {
        [SerializeField] private Dropdown SelectBGM;
        [SerializeField] private GameObject Arrow;

        private void Start()
        {
            SelectBGM.value = GameManager.Instance.MainLobby_BGM;
        }

        private void Update()
        {

        }

        public void OnValueChange()
        {
            GameManager.Instance.MainLobby_BGM = SelectBGM.value;

            SoundManager.Instance.Play("Sounds/SoundSource/00. Default/SFX_Button_Default");

            switch (SelectBGM.value)
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
        }
    }
}
