using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Juhyung
{
    public class HealPack_Item : Item
    {
        public override void DestroyItem()
        {
            if (ItemStation.name.Equals(ItemManager.Instance.s_Getitem) && ItemManager.Instance.s_PlayerNum != null)
            {
                RunItem();
                photonView.RPC(nameof(DestroyItemRPC), RpcTarget.All);
            }
        }

        public override void RunItem()
        {
            Junpyo.PlayerController_FSM Player = ItemManager.Instance.s_PlayerNum;

            Player.Heal(0);

/*            if (Player.playerInformation.Cur_HP + 1500.0f >= Player.playerInformation.HP_Max)
            {
                Player.playerInformation.Cur_HP = Player.playerInformation.HP_Max;
                Player.HP_Bar.fillAmount = Player.playerInformation.Cur_HP / Player.playerInformation.HP_Max;
                Player.hp_Value.SetHP_Value((int)Player.playerInformation.HP_Max);
            }
            else
            {
                Player.playerInformation.Cur_HP += 1500.0f;
                Player.HP_Bar.fillAmount = Player.playerInformation.Cur_HP / Player.playerInformation.HP_Max;
                Player.hp_Value.SetHP_Value((int)Player.playerInformation.Cur_HP);
            }
            GameManager.Instance.HP(Player.playerInformation.Cur_HP, Player.ID);
            Player.ItemTrigger = false;*/
        }
    }
}
