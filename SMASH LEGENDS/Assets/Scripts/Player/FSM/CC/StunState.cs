using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class StunState : State_Base
    {
        float Timer;
        float StunStateTime = 3.0f;
        bool Ground;

        public override void Setting() { StateType = PLAYERSTATE.STUN; }

        public override void StateEnter()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationTrigger("Hurt", Pv_ID);
                GameManager.Instance.ConditionEffectOn(Wooseok.ATTACKTYPE.STUN, true, Pv_ID);
            }
            else
            {
                Owner_animator.SetTrigger("Hurt");
            }

            Ground = false;
        }

        public override void Update()
        {
            Timer += Time.deltaTime;

            //일정시간동안 기절상태가 안 풀린다.
            if (StunStateTime < Timer)
            {
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
            else if(0.1f < Timer)
            {
                if((GroundPos.position.y > Owner.position.y) && !Ground)
                {
                    Debug.Log("땅에 닿았다");
                    Ground = true;

                    if (PhotonNetwork.IsConnected)
                    {
                        GameManager.Instance.AnimationTrigger("Stun", Pv_ID);
                        GameManager.Instance.ConditionEffectOn(Wooseok.ATTACKTYPE.STUN, true, Pv_ID);
                    }
                    else
                    {
                        Owner_animator.SetTrigger("Stun");
                    }
                }
            }
        }

        public override void StateExit()
        {
            Timer = 0.0f;

            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.ConditionEffectOn(Wooseok.ATTACKTYPE.STUN, false, Pv_ID);
            }

            Ground = false;
        }
    }
}
