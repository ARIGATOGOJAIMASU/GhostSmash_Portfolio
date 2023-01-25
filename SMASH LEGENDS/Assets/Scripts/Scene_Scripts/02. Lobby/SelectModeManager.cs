using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Juhyung
{
    public class SelectModeManager : MonoBehaviourPun
    {
        public void OnClickBackBtn()
        {
            SoundManager.Instance.Play("Sounds/SoundSource/00. Default/SFX_Button_Default");

            PhotonNetwork.LoadLevel("02_0. MainLobby");
        }

        public void OnClickDuelBtn()
        {
            SoundManager.Instance.Play("Sounds/SoundSource/00. Default/SFX_Button_Default");

            GameManager.Instance.i_GameMode = 0;
            PhotonNetwork.LoadLevel("02_0. MainLobby");
        }

        public void OnClickTTDBtn()
        {
            SoundManager.Instance.Play("Sounds/SoundSource/00. Default/SFX_Button_Default");

            GameManager.Instance.i_GameMode = 1;
            PhotonNetwork.LoadLevel("02_0. MainLobby");
        }

        public void OnClickOCCBtn()
        {
            SoundManager.Instance.Play("Sounds/SoundSource/00. Default/SFX_Button_Default");

            GameManager.Instance.i_GameMode = 2;
            PhotonNetwork.LoadLevel("02_0. MainLobby");
        }
    }
}
