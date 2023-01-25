using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Junpyo
{
    class DeadState : State_Base
    {
        [SerializeField] private float CurTime;
        private float ReviveWaitingTime = 5.0f;
        private GameObject RevivalUI;
        private Image WaitingImage;
        private RevivalUI revival_UI_Script;

        public override void Setting()
        {
            StateType = PLAYERSTATE.DEAD;
            RevivalUI = Owner_Script.RevivalUI.gameObject;
            WaitingImage = Owner_Script.RevivalWaitingImage;
            revival_UI_Script = RevivalUI.transform.GetChild(0).GetComponent<RevivalUI>();
        }
        public override void StateEnter()
        {
            //부활UI설정
            RevivalUI.SetActive(true);

            //떨어져 죽은 것이 아니라면 누가 자신을 죽였는지 표시함
            if (!Owner_Script.fallDead)
            {
                revival_UI_Script.SetRevivalUI(Owner_Script.CharacterName, Owner_Script.EnemyCharacter);
            }
            else
            {
                revival_UI_Script.gameObject.SetActive(false);
                Owner_Script.fallDead = false;
            }

            if (PhotonNetwork.IsConnected)
            {
                EffectManager.Instance.EffectInst(EFFECT.DEAD, Owner.transform.position);
            }
        }

        public override void Update()
        {
            //부활시간을 초기화
            CurTime += Time.deltaTime;

            //uI를 통해 부활대기시간 표시
            WaitingImage.fillAmount = (CurTime / ReviveWaitingTime);

            //측정한 시간이 대기시간을 넘기면 부활
            if (ReviveWaitingTime < CurTime)
            {
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }

        public override void StateExit()
        {
            //타이머 초기화
            CurTime = 0.0f;

            //Player설정 초기화
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.Dead(true, Pv_ID);
            }
            else
            {
                Owner_Script.Dead(true);
            }

            Owner_rigidbody.isKinematic = false;
            Owner_rigidbody.useGravity = true;

            Owner_Script.playerInformation.Cur_HP = Owner_Script.playerInformation.HP_Max;

            //SpawnPos로 위치 조정
            Owner.position = Owner_Script.SpawnPos;
            //Owner.position -= new Vector3(0, Owner.position.y - 5.0f, 0);

            //PlayerAnimation 다시 재성
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationPlay(Pv_ID, true);
                GameManager.Instance.HP(Owner_Script.playerInformation.HP_Max, Pv_ID);
            }
            else
            {
                Owner_animator.StopPlayback();
            }

            //Ui및 GroundCheak기능 활성화
            HP_Bar.fillAmount = 1.0f;

            //RevivalUi 비활성화 및 초기화
            RevivalUI.gameObject.SetActive(false);
            revival_UI_Script.gameObject.SetActive(true);
            WaitingImage.fillAmount = 1;       

            //LookCheck 활성화
            Owner_Script.LookCheck.SetActive(true);

            //Layer값 Player로 변경
            Owner.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
}
