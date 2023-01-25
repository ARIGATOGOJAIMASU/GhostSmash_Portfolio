using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Juhyung
{
    public class MatchManager : MonoBehaviourPunCallbacks
    {
        private float t;
        [SerializeField] float Speed;

        [SerializeField] private List<GameObject> GameMode_Image;
        [SerializeField] private GameObject Matching_Clear;
        [SerializeField] private GameObject Progress_Circle;

        [SerializeField] private List<GameObject> EnterPlayer;

        void Awake()
        {
            // All Client get Event
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            SoundManager.Instance.Play("SoundSource/99. BGM/BGM_Matching", SoundManager.DEFINE.BGM);
            GameMode_Image[GameManager.Instance.i_GameMode].SetActive(true);
        }

        private void Update()
        {
            t += Time.deltaTime;
            Progress_Circle.transform.rotation = Quaternion.Euler(0, 0, t * Speed * -1);
            
            if (t * Speed <= -360)
            {
                t = 0;
            }
        }

        public void OnClickExitBtn() // LoadScene "MainLobby" and Leave Matching Room
        {
            SoundManager.Instance.Play("Sounds/SoundSource/00. Default/SFX_Button_Default");
            SoundManager.Instance.Play("Sounds/SoundSource/99. BGM/BGM_MainLobby", SoundManager.DEFINE.BGM);

            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.LoadLevel("02_0. MainLobby");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(OnEnterPlayer), RpcTarget.All);

                if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    photonView.RPC(nameof(MaxPlayer), RpcTarget.All);
                }
            }
        }

        [PunRPC]
        private void OnEnterPlayer()
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount <= EnterPlayer.Count)
            {
                for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
                {
                    EnterPlayer[i].SetActive(true);
                }
            }
        }

        [PunRPC]
        private void MaxPlayer()
        {
            Matching_Clear.SetActive(true);
        }
    }
}
