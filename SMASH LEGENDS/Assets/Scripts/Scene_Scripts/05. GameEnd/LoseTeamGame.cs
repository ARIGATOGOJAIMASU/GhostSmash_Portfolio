using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Juhyung
{
    public class LoseTeamGame : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Text Player_Name_Text;

        [SerializeField] public List<GameObject> avatars;
        private GameObject Avatar;
        private void Awake()
        {
            if(PhotonNetwork.CurrentRoom != null)
            {
                PhotonNetwork.LeaveRoom();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = 1;

            Player_Name_Text.text = PhotonNetwork.NickName;
            SetChar();
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void CreatePlayerPrefab(int _idx)
        {
            if (GameObject.Find("Avatar") != null)
            {
                Destroy(Avatar);
            }

            Avatar = Instantiate(avatars[_idx]);

            if (GameManager.Instance.e_SetChar != Junpyo.CHARACTERNAME.PENUKUE)
            {
                Avatar.transform.position = new Vector3(0.0f, 0.6f, -8f);
            }
            else
            {
                Avatar.transform.position = new Vector3(0.0f, 0.8f, -8f);
            }

            Avatar.transform.rotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);

            // Victory Animation

            Avatar.name = "Avatar";
        }
        private void SetChar()
        {
            switch (GameManager.Instance.e_SetChar)
            {
                case Junpyo.CHARACTERNAME.GANGNIM:
                    CreatePlayerPrefab((int)Junpyo.CHARACTERNAME.GANGNIM);

                    break;

                case Junpyo.CHARACTERNAME.CHEPESYU:
                    CreatePlayerPrefab((int)Junpyo.CHARACTERNAME.CHEPESYU);

                    break;

                case Junpyo.CHARACTERNAME.PENUKUE:
                    CreatePlayerPrefab((int)Junpyo.CHARACTERNAME.PENUKUE);

                    break;

                case Junpyo.CHARACTERNAME.TRUELOVE:
                    CreatePlayerPrefab((int)Junpyo.CHARACTERNAME.TRUELOVE);

                    break;

                case Junpyo.CHARACTERNAME.DUSEONIN:
                    CreatePlayerPrefab((int)Junpyo.CHARACTERNAME.DUSEONIN);

                    break;

                case Junpyo.CHARACTERNAME.PATAL:
                    CreatePlayerPrefab((int)Junpyo.CHARACTERNAME.PATAL);

                    break;
            }
        }

        public void OnClickExitBtn()
        {
            SoundManager.Instance.Play("SoundSource/00. Default/SFX_Button_Default");

            for (int i = 0; i < GameManager.Instance.Players.Count; ++i)
            {
                Destroy(GameManager.Instance.Players[i]);
                GameManager.Instance.PlayersScript[i] = null;
            }

            GameManager.Instance.Players.Clear();
            GameManager.Instance.PlayersScript.Clear();

            switch (GameManager.Instance.MainLobby_BGM)
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
