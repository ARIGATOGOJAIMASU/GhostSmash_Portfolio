using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Juhyung
{
    public class LoginManager : MonoBehaviourPun
    {
        [SerializeField] private InputField NickName_InputField;

        private string NickName;

        private void Awake()
        {
            Screen.SetResolution(1920, 1080, false);
        }
        private void Start()
        {
            // GameVersion
            PhotonNetwork.GameVersion = "1.14.1";

            // Master Server Connect
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        bool SetIDPass() // Login ID, PW Blank Check
        {
            NickName = NickName_InputField.text.Trim();

            if (NickName == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void OnClickLoginButton() // Send Order, ID, PW to DataBase Script
        {
            SoundManager.Instance.Play("Sounds/SoundSource/00. Default/SFX_Button_Default");

            if (!SetIDPass())
            {
                print("NickName 비어있음.");
                return;
            }

            PhotonNetwork.NickName = NickName_InputField.text;


            PhotonNetwork.LoadLevel("01_0. Loading_Scene");
        }
    }
}
