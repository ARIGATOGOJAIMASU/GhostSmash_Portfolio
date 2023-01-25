using Junpyo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{
    public class Item_HealingPotion : FieldItem
    {
        

        public override void Effect(PlayerController_FSM player)
        {
            if(player.playerInformation.Cur_HP + amount > player.playerInformation.HP_Max)
            {
                player.playerInformation.Cur_HP = player.playerInformation.HP_Max;
            }
            else
            {
                player.playerInformation.Cur_HP += amount;
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