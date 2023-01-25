using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Juhyung
{
    public class MatchLoadingManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] float Speed;

        [SerializeField] private List<GameObject> GameMode_Image;

        [SerializeField] private Text Loading_Num;

        private float t_Temp;
        void Awake()
        {
            // All Client get Event
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            GameMode_Image[GameManager.Instance.i_GameMode].SetActive(true);

            StartCoroutine(LoadingScene(GameManager.Instance.i_GameMode));
        }

        [PunRPC]
        private void PlayBGM(int gameMode)
        {
            switch(gameMode)
            {
                case 0:
                    SoundManager.Instance.Play("Sounds/SoundSource/99. BGM/BGM_Duel", SoundManager.DEFINE.BGM);

                    break;

                case 1:
                    SoundManager.Instance.Play("Sounds/SoundSource/99. BGM/BGM_TTD", SoundManager.DEFINE.BGM);

                    break;

                case 2:
                    SoundManager.Instance.Play("Sounds/SoundSource/99. BGM/BGM_Occupation", SoundManager.DEFINE.BGM);

                    break;
            }
        }

        IEnumerator LoadingScene(int gameMode)
        {
            yield return null;

            while(t_Temp <= 1)
            {
                t_Temp += Time.deltaTime / 4.0f;

                Loading_Num.text = ((int)(t_Temp * 100.0f)).ToString() + "%";
            }

            if(t_Temp >= 1)
            {
                Loading_Num.text = "100%";
            }

            if(PhotonNetwork.IsMasterClient)
            {
                switch (gameMode)
                {
                    case 0:
                        photonView.RPC(nameof(PlayBGM), RpcTarget.All, gameMode);
                        PhotonNetwork.LoadLevel("04_0. DuelScene");

                        break;

                    case 1:
                        photonView.RPC(nameof(PlayBGM), RpcTarget.All, gameMode);
                        PhotonNetwork.LoadLevel("04_1. TeamTouchDownScene");

                        break;

                    case 2:
                        photonView.RPC(nameof(PlayBGM), RpcTarget.All, gameMode);
                        PhotonNetwork.LoadLevel("04_2. OccupationScene");

                        break;
                }
            }
        }
    }
}
