using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Junpyo;

namespace Wooseok
{
    public class Item_CoolDownBoost : FieldItem
    {

        public override void Effect(PlayerController_FSM player)
        {
            if (player.playerInformation.Cur_UltGage + amount > player.playerInformation.UltGage_Max)
            {
                player.playerInformation.Cur_UltGage = player.playerInformation.UltGage_Max;
            }
            else
            {
                player.playerInformation.Cur_UltGage += amount;
            }

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}