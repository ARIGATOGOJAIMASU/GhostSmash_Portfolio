using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Junpyo
{
    public class Player_Info_UI : MonoBehaviour
    {
        [SerializeField] public Image BackGroundImge;
        [SerializeField] public Image CharacterImge;
        [SerializeField] public Image HP_Bar;
        [SerializeField] public GameObject Ultimate_Ui;
        [HideInInspector] public int ID;
        [HideInInspector]public PlayerController_FSM Player;

        private void Update()
        {
            if (Player != null)
            {
                HP_Bar.fillAmount = Player.HP_Bar.fillAmount;

                if (Ultimate_Ui != null)
                {
                    if (Player.playerInformation.UltimateOn)
                    {
                        Ultimate_Ui.SetActive(true);
                    }
                    else
                    {
                        Ultimate_Ui.SetActive(false);
                    }
                }
            }
        }
    }
}
