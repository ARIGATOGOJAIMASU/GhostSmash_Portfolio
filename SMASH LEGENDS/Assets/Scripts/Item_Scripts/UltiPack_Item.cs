using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Juhyung
{
    public class UltiPack_Item : Item
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

            Player.Heal(1);

           /* Player.UtimateGageUp(40);
            Player.ItemTrigger = false;*/
        }
    }
}
