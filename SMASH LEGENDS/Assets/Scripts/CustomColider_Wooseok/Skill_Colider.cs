using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace Wooseok
{
    public class Skill_Colider : MonoBehaviour
    {   
        public Skill mainskill;


        private void OnTriggerEnter(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            //Debug.Log("Enter");
            //Vector3 thistocol = other.ClosestPoint(this.gameObject.transform.position) - this.gameObject.transform.position;
            mainskill.SkillEffectOnEnter(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            //Debug.Log("Exit");
            mainskill.SkillEffectOnExit(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            //Debug.Log("Stay");
            mainskill.SkillEffectOnStay(other.gameObject);
        }


        private void OnDrawGizmos()
        {
            //Gizmos.matrix = this.transform.localToWorldMatrix;
            //Gizmos.color = Color.red;
            //Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }
    }
}