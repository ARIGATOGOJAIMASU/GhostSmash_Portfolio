using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Juhyung
{
    public class TTDManager : Junpyo.GamLogicManager
    {
        private static TTDManager _instance;

        public static TTDManager Instance
        {
            get
            {
                // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(TTDManager)) as TTDManager;

                    if (_instance == null)
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
        }

        public bool Red_Win = false;
        public bool Blue_Win = false;

        [SerializeField] private GameObject g_FlowerField;
        [SerializeField] private float Delay_Time;
        [SerializeField] private GameObject HangLine;

        private bool FallStart = true;
        private void Awake()
        {
            GameTime = 0.0f;
            Singleton();
            CreatePlayer();
            FallStart = true;
        }

        private void Start()
        {
            StartCoroutine(SetOccupationDelay());
        }

        private void Update()
        {
            if(GameStart)
            {
                if (FallStart)
                {
                    Junpyo.CoroutineHelper.StartCoroutine(Fall());
                    FallStart = false;
                }

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
            if(GameTime < 600.0f)
            {
                GameTime += Time.deltaTime;

                int i_Min = (int)GameTime / 60;
                int i_Tensec = (int)(GameTime - (i_Min * 60)) / 10;
                int i_Onesec = (int)(GameTime - (i_Min * 60) - (i_Tensec * 10));

                Min.sprite = i_Num[i_Min];
                TenSec.sprite = i_Num[i_Tensec];
                OneSec.sprite = i_Num[i_Onesec];
            }
            else
            {
                GameTime = 0.0f;
            }
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

        //Coroutine Funtion
        IEnumerator SetOccupationDelay()
        {
            yield return new WaitForSeconds(StartDelayTime);

            //OccupationTrigger.SetActive(true);
        }

        IEnumerator Fall()
        {
            yield return new WaitForSeconds(Delay_Time);

            g_FlowerField.GetComponent<BoxCollider>().isTrigger = true;
            HangLine.SetActive(true);

            while (true)
            {
                if (g_FlowerField.transform.position.y > -50.0f)
                {
                    g_FlowerField.transform.position = Vector3.MoveTowards(g_FlowerField.transform.position, new Vector3(0, -50.0f, 0), 5.0f * Time.deltaTime);
                    yield return null;
                }
                else
                {
                    Destroy(g_FlowerField.gameObject);

                    yield break;
                }
            }
        }
    }
}
