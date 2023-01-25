using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Wooseok
{

    public class HalfSphereCollider : Skill_Colider
    {
        // this class suppose that gameobject have a sphere collider.
        // 이 클래스는 구형 콜라이더가 있다는 가정 하에 사용됨.

        SphereCollider MainCollider;

        Ray circleray;

        Vector3 playerforward;


        float angletorad = Mathf.PI / 180;

        [SerializeField]
        [Header("각도")]
        float CriterionAngle;

        private void OnTriggerEnter(Collider other)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (CheckAngle(other))
                {
                    mainskill.SkillEffectOnEnter(other.gameObject);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (CheckAngle(other))
                {
                    mainskill.SkillEffectOnStay(other.gameObject);
                }
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (CheckAngle(other))
                {
                    mainskill.SkillEffectOnExit(other.gameObject);
                }
            }
        }


        void Start()
        {
            this.MainCollider = this.gameObject.GetComponent<SphereCollider>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        bool CheckAngle(Collider other)
        {
            if (mainskill && mainskill.ParentPlayer)
            {
                playerforward = mainskill.ParentPlayer.transform.forward;
            }
            else
            {
                playerforward = this.gameObject.transform.forward;
            }
            float otherradius;
            if (other.gameObject.GetComponent<CapsuleCollider>())
            {
                otherradius = other.gameObject.GetComponent<CapsuleCollider>().radius;
            }
            else
            {
                otherradius = Vector3.Distance(other.ClosestPoint(transform.position + transform.forward), transform.position);
            }

            Vector3 thistoother = other.gameObject.transform.position - this.gameObject.transform.position; //this.mainskill.ParentPlayer.transform.position;
            float cosanglebtwforward = Vector3.Dot(playerforward, thistoother) / playerforward.magnitude * thistoother.magnitude;
            float cosanglebtwother =  Mathf.Sqrt(1 - (otherradius / thistoother.magnitude) *(otherradius / thistoother.magnitude));

            float cosangle = Mathf.Cos(CriterionAngle * angletorad);

            
            if (cosanglebtwforward + cosanglebtwother > cosangle || cosanglebtwforward - cosanglebtwother > cosangle || cosanglebtwforward + cosanglebtwother > cosangle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnDrawGizmos()
        {
            ////forward
            //Gizmos.color = Color.white;
            //Gizmos.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + this.gameObject.transform.forward);
            ////backward
            //Gizmos.color = Color.black;
            //Gizmos.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position - this.gameObject.transform.forward);
            ////right
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + this.gameObject.transform.right);
            ////left
            //Gizmos.color = Color.red;
            //Gizmos.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position - this.gameObject.transform.right);
            ////up
            //Gizmos.color = Color.cyan;
            //Gizmos.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + this.gameObject.transform.up);
            ////down
            //Gizmos.color = Color.magenta;
            //Gizmos.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position - this.gameObject.transform.up);
            //test
            //Gizmos.color = Color.green;
            //Gizmos.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + new Vector3(0f, -Mathf.Sin(CriterionAngle * angletorad), Mathf.Cos(CriterionAngle * angletorad)));
            //Gizmos.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + new Vector3(0f, Mathf.Sin(CriterionAngle * angletorad), Mathf.Cos(CriterionAngle * angletorad)));
            //
            ////test2
            //Gizmos.color = Color.blue;
            //Gizmos.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + new Vector3(-Mathf.Sin(CriterionAngle * angletorad), 0f , Mathf.Cos(CriterionAngle * angletorad)));
            //Gizmos.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + new Vector3(Mathf.Sin(CriterionAngle * angletorad), 0f ,Mathf.Cos(CriterionAngle * angletorad)));

        }


    }
}