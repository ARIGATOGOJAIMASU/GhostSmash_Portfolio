using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Junpyo;

namespace Wooseok
{ 

    public class Item_Bomb : FieldItem
    {
        Skill Bomb;
        Skill Ex_Skill;


        public override void Effect(PlayerController_FSM player)
        {
            //플레이어의 플레이어 스킬에서 평타를 Ex_Skill에 넣어두기. 그리고 스킬 교체하기.
            //player.playerInformation.playerSkill.
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