using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Juhyung
{
    public class TTDLogic : MonoBehaviourPun
    {
        #region Ghost Trigger Value
        [SerializeField] private int RedTeamPlayer = 0;                 // 레드팀 플레이어 수
        [SerializeField] private int BlueTeamPlayer = 0;                // 블루팀 플레이어 수

        #endregion

        #region Ghost Logic Value
        //---Point---//
        [SerializeField] private List<GameObject> g_Waypoint;           // Ghost Waypoint
        [SerializeField] private List<GameObject> g_RedCheckpoint;      // Red Team 체크포인트
        [SerializeField] private List<GameObject> g_BlueCheckpoint;     // Blue Team 체크포인트

        //-----UI-----//
        [SerializeField] private Slider Main_UI_Line;                   // Main UI Line
        [SerializeField] private List<GameObject> Main_UI_Line_Icon;    // Main UI Line Icon

        [SerializeField] private Image Main_UI_Red_fill;                // Main UI RedTeam Gage
        [SerializeField] private Image Main_UI_Blue_fill;               // Main UI BlueTeam Gage

        [SerializeField] private List<GameObject> Main_UI_Red_OCC;
        [SerializeField] private List<GameObject> Main_UI_Blue_OCC;

        [SerializeField] private GameObject Red_MatchPoint;
        [SerializeField] private GameObject Blue_MatchPoint;

        [SerializeField] private GameObject Red_MatchOver;
        [SerializeField] private GameObject Blue_MatchOver;

        //---Value---//
        [SerializeField] private int Way_Idx;                           // Waypoint Index

        private bool b_Red = true;                                      // 레드팀 방향
        private bool b_Blue = true;                                     // 블루팀 방향

        private Vector3 cur_Pos;                                        // 현재 Ghost 위치

        private float GameEndTime;

        #endregion

        #region Ghost Move Value
        [SerializeField] private float ghost_Movespeed;                 // 화물 이동 속도
        [SerializeField] private float _MaxLen;                         // 화물 경로 길이

        private float _dis;                                             // 화물 이동 거리

        #endregion

        #region Ghost Gage Value
        [SerializeField] private Image Ghost_Red_Fill;                  // Ghost Red Circle Progress
        [SerializeField] private Image Ghost_Blue_Fill;                 // Ghost Blue Circle Progress

        [SerializeField] private Image Ghost_RedFinal_Fill;             // 레드팀 마지막 점령
        [SerializeField] private Image Ghost_BlueFinal_Fill;            // 블루팀 마지막 점령

        [SerializeField] private float Ghost_fillSpeed;                 // Ghost gage 차는 속도

        private bool Check_fill = false;
        #endregion

        // Update is called once per frame
        void Update()
        {
            if (g_RedCheckpoint.Count != 0 && g_BlueCheckpoint.Count != 0)
            {
                GhostLogic();
            }
            else
            {
                Time.timeScale = 0;

                GameManager.Instance.GameOver();

                if(g_RedCheckpoint.Count == 0)
                {
                    TTDManager.Instance.Blue_Win = true;

                    Blue_MatchPoint.SetActive(true);

                    GameEndTime += Time.unscaledDeltaTime;
                    if (GameEndTime >= 2.0f)
                    {
                        Blue_MatchPoint.SetActive(false);
                        Blue_MatchOver.SetActive(true);

                        if (GameEndTime >= 4.0f)
                        {
                            TTDManager.Instance.GameSet();
                        }
                    }
                }
                else if(g_BlueCheckpoint.Count == 0)
                {
                    TTDManager.Instance.Red_Win = true;

                    Red_MatchPoint.SetActive(true);

                    GameEndTime += Time.unscaledDeltaTime;
                    if (GameEndTime >= 2.0f)
                    {
                        Red_MatchPoint.SetActive(false);
                        Red_MatchOver.SetActive(true);

                        if (GameEndTime >= 4.0f)
                        {
                            TTDManager.Instance.GameSet();
                        }
                    }
                }
            }
        }

        #region Ghost Trigger Logic
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Red"))
            {
                RedTeamPlayer++;
            }

            if (other.tag.Equals("Blue"))
            {
                BlueTeamPlayer++;
            }
            Debug.Log("레드: " + RedTeamPlayer + "블루" + BlueTeamPlayer);
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Red"))
            {
                RedTeamPlayer--;
            }

            if (other.tag.Equals("Blue"))
            {
                BlueTeamPlayer--;
            }
            Debug.Log("레드: " + RedTeamPlayer + "블루" + BlueTeamPlayer);
        }

        #endregion

        #region Ghost Logic
        private void GhostLogic()
        {
            // Red Team만 화물에 있을 때
            if (RedTeamPlayer > 0 && BlueTeamPlayer == 0)
            {
                if (Ghost_Blue_Fill.fillAmount == 0.0f)
                {
                    if (b_Red)
                    {
                        Way_Idx--;

                        b_Red = false;
                        b_Blue = true;
                    }
                }

                if (Vector3.Distance(g_Waypoint[Way_Idx].transform.position, cur_Pos) == 0.0f)
                {
                    GhostGage();
                }

                GhostMove();
            }
            // Blue Team만 화물에 있을 때
            else if (RedTeamPlayer == 0 && BlueTeamPlayer > 0)
            {
                if (Ghost_Red_Fill.fillAmount == 0.0f)
                {
                    if (b_Blue)
                    {
                        Way_Idx++;

                        b_Red = true;
                        b_Blue = false;
                    }
                }

                if (Vector3.Distance(g_Waypoint[Way_Idx].transform.position, cur_Pos) == 0.0f)
                {
                    GhostGage();
                }

                GhostMove();
            }
        }

        #endregion

        #region Ghost Move Logic
        private void GhostMove()
        {
            cur_Pos = transform.position;

            if (Way_Idx < g_Waypoint.Count)
            {
                float step = ghost_Movespeed * Time.deltaTime;

                transform.position = Vector3.MoveTowards(cur_Pos, g_Waypoint[Way_Idx].transform.position, step);

                _dis = Vector3.Distance(cur_Pos, transform.position);

                if (RedTeamPlayer > 0 && BlueTeamPlayer == 0)
                {
                    Main_UI_Line.value -= _dis / _MaxLen;
                }
                else if (RedTeamPlayer == 0 && BlueTeamPlayer > 0)
                {
                    Main_UI_Line.value += _dis / _MaxLen;
                }
            }
        }

        #endregion

        #region Ghost Gage Logic
        private void GhostGage()
        {
            if (RedTeamPlayer > 0 && BlueTeamPlayer == 0)
            {
                if (Ghost_Blue_Fill.fillAmount == 0.0f)
                {
                    for (int i = 0; i < g_BlueCheckpoint.Count; ++i)
                    {
                        if (Vector3.Distance(g_BlueCheckpoint[i].transform.position, cur_Pos) == 0.0f)
                        {
                            if (g_BlueCheckpoint.Count > 1)
                            {
                                if (Main_UI_Line_Icon[0].activeSelf)
                                {
                                    Main_UI_Line_Icon[0].SetActive(false);
                                    Main_UI_Line_Icon[1].SetActive(true);
                                    Main_UI_Line_Icon[2].SetActive(false);
                                }

                                if (Ghost_Red_Fill.fillAmount < 1.0f)
                                {
                                    Ghost_Red_Fill.fillAmount += Time.deltaTime / Ghost_fillSpeed;
                                    Main_UI_Red_fill.fillAmount = Ghost_Red_Fill.fillAmount;
                                    Check_fill = false;

                                    break;
                                }
                                else
                                {
                                    Main_UI_Line_Icon[0].SetActive(true);
                                    Main_UI_Line_Icon[1].SetActive(false);
                                    Main_UI_Line_Icon[2].SetActive(false);

                                    if (Main_UI_Blue_OCC.Count > 0)
                                    {
                                        Main_UI_Blue_OCC[0].SetActive(false);

                                        Main_UI_Blue_OCC.RemoveAt(0);
                                    }

                                    g_BlueCheckpoint.RemoveAt(i);
                                    Ghost_Red_Fill.fillAmount = 0.0f;
                                    Main_UI_Red_fill.fillAmount = 0.0f;

                                    Check_fill = true;

                                    break;
                                }
                            }
                            else
                            {
                                if (Main_UI_Line_Icon[0].activeSelf)
                                {
                                    Main_UI_Line_Icon[0].SetActive(false);
                                    Main_UI_Line_Icon[1].SetActive(false);
                                    Main_UI_Line_Icon[2].SetActive(false);
                                }

                                if (Ghost_Red_Fill.fillAmount < 1.0f)
                                {
                                    Ghost_Red_Fill.fillAmount += Time.deltaTime / Ghost_fillSpeed;
                                    Ghost_RedFinal_Fill.fillAmount = Ghost_Red_Fill.fillAmount;
                                    Check_fill = false;

                                    break;
                                }
                                else
                                {
                                    g_BlueCheckpoint.RemoveAt(i);
                                    Ghost_Red_Fill.fillAmount = 1.0f;
                                    Ghost_RedFinal_Fill.fillAmount = 1.0f;

                                    Check_fill = true;

                                    break;
                                }
                            }
                        }
                        else
                        {
                            Check_fill = true;
                        }
                    }
                }
                else
                {
                    if (g_RedCheckpoint.Count > 1)
                    {
                        Ghost_Blue_Fill.fillAmount -= Time.deltaTime / Ghost_fillSpeed;
                        Main_UI_Blue_fill.fillAmount = Ghost_Blue_Fill.fillAmount;
                    }
                    else
                    {
                        Ghost_Blue_Fill.fillAmount -= Time.deltaTime / Ghost_fillSpeed;
                        Ghost_BlueFinal_Fill.fillAmount = Ghost_Blue_Fill.fillAmount;
                    }

                    if (Ghost_Blue_Fill.fillAmount == 0.0f)
                    {
                        Main_UI_Line_Icon[0].SetActive(true);
                        Main_UI_Line_Icon[1].SetActive(false);
                        Main_UI_Line_Icon[2].SetActive(false);
                    }
                }

                if (Check_fill)
                {
                    Way_Idx--;
                    Check_fill = false;
                }
            }
            else if (RedTeamPlayer == 0 && BlueTeamPlayer > 0)
            {
                if (Ghost_Red_Fill.fillAmount == 0.0f)
                {
                    for (int i = 0; i < g_RedCheckpoint.Count; ++i)
                    {
                        if (Vector3.Distance(g_RedCheckpoint[i].transform.position, cur_Pos) == 0.0f)
                        {
                            if (g_RedCheckpoint.Count > 1)
                            {
                                if (Main_UI_Line_Icon[0].activeSelf)
                                {
                                    Main_UI_Line_Icon[0].SetActive(false);
                                    Main_UI_Line_Icon[1].SetActive(false);
                                    Main_UI_Line_Icon[2].SetActive(true);
                                }

                                if (Ghost_Blue_Fill.fillAmount < 1.0f)
                                {
                                    Ghost_Blue_Fill.fillAmount += Time.deltaTime / Ghost_fillSpeed;
                                    Main_UI_Blue_fill.fillAmount = Ghost_Blue_Fill.fillAmount;
                                    Check_fill = false;

                                    break;
                                }
                                else
                                {
                                    Main_UI_Line_Icon[0].SetActive(true);
                                    Main_UI_Line_Icon[1].SetActive(false);
                                    Main_UI_Line_Icon[2].SetActive(false);

                                    if (Main_UI_Red_OCC.Count > 0)
                                    {
                                        Main_UI_Red_OCC[0].SetActive(false);

                                        Main_UI_Red_OCC.RemoveAt(0);
                                    }

                                    g_RedCheckpoint.RemoveAt(i);
                                    Ghost_Blue_Fill.fillAmount = 0.0f;
                                    Main_UI_Blue_fill.fillAmount = 0.0f;

                                    Check_fill = true;

                                    break;
                                }
                            }
                            else
                            {
                                if (Main_UI_Line_Icon[0].activeSelf)
                                {
                                    Main_UI_Line_Icon[0].SetActive(false);
                                    Main_UI_Line_Icon[1].SetActive(false);
                                    Main_UI_Line_Icon[2].SetActive(false);
                                }

                                if (Ghost_Blue_Fill.fillAmount < 1.0f)
                                {
                                    Ghost_Blue_Fill.fillAmount += Time.deltaTime / Ghost_fillSpeed;
                                    Ghost_BlueFinal_Fill.fillAmount = Ghost_Blue_Fill.fillAmount;
                                    Check_fill = false;

                                    break;
                                }
                                else
                                {
                                    g_RedCheckpoint.RemoveAt(i);
                                    Ghost_Blue_Fill.fillAmount = 1.0f;
                                    Ghost_BlueFinal_Fill.fillAmount = 1.0f;

                                    Check_fill = false;

                                    break;
                                }
                            }
                        }
                        else
                        {
                            Check_fill = true;
                        }
                    }
                }
                else
                {
                    if (g_BlueCheckpoint.Count > 1)
                    {
                        Ghost_Red_Fill.fillAmount -= Time.deltaTime / Ghost_fillSpeed;
                        Main_UI_Red_fill.fillAmount = Ghost_Red_Fill.fillAmount;
                    }
                    else
                    {
                        Ghost_Red_Fill.fillAmount -= Time.deltaTime / Ghost_fillSpeed;
                        Ghost_RedFinal_Fill.fillAmount = Ghost_Red_Fill.fillAmount;
                    }

                    if (Ghost_Red_Fill.fillAmount <= 0.0f)
                    {
                        Main_UI_Line_Icon[0].SetActive(true);
                        Main_UI_Line_Icon[1].SetActive(false);
                        Main_UI_Line_Icon[2].SetActive(false);
                    }
                }

                if (Check_fill)
                {
                    Way_Idx++;
                    Check_fill = false;
                }
            }
        }

        #endregion

        public void ChangeColorUI()
        {
            ColorChange(Ghost_Red_Fill, Ghost_Blue_Fill);
            ColorChange(Main_UI_Red_fill, Main_UI_Blue_fill);
        }

        protected void ColorChange(Image red, Image blue)
        {
            Vector4 RedColor = red.color;

            red.color = blue.color;
            blue.color = RedColor;
        }

        protected void SpriteChange(Image red, Image blue)
        {
            Sprite RedImage = red.sprite;
            red.sprite = blue.sprite;
            blue.sprite = RedImage;
        }
    }
}
