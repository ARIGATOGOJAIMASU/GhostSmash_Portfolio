using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Junpyo
{
    public class UIManagement : MonoBehaviour
    {
        List<PlayerController_FSM> Players = new List<PlayerController_FSM>();
        [SerializeField] Sprite[] Characterimage;

        [SerializeField] protected Player_Info_UI[] Red_Info_Ui;
        [SerializeField] protected Player_Info_UI[] Blue_Info_Ui;

        [SerializeField] protected Sprite redHpBar_Sprite;
        [SerializeField] protected Sprite blueHpBar_Sprite;

        [SerializeField] protected Sprite redBack_Sprite;
        [SerializeField] protected Sprite blueBack_Sprite;

        [SerializeField] protected Image[] Red_SpriteChange_UI;
        [SerializeField] protected Image[] Blue_SpriteChange_UI;

        [SerializeField] protected Image[] Red_ColorChange_UI;
        [SerializeField] protected Image[] Blue_ColorChange_UI;

        [SerializeField] protected GamLogicManager gamLogicManager;

        public virtual void Setting()
        {
            Debug.Log("들어가나?");
            foreach (PlayerController_FSM player in Players)
            {
                //클라이언트 찾기
                if (player.IsMine)
                {
                    if (player.CompareTag("Red"))
                    {
                        Red_Info_Ui[0].CharacterImge.sprite = Characterimage[(int)player.playerInformation.Name];
                        Red_Info_Ui[0].ID = player.ID;
                        Red_Info_Ui[0].Player = player;
                    }
                    else
                    {
                        Blue_Info_Ui[0].CharacterImge.sprite = Characterimage[(int)player.playerInformation.Name];
                        Blue_Info_Ui[0].ID = player.ID;
                        Blue_Info_Ui[0].Player = player;
                    }
                    tag = player.tag;

                    break;
                }
            }

            //자신이 Red팀일때
            if (tag == "Red")
            {
                ColorChange();
                RedTeam();
            }
            else
            {
                BlueTeam();
            }

            gamLogicManager.ReayPlayer();
        }

        public void AddPlayer(PlayerController_FSM player)
        {
            Players.Add(player);

            if(GameManager.Instance.SetMaxPlayer() == Players.Count)
            {
                Setting();
            }
        }

        protected void ColorChange()
        {
            SetUI();

            foreach (Player_Info_UI info in Red_Info_Ui)
            {
                info.BackGroundImge.sprite = blueBack_Sprite;
                info.HP_Bar.sprite = blueHpBar_Sprite;
            }

            foreach (Player_Info_UI info in Blue_Info_Ui)
            {
                info.BackGroundImge.sprite = redBack_Sprite;
                info.HP_Bar.sprite = redHpBar_Sprite;
            }
        }

        protected void RedTeam()
        {
            int players = 1;
            int Enemys = 0;

            foreach (PlayerController_FSM player in Players)
            {
                if (player.IsMine)
                {
                    continue;
                }

                //우리 팀인 경우
                if (player.CompareTag(tag))
                {
                    Red_Info_Ui[players].CharacterImge.sprite = Characterimage[(int)player.playerInformation.Name];
                    Red_Info_Ui[players].ID = player.ID;
                    Red_Info_Ui[players].Player = player;
                    ++players;
                }
                else
                {
                    Blue_Info_Ui[Enemys].CharacterImge.sprite = Characterimage[(int)player.playerInformation.Name];
                    Blue_Info_Ui[Enemys].ID = player.ID;
                    Blue_Info_Ui[Enemys].Player = player;
                    ++Enemys;
                }
            }
        }

        protected void BlueTeam()
        {
            int players = 1;
            int Enemys = 0;

            foreach (PlayerController_FSM player in Players)
            {
                if (player.IsMine)
                {
                    continue;
                }

                //우리 팀인 경우
                if (player.CompareTag(tag))
                {
                    Blue_Info_Ui[players].CharacterImge.sprite = Characterimage[(int)player.playerInformation.Name];
                    Blue_Info_Ui[players].ID = player.ID;
                    Blue_Info_Ui[players].Player = player;
                    ++players;
                }
                else
                {
                    Red_Info_Ui[Enemys].CharacterImge.sprite = Characterimage[(int)player.playerInformation.Name];
                    Red_Info_Ui[Enemys].ID = player.ID;
                    Red_Info_Ui[Enemys].Player = player;
                    ++Enemys;
                }
            }
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

        protected virtual void SetUI()
        {
            for (int i = 0; i < Red_SpriteChange_UI.Length; ++i)
            {
                SpriteChange(Red_SpriteChange_UI[i], Blue_SpriteChange_UI[i]);
            }

            for (int i = 0; i < Red_ColorChange_UI.Length; ++i)
            {
                ColorChange(Red_ColorChange_UI[i], Blue_ColorChange_UI[i]);
            }
        }
    }
}
