using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Juhyung
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] // Player NickName
        public Text NickName;
        
        [SerializeField] // GameStart Button
        public Button GameStartBtn;

        [SerializeField] private GameObject Menu_UI;

        [SerializeField]
        public List<GameObject> avatars;

        private GameObject Avatar;

        // Using Struct MatchingOption
        MatchingOption MO = new MatchingOption();

        private void Awake()
        {
            
        }

        void Start()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameStartBtn.interactable = true; // Button Activate
                NickName.text = PhotonNetwork.NickName; // Set Player NickName
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings(); // Connect Master Server
            }

            SetCharacterAni();
        }

        private void Update()
        {
            if(!Menu_UI.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SoundManager.Instance.Play("SoundSource/00. Default/SFX_Button_Default");
                    Menu_UI.SetActive(true);
                }
            }
        }

        public void OnClickSelectModeBtn() // LoadScene "SelectMode_Scene"
        {
            SoundManager.Instance.Play("SoundSource/00. Default/SFX_Button_Default");
            PhotonNetwork.LoadLevel("02_2. SelectMode_Scene");
        }

        public void OnClickSelectCharacterBtn()
        {
            SoundManager.Instance.Play("SoundSource/00. Default/SFX_Button_Default");
            PhotonNetwork.LoadLevel("02_1. SelectCharacter_Scene");
        }

        public void OnClickGameStartBtn()
        {
            SoundManager.Instance.Play("SoundSource/00. Default/SFX_Button_GameStart");
            QuickMatchMaking(); // QuickMatching Logic
            PhotonNetwork.LoadLevel("01_1. Matching_Scene"); // LoadScene "Matching_Scene"
        }

        public void OnClickMenuBtn()
        {
            SoundManager.Instance.Play("SoundSource/00. Default/SFX_Button_Default");
            Menu_UI.SetActive(true);
        }

        public RoomOptions SetRoomOption()
        {
            RoomOptions roomOptions = new RoomOptions(); // Using RoomOptions
            roomOptions.MaxPlayers = MO.b_maxPlayer; // Set Game MaxPlayer Count
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() // Set CustomRoomProperties
            { 
                { "GameMode", MO.i_GameMode },  // Set Gamemode
                { "RoomName", MO.s_Name },      // Set RoomName
                { "GamePlayTime", MO.f_MaxTime }// Set Game Play Max Time
            };
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "GameMode" }; // Checking Gamemode

            return roomOptions;
        }

        void QuickMatchMaking()
        {
            if (PhotonNetwork.IsConnected)
            {
                switch (GameManager.Instance.i_GameMode)
                {
                    case 0:
                        MO.SetOption1(); // Gamemode 1

                        break;

                    case 1:
                        MO.SetOption2(); // Gamemode 2

                        break;

                    case 2:
                        MO.SetOption3(); // Gamemode 3

                        break;
                }

                RoomOptions roomOptions = SetRoomOption(); // Set RoomOptions

                PhotonNetwork.JoinRandomOrCreateRoom(expectedCustomRoomProperties: new ExitGames.Client.Photon.Hashtable() 
                { 
                    { "GameMode", GameManager.Instance.i_GameMode }  // Checking Gamemode
                }, expectedMaxPlayers: MO.b_maxPlayer, roomOptions: roomOptions); // Set Player max count and RoomOptions
            }
        }

        private void SetMainLobbyAvata(int _idx)
        {
            if (GameObject.Find("Avatar") != null)
            {
                Destroy(Avatar);
            }

            Avatar = Instantiate<GameObject>(avatars[_idx]);

            if (GameManager.Instance.e_SetChar != Junpyo.CHARACTERNAME.PENUKUE)
            {
                Avatar.transform.position = new Vector3(0.0f, 0.6f, -8f);
            }
            else
            {
                Avatar.transform.position = new Vector3(0.0f, 0.8f, -8f);
            }

            Avatar.transform.rotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);
            //Avatar.transform.localScale *= 6.0f;

            Avatar.AddComponent<CapsuleCollider>().isTrigger = true;
            Avatar.GetComponent<CapsuleCollider>().center = new Vector3(0.0f, 0.7f, 0.0f);
            Avatar.GetComponent<CapsuleCollider>().height = 2.0f;

            Avatar.AddComponent<ObjectRotate>();

            Avatar.name = "Avatar";
        }

        void SetCharacterAni()
        {
            switch (GameManager.Instance.e_SetChar)
            {
                case Junpyo.CHARACTERNAME.GANGNIM:
                    SetMainLobbyAvata((int)Junpyo.CHARACTERNAME.GANGNIM);

                    break;

                case Junpyo.CHARACTERNAME.CHEPESYU:
                    SetMainLobbyAvata((int)Junpyo.CHARACTERNAME.CHEPESYU);

                    break;

                case Junpyo.CHARACTERNAME.PENUKUE:
                    SetMainLobbyAvata((int)Junpyo.CHARACTERNAME.PENUKUE);

                    break;

                case Junpyo.CHARACTERNAME.TRUELOVE:
                    SetMainLobbyAvata((int)Junpyo.CHARACTERNAME.TRUELOVE);

                    break;

                case Junpyo.CHARACTERNAME.DUSEONIN:
                    SetMainLobbyAvata((int)Junpyo.CHARACTERNAME.DUSEONIN);

                    break;

                case Junpyo.CHARACTERNAME.PATAL:
                    SetMainLobbyAvata((int)Junpyo.CHARACTERNAME.PATAL);

                    break;
            }
        }
    }
}
