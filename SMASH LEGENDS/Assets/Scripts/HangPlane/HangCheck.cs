using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class HangCheck : MonoBehaviour
    {
        [SerializeField] private int direction;
        [SerializeField] private float y =  -1.1f;
        [SerializeField] private float slope = 0.5f;
        [SerializeField] private float stright;

        public int _direction
        {
            get { return direction; }
            set { direction = value; }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                PlayerController_FSM playerScript;
                BoxCollider collide;
                playerScript = other.transform.GetComponent<PlayerController_FSM>();
                collide = GetComponent<BoxCollider>();
                y = playerScript.playerInformation.Hang_Y;
                slope = playerScript.playerInformation.HangSlope;
                stright = playerScript.playerInformation.HangStrigh;

                switch (direction)//그방향으로 Hang애니메이션 실행
                {
                    //forward
                    case 0:
                        playerScript.Hang(new Vector3(playerScript.transform.position.x, collide.bounds.max.y + y, collide.bounds.max.z - stright), new Vector3(0, 0, 0));
                        break;
                    //back
                    case 1:
                        playerScript.Hang(new Vector3(playerScript.transform.position.x, collide.bounds.max.y + y, collide.bounds.min.z + stright), new Vector3(0, 180, 0));
                        break;
                    //Right
                    case 2:
                        playerScript.Hang(new Vector3(collide.bounds.max.x - stright, collide.bounds.max.y + y, playerScript.transform.position.z), new Vector3(0, 90, 0));
                        break;
                    //Left
                    case 3:
                        playerScript.Hang(new Vector3(collide.bounds.min.x + stright, collide.bounds.max.y + y, playerScript.transform.position.z), new Vector3(0, -90, 0));
                        break;
                    //Left Foward
                    case 4:
                        playerScript.Hang(LeftForward(playerScript.transform.position, new Vector3(collide.bounds.max.x, collide.bounds.max.y + y, collide.bounds.max.z)), new Vector3(0, -45, 0));
                        break;
                    //Left back
                    case 5:
                        playerScript.Hang(LeftBack(playerScript.transform.position, new Vector3(collide.bounds.max.x, collide.bounds.max.y + y, collide.bounds.min.z)), new Vector3(0, -135, 0));
                        break;
                    //Right foward 
                    case 6:
                        playerScript.Hang(RightFoward(playerScript.transform.position, new Vector3(collide.bounds.max.x, collide.bounds.max.y + y, collide.bounds.min.z)), new Vector3(0, 45, 0));
                        break;
                    //Right back 
                    case 7:
                        playerScript.Hang(RightBack(playerScript.transform.position, new Vector3(collide.bounds.max.x, collide.bounds.max.y + y, collide.bounds.max.z)), new Vector3(0, 135, 0));
                        break;
                }
            }
        }

        //-------------------------------------------------------------------------------------Hang기능 함수---------------------------------------------------------------------------------------
        private Vector3 LeftForward(Vector3 p_Pos, Vector3 up_Pos)
        {
            float a = up_Pos.z - up_Pos.x;
            float b = p_Pos.x + p_Pos.z;

            Vector3 hang_Pos = new Vector3((b - a) * 0.5f, up_Pos.y, (a + b) * 0.5f) + new Vector3(-slope, 0, slope);

            return hang_Pos;
        }

        private Vector3 LeftBack(Vector3 p_Pos, Vector3 down_Pos)
        {
            float a = p_Pos.z - p_Pos.x;
            float b = down_Pos.x + down_Pos.z;

            Vector3 hang_Pos = new Vector3((b - a) * 0.5f, down_Pos.y, (a + b) * 0.5f) + new Vector3(-slope, 0, -slope);

            return hang_Pos;
        }

        private Vector3 RightFoward(Vector3 p_Pos, Vector3 down_Pos)
        {
            float a = p_Pos.z - p_Pos.x;
            float b = down_Pos.x + down_Pos.z;

            Vector3 hang_Pos = new Vector3((b - a) * 0.5f, down_Pos.y, (a + b) * 0.5f) + new Vector3(slope, 0, slope);

            return hang_Pos;
        }

        private Vector3 RightBack(Vector3 p_Pos, Vector3 up_Pos)
        {
            float a = up_Pos.z - up_Pos.x;
            float b = p_Pos.x + p_Pos.z;

            Vector3 hang_Pos = new Vector3((b - a) * 0.5f, up_Pos.y, (a + b) * 0.5f) + new Vector3(slope, 0, -slope);

            return hang_Pos;
        }
    }

//-------------------------------------------------------------------------------------Hang기능 함수---------------------------------------------------------------------------------------
}
