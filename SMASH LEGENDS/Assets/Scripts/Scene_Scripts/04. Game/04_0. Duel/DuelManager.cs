using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Juhyung
{
    public class DuelManager : Junpyo.GamLogicManager
    {
        private static DuelManager _instance;

        public static DuelManager Instance
        {
            get
            {
                // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(DuelManager)) as DuelManager;

                    if (_instance == null)
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
        }

        public int RedScore;
        public int BlueScore;

        private void Awake()
        {
            Singleton();
            CreatePlayer();
        }

        private void Update()
        {
            if (GameStart)
            {
                GameTimer();
            }

            if(RedScore == 3 || BlueScore == 3)
            {
                GameSet();
            }
        }

        private void Singleton()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected override void GameTimer()
        {
            if (GameTime <= 0.0f)
            {
                return; // GameEnd
            }

            GameTime -= Time.deltaTime;

            int i_Min = (int)GameTime / 60;
            int i_Tensec = (int)(GameTime - (i_Min * 60)) / 10;
            int i_Onesec = (int)(GameTime - (i_Min * 60) - (i_Tensec * 10));

            Min.sprite = i_Num[i_Min];
            TenSec.sprite = i_Num[i_Tensec];
            OneSec.sprite = i_Num[i_Onesec];
        }

        public override void GameSet()
        {
            PhotonNetwork.AutomaticallySyncScene = false;

            if (go.GetPhotonView().IsMine)
            {
                if (RedScore == 3)
                {
                    if (go.tag.Equals("Red"))
                    {
                        PhotonNetwork.LoadLevel("05_0. TeamVictoryScene");
                    }
                    else if (go.tag.Equals("Blue"))
                    {
                        PhotonNetwork.LoadLevel("05_1. TeamLoseScene");
                    }
                }
                else if (BlueScore == 3)
                {
                    if (go.tag.Equals("Blue"))
                    {
                        PhotonNetwork.LoadLevel("05_0. TeamVictoryScene");
                    }
                    else if (go.tag.Equals("Red"))
                    {
                        PhotonNetwork.LoadLevel("05_1. TeamLoseScene");
                    }
                }
            }
        }
    }
}
