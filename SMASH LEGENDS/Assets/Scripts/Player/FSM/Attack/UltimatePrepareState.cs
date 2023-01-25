using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class UltimatePrepareState : State_Base 
    {
        float Magnitude = 19;

        public override void Setting()
        {
            StateType = PLAYERSTATE.ULTIMATEPREPARE;
        }

        public override void StateEnter()
        {
            //Layer교채
            Owner.gameObject.layer = LayerMask.NameToLayer("SuperArmer");

            //궁극기 게이지 0으로 초기화 후 에셋도 초기화
            Owner_Script.UseUtimate();

            //애니메이션 재생
            if(PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("UltimatePrepare", true,Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("UltimatePrepare", true);
            }
        }

        public override void Update()
        {
            if (Owner_rigidbody.velocity.y <= 0.2f)
            {
                Owner_rigidbody.velocity -= new Vector3(0, Time.deltaTime * Magnitude, 0);
            }

            if (Owner_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95 && Owner_animator.GetCurrentAnimatorStateInfo(0).IsName("UltimatePrepare"))
            {
                ChracterUltimate();
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("UltimatePrepare", false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("UltimatePrepare", false);
            }
        }

        public void ChracterUltimate()
        {
            switch (Owner_Script.playerInformation.Name)
            {
                case CHARACTERNAME.CHEPESYU:
                    state_Machine.ChangeState(PLAYERSTATE.CHEPESYULTIMATE);
                    break;
            }
        }
    }
}
