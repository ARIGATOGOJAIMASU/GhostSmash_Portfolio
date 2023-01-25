using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{


    public class Skill_BouncingSlash : Skill_Slash
    {
        [SerializeField]
        Vector2 BouncingDirection;

        public Skill_BouncingSlash(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }
        protected override Vector3 DrawDirection(GameObject otherobj)
        {
            Vector3 direction = otherobj.transform.position - ParentPlayer.transform.position;
            direction.y = 0f;
            direction.Normalize();
            direction = direction * BouncingDirection.x + Vector3.down * BouncingDirection.y;

            return direction;
        }

        protected override void HitEnemy(GameObject otherobj)
        {
            Junpyo.PlayerController_FSM HitObj = otherobj.GetComponent<Junpyo.PlayerController_FSM>();

            //상대를 날리는 방향
            Vector3 Direction;

            //맞은 객체와 자신의 벡터를 이용해 자신에서 맞은객체 쪽으로 이동하는 벡터를 구함
            Direction = DrawDirection(otherobj);
            //맞은 객체에 피격함수를 호출

            GameManager.Instance.GagePus(Ultimatecharge, ParentScript.ID);
            Debug.Log("HurtOn");
            GameManager.Instance.MovePlayer(otherobj.transform.position + (Direction / 60), HitObj.ID);
            GameManager.Instance.Hurt(Direction, debuffDuration, AttackType, Damage, HitSoundNumber, ParentScript.ID, HitObj.ID);
            //데미지가 0이 아니라면 카메라효과 및 HitEffect연출
            if (Damage != 0)
            {
                AfterEffect(Junpyo.EFFECT.HIT, otherobj.GetComponent<Collider>());
                GameManager.Instance.CameraShaking(ParentScript.ID);
                //PlayHitSound();
            }
        }
    }
}