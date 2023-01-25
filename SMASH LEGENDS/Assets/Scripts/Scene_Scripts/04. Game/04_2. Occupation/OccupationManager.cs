using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Juhyung
{
    public class OccupationManager : Junpyo.GamLogicManager
    {
        private static OccupationManager _instance;

        public static OccupationManager Instance
        {
            get
            {
                // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(OccupationManager)) as OccupationManager;

                    if (_instance == null)
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
        }

        public int RedScore;
        public int BlueScore;
        private int MaxScore;

        public bool Red_Win = false;
        public bool Blue_Win = false;

        [SerializeField] private GameObject OccupationTrigger;                          // Occupation Range

        private void Awake()
        {
            Singleton();
            CreatePlayer();
        }

        private void Start()
        {
            StartCoroutine(SetOccupationDelay());
        }

        private void Update()
        {
            if (GameStart)
            {
                GameTimer();
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
                if(RedScore > BlueScore)
                {
                    Red_Win = true;
                    Blue_Win = false;
                }
                else if(RedScore < BlueScore)
                {
                    Red_Win = false;
                    Blue_Win = true;
                }

                Time.timeScale = 0;
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
                if (Red_Win)
                {
                    if (go.tag.Equals("Red"))
                    {
                        SoundManager.Instance.Play("SoundSource/99. BGM/BGM_Result", SoundManager.DEFINE.BGM);

                        PhotonNetwork.LoadLevel("05_0. TeamVictoryScene");
                    }
                    else if (go.tag.Equals("Blue"))
                    {
                        SoundManager.Instance.Play("SoundSource/99. BGM/BGM_Result", SoundManager.DEFINE.BGM);

                        PhotonNetwork.LoadLevel("05_1. TeamLoseScene");
                    }
                }
                else if (Blue_Win)
                {
                    if (go.tag.Equals("Blue"))
                    {
                        SoundManager.Instance.Play("SoundSource/99. BGM/BGM_Result", SoundManager.DEFINE.BGM);

                        PhotonNetwork.LoadLevel("05_0. TeamVictoryScene");
                    }
                    else if (go.tag.Equals("Red"))
                    {
                        SoundManager.Instance.Play("SoundSource/99. BGM/BGM_Result", SoundManager.DEFINE.BGM);

                        PhotonNetwork.LoadLevel("05_1. TeamLoseScene");
                    }
                }
            }
        }

        public void GameRe()
        {
            GameTime = 180.0f;
        }

        //Coroutine Funtion
        IEnumerator SetOccupationDelay()
        {
            yield return new WaitForSeconds(StartDelayTime);

            OccupationTrigger.SetActive(true);
        }
    }
}
