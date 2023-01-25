using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Juhyung
{
    public class Bomb_Item : Item
    {
        public override void DestroyItem()
        {
            if (ItemStation.name.Equals(ItemManager.Instance.s_Getitem) && (ItemManager.Instance.s_PlayerNum != null))
            {
                RunItem();
                photonView.RPC(nameof(DestroyItemRPC), RpcTarget.All);
            }
        }

        public override void RunItem()
        {
            Junpyo.PlayerController_FSM Player = ItemManager.Instance.s_PlayerNum;
            GameManager.Instance.BoomItem(true, Player.ID);
            Player.ItemTrigger = false;
        }
    }
}

