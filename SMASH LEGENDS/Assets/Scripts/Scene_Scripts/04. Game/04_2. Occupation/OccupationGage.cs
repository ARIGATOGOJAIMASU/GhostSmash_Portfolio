using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Juhyung
{
    public class OccupationGage : MonoBehaviourPun
    {
        [SerializeField] private float Set1stGageSpeed;                                 // FirstGage Fill Up Speed
        [SerializeField] private float Set2ndGageSpeed;                                 // SecondGage Fill Up Speed
        [SerializeField] private float Set3rdGageSpeed;                                 // ThirdGage Fill Up Speed
        [SerializeField] private float SetKDGageSpeed;                                 // ForthGage Fill Up Speed

        [SerializeField] public Image Red1stGage_Fill;
        [SerializeField] public Image Blue1stGage_Fill;
        [SerializeField] public Image Red2stGage_Fill;
        [SerializeField] public Image Blue2stGage_Fill;
        [SerializeField] public Image Red3stGage_Fill;
        [SerializeField] public Image Blue3stGage_Fill;
        [SerializeField] public Image RedKDGage_Fill;
        [SerializeField] public Image BlueKDGage_Fill;
        [SerializeField] public Image RedKDGageText;
        [SerializeField] public Image BlueKDGageText;

        [SerializeField] public Image RedBase_Fill;
        [SerializeField] public Image BlueBase_Fill;

        [SerializeField] private int MaxScore = 4;                                      // Score of Victory

        [SerializeField] private List<Sprite> i_Num;                                    // 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 sprite

        [SerializeField] private GameObject BlankGage;
        [SerializeField] public GameObject RedGage;
        [SerializeField] public GameObject BlueGage;

        [SerializeField] private GameObject k_BlankGage;
        [SerializeField] private GameObject k_RedGage;
        [SerializeField] private GameObject k_BlueGage;

        [SerializeField] private GameObject Base_;
        [SerializeField] public GameObject Base_Red;
        [SerializeField] public GameObject Base_Blue;

        [SerializeField] public GameObject Red_MatchPoint;
        [SerializeField] public GameObject Blue_MatchPoint;
        [SerializeField] public Image Red_MatchText;
        [SerializeField] public Image Blue_MatchText;

        [SerializeField] public GameObject Red_MatchOver;
        [SerializeField] public GameObject Blue_MatchOver;
        [SerializeField] public Image Red_MatchOverText;
        [SerializeField] public Image Blue_MatchOverText;


        [SerializeField] private List<Image> RedScore_blank;                            // RedScore Blank image
        [SerializeField] private List<Image> BlueScore_blank;                           // BlueScore Blank image
        [SerializeField] public Sprite RedScore_Fill;                                  // RedScore Fill Sprite
        [SerializeField] public Sprite BlueScore_Fill;                                 // BlueScore Fill Sprite

        [SerializeField] private Image RedScoreBox_Num;                                 // RedTeam Score Num
        [SerializeField] private Image BlueScoreBox_Num;                                // BlueTeam Score Num

        private bool FirstGage = true;                                                  // Setting FirstGage bool
        private bool b_KDGage = false;

        private int RedOrBlue = 0;                                                      // RedTeam Occupate or BlueTeam Occupate 1 is Red, 2 is Blue

        private float Red1stGage = 0.0f;                                                // First Occupate RedTeam Gage
        private float Blue1stGage = 0.0f;                                               // First Occupate BlueTeam Gage

        private float Red2ndGage = 0.0f;                                                // Second Occupate RedTeam Gage
        private float Blue2ndGage = 0.0f;                                               // Second Occupate BlueTeam Gage

        private float Red3rdGage = 0.0f;                                                // Third Occupate RedTeam Gage
        private float Blue3rdGage = 0.0f;                                               // Third Occupate BlueTeam Gage

        private float RedKDGage = 0.0f;
        private float BlueKDGage = 0.0f;

        private float GameEndTime;

        private Sound3DManager mysound;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            OccupateGageLogic();
        }

        private void OccupateGageLogic()
        {
            if (Time.timeScale != 0)
            {
                if (FirstGage)
                {
                    FirstGageLogic();
                }
                else
                {
                    Red1stGage = 0.0f;
                    Blue1stGage = 0.0f;


                    if (!b_KDGage)
                    {
                        SecondGageLogic();
                        ThirdGageLogic();
                    }
                    else
                    {
                        KDGage();
                    }
                }
            }
            else
            {
                Time.timeScale = 0;
                GameManager.Instance.GameOver();

                if (OccupationManager.Instance.RedScore == MaxScore)
                {
                    OccupationManager.Instance.Red_Win = true;

                    Red_MatchPoint.SetActive(true);

                    GameEndTime += Time.unscaledDeltaTime;

                    if (GameEndTime >= 2.0f)
                    {
                        Red_MatchPoint.SetActive(false);
                        Red_MatchOver.SetActive(true);

                        if (GameEndTime >= 4.0f)
                        {
                            OccupationManager.Instance.GameSet();
                        }
                    }
                }
                else if (OccupationManager.Instance.BlueScore == MaxScore)
                {
                    OccupationManager.Instance.Blue_Win = true;

                    Blue_MatchPoint.SetActive(true);

                    GameEndTime += Time.unscaledDeltaTime;

                    if (GameEndTime >= 2.0f)
                    {
                        Blue_MatchPoint.SetActive(false);
                        Blue_MatchOver.SetActive(true);

                        if (GameEndTime >= 4.0f)
                        {
                            OccupationManager.Instance.GameSet();
                        }
                    }
                }
                else if(Time.timeScale == 0)
                {
                    if(OccupationManager.Instance.Red_Win)
                    {
                        GameEndTime += Time.unscaledDeltaTime;

                        Red_MatchOver.SetActive(true);

                        if (GameEndTime >= 2.0f)
                        {
                            OccupationManager.Instance.GameSet();
                        }
                    }
                    else if(OccupationManager.Instance.Blue_Win)
                    {
                        GameEndTime += Time.unscaledDeltaTime;

                        Blue_MatchOver.SetActive(true);

                        if (GameEndTime >= 2.0f)
                        {
                            OccupationManager.Instance.GameSet();
                        }
                    }
                    else
                    {
                        Time.timeScale = 1;
                        OccupationManager.Instance.GameRe();
                    }
                }
            }
        }

        private void FirstGageLogic()
        {
            // Under 100%
            if (Red1stGage + Blue1stGage < 100.0f)
            {
                if (OccupationManager.Instance.RedTeamPlayer > 0)
                {
                    Red1stGage += Time.deltaTime * Set1stGageSpeed;
                }

                if (OccupationManager.Instance.BlueTeamPlayer > 0)
                {
                    Blue1stGage += Time.deltaTime * Set1stGageSpeed;
                }

                if (OccupationManager.Instance.RedTeamPlayer == 0 && OccupationManager.Instance.BlueTeamPlayer == 0)
                {
                    if (Red1stGage > 0)
                    {
                        Red1stGage -= Time.deltaTime * Set1stGageSpeed;
                    }

                    if (Blue1stGage > 0)
                    {
                        Blue1stGage -= Time.deltaTime * Set1stGageSpeed;
                    }
                }
            }
            // Over 100%
            else
            {
                if (Red1stGage < 100.0f && Blue1stGage < 100.0f)
                {
                    if (OccupationManager.Instance.RedTeamPlayer == 0 && OccupationManager.Instance.BlueTeamPlayer > 0)
                    {
                        if (Red1stGage > 0)
                        {
                            Red1stGage -= Time.deltaTime * Set1stGageSpeed;
                        }

                        Blue1stGage += Time.deltaTime * Set1stGageSpeed;
                    }

                    else if (OccupationManager.Instance.RedTeamPlayer > 0 && OccupationManager.Instance.BlueTeamPlayer == 0)
                    {
                        Red1stGage += Time.deltaTime * Set1stGageSpeed;

                        if (Blue1stGage > 0)
                        {
                            Blue1stGage -= Time.deltaTime * Set1stGageSpeed;
                        }
                    }

                    else if (OccupationManager.Instance.RedTeamPlayer == 0 && OccupationManager.Instance.BlueTeamPlayer == 0)
                    {
                        if (Red1stGage > 0)
                        {
                            Red1stGage -= Time.deltaTime * Set1stGageSpeed;
                        }

                        if (Blue1stGage > 0)
                        {
                            Blue1stGage -= Time.deltaTime * Set1stGageSpeed;
                        }
                    }
                }
                else
                {
                    FirstGage = false;

                    if (Red1stGage >= 100.0f)
                    {
                        OccupationManager.Instance.Red_Win = true;

                        RedOrBlue = 1;
                        BlankGage.SetActive(false);
                        RedGage.SetActive(true);
                    }
                    else if (Blue1stGage >= 100.0f)
                    {
                        OccupationManager.Instance.Blue_Win = true;

                        RedOrBlue = 2;
                        BlankGage.SetActive(false);
                        BlueGage.SetActive(true);
                    }

                    BlueBase_Fill.fillClockwise = true;
                }
            }

            Red1stGage_Fill.fillAmount = Red1stGage / 100.0f;
            Blue1stGage_Fill.fillAmount = Blue1stGage / 100.0f;

            RedBase_Fill.fillAmount = Red1stGage / 100.0f;
            BlueBase_Fill.fillAmount = Blue1stGage / 100.0f;

            if (RedBase_Fill.fillAmount >= 1.0f)
            {
                RedBase_Fill.fillAmount = 0.0f;

                Base_.SetActive(false);
                Base_Red.SetActive(true);
            }
            else if (BlueBase_Fill.fillAmount >= 1.0f)
            {
                BlueBase_Fill.fillAmount = 0.0f;

                Base_.SetActive(false);
                Base_Blue.SetActive(true);
            }
        }

        private void SecondGageLogic()
        {
            //photonView.RPC(nameof(GetScore), RpcTarget.All);
            switch (RedOrBlue)
            {
                case 1: // Red
                    if (OccupationManager.Instance.RedScore != 3)
                    {
                        if (Red2ndGage < 100.0f)
                        {
                            if (OccupationManager.Instance.RedTeamPlayer > 0)
                            {
                                Red2ndGage += Time.deltaTime * Set2ndGageSpeed;
                            }
                            else
                            {
                                if (Red2ndGage > 0)
                                {
                                    Red2ndGage -= Time.deltaTime * Set2ndGageSpeed;
                                }
                            }
                        }
                        else
                        {
                            if (PhotonNetwork.IsMasterClient)
                            {
                                photonView.RPC(nameof(GetScore), RpcTarget.All, RedOrBlue);
                            }
                        }
                    }
                    else
                    {
                        if (Red2ndGage < 100.0f)
                        {
                            if (OccupationManager.Instance.RedTeamPlayer > 0)
                            {
                                Red2ndGage += Time.deltaTime * Set2ndGageSpeed;
                            }
                            else if (OccupationManager.Instance.RedTeamPlayer == 0)
                            {
                                if (Red2ndGage > 0)
                                {
                                    Red2ndGage -= Time.deltaTime * Set2ndGageSpeed;
                                }
                            }
                        }
                        else
                        {
                            k_BlankGage.SetActive(true);
                            k_RedGage.SetActive(true);
                            k_BlueGage.SetActive(false);

                            b_KDGage = true;
                        }
                    }
                    Red2stGage_Fill.fillAmount = Red2ndGage / 100.0f;

                    break;

                case 2: // Blue
                    if (OccupationManager.Instance.BlueScore != 3)
                    {
                        if (Blue2ndGage < 100.0f)
                        {
                            if (OccupationManager.Instance.BlueTeamPlayer > 0)
                            {
                                Blue2ndGage += Time.deltaTime * Set2ndGageSpeed;
                            }
                            else
                            {
                                if (Blue2ndGage > 0)
                                {
                                    Blue2ndGage -= Time.deltaTime * Set2ndGageSpeed;
                                }
                            }
                        }
                        else
                        {
                            if (PhotonNetwork.IsMasterClient)
                            {
                                photonView.RPC(nameof(GetScore), RpcTarget.All, RedOrBlue);
                            }
                        }
                    }
                    else
                    {
                        if (Blue2ndGage < 100.0f)
                        {
                            if ((OccupationManager.Instance.BlueTeamPlayer > 0))
                            {
                                Blue2ndGage += Time.deltaTime * Set2ndGageSpeed;
                            }
                            else if (OccupationManager.Instance.BlueTeamPlayer == 0)
                            {
                                if (Blue2ndGage > 0)
                                {
                                    Blue2ndGage -= Time.deltaTime * Set2ndGageSpeed;
                                }
                            }

                            BlueKDGage_Fill.fillAmount = Blue2ndGage / 100.0f;
                        }
                        else
                        {
                            k_BlankGage.SetActive(true);
                            k_RedGage.SetActive(false);
                            k_BlueGage.SetActive(true);

                            b_KDGage = true;
                        }
                    }
                    Blue2stGage_Fill.fillAmount = Blue2ndGage / 100.0f;

                    break;

                default:
                    FirstGage = true;

                    break;
            }
        }

        private void ThirdGageLogic()
        {
            switch (RedOrBlue)
            {
                case 1:
                    if (Blue3rdGage < 100.0f)
                    {
                        if (OccupationManager.Instance.BlueTeamPlayer > 0 && OccupationManager.Instance.RedTeamPlayer == 0)
                        {
                            Blue3rdGage += Time.deltaTime * Set3rdGageSpeed;
                        }
                        else if (OccupationManager.Instance.BlueTeamPlayer == 0)
                        {
                            if (Blue3rdGage > 0)
                            {
                                Blue3rdGage -= Time.deltaTime * Set3rdGageSpeed;
                            }
                        }
                    }
                    else
                    {
                        OccupationManager.Instance.Blue_Win = true;

                        Red2ndGage = 0.0f;
                        Blue3rdGage = 0.0f;
                        RedOrBlue = 2;

                        RedGage.SetActive(false);
                        BlueGage.SetActive(true);

                        k_BlankGage.SetActive(false);
                        k_RedGage.SetActive(false);
                        k_BlueGage.SetActive(false);

                        Base_Blue.SetActive(true);
                        Base_Red.SetActive(false);

                    }
                    Blue3stGage_Fill.fillAmount = Blue3rdGage / 100.0f;
                    BlueBase_Fill.fillAmount = Blue3rdGage / 100.0f;

                    break;

                case 2:
                    if (Red3rdGage < 100.0f)
                    {
                        if (OccupationManager.Instance.RedTeamPlayer > 0 && OccupationManager.Instance.BlueTeamPlayer == 0)
                        {
                            Red3rdGage += Time.deltaTime * Set3rdGageSpeed;
                        }
                        else if (OccupationManager.Instance.RedTeamPlayer == 0)
                        {
                            if (Red3rdGage > 0)
                            {
                                Red3rdGage -= Time.deltaTime * Set3rdGageSpeed;
                            }
                        }
                    }
                    else
                    {
                        OccupationManager.Instance.Red_Win = true;

                        Blue2ndGage = 0.0f;
                        Red3rdGage = 0.0f;
                        RedOrBlue = 1;

                        BlueGage.SetActive(false);
                        RedGage.SetActive(true);

                        k_BlankGage.SetActive(false);
                        k_RedGage.SetActive(false);
                        k_BlueGage.SetActive(false);

                        Base_Blue.SetActive(false);
                        Base_Red.SetActive(true);
                    }
                    Red3stGage_Fill.fillAmount = Red3rdGage / 100.0f;
                    RedBase_Fill.fillAmount = Red3rdGage / 100.0f;

                    break;
            }
        }

        private void KDGage()
        {
            switch (RedOrBlue)
            {
                case 1:
                    if (RedKDGage < 100.0f)
                    {
                        if (OccupationManager.Instance.RedTeamPlayer > 0 && OccupationManager.Instance.BlueTeamPlayer == 0)
                        {
                            RedKDGage += Time.deltaTime * SetKDGageSpeed;
                        }
                        else if (OccupationManager.Instance.RedTeamPlayer == 0)
                        {
                            RedKDGage -= Time.deltaTime * SetKDGageSpeed;

                            if (RedKDGage <= 0.0f)
                            {
                                k_BlankGage.SetActive(false);
                                k_RedGage.SetActive(false);
                                b_KDGage = false;
                                RedKDGage = 0.0f;
                                Red2ndGage = 99.0f;
                            }
                        }
                    }
                    else
                    {
                        photonView.RPC(nameof(GetScore), RpcTarget.All, 1);
                    }

                    RedKDGage_Fill.fillAmount = RedKDGage / 100.0f;

                    break;

                case 2:
                    if (BlueKDGage < 100.0f)
                    {
                        if (OccupationManager.Instance.RedTeamPlayer == 0 && OccupationManager.Instance.BlueTeamPlayer > 0)
                        {
                            BlueKDGage += Time.deltaTime * SetKDGageSpeed;
                        }
                        else if (OccupationManager.Instance.BlueTeamPlayer == 0)
                        {
                            BlueKDGage -= Time.deltaTime * SetKDGageSpeed;

                            if (BlueKDGage <= 0.0f)
                            {
                                k_BlankGage.SetActive(false);
                                k_BlueGage.SetActive(false);
                                b_KDGage = false;
                                BlueKDGage = 0.0f;
                                Blue2ndGage = 99.0f;
                            }
                        }
                    }
                    else
                    {
                        photonView.RPC(nameof(GetScore), RpcTarget.All, 2);
                    }

                    BlueKDGage_Fill.fillAmount = BlueKDGage / 100.0f;

                    break;
            }
        }

        [PunRPC]
        private void GetScore(int teamck)
        {
            switch (teamck)
            {
                case 1:
                    if(OccupationManager.Instance.RedScore != 4)
                    {
                        RedScore_blank[OccupationManager.Instance.RedScore].sprite = RedScore_Fill;
                        OccupationManager.Instance.RedScore++;
                        RedScoreBox_Num.sprite = i_Num[OccupationManager.Instance.RedScore];

                        if (OccupationManager.Instance.RedScore == MaxScore) break;

                        Red2ndGage = 0.0f;
                    }
                    else
                    {
                        Time.timeScale = 0;

                        mysound.Play(SoundManager.Instance.HitSound[0], Sound3DManager.SOUNDTYPE.ONESHOT);
                    }

                    break;

                case 2:
                    if(OccupationManager.Instance.BlueScore != 4)
                    {
                        BlueScore_blank[OccupationManager.Instance.BlueScore].sprite = BlueScore_Fill;
                        OccupationManager.Instance.BlueScore++;
                        BlueScoreBox_Num.sprite = i_Num[OccupationManager.Instance.BlueScore];

                        if (OccupationManager.Instance.BlueScore == MaxScore) break;

                        Blue2ndGage = 0.0f;
                    }
                    else
                    {
                        Time.timeScale = 0;
                    }

                    break;
            }
        }
    }
}