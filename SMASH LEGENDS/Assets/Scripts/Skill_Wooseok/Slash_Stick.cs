using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Wooseok
{
    public class Slash_Stick : MonoBehaviour
    {
        [SerializeField]
        Skill originalskill;

        private void OnTriggerEnter(Collider other)
        {
            originalskill.SkillEffectOnEnter(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            originalskill.SkillEffectOnStay(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            originalskill.SkillEffectOnExit(other.gameObject);
        }
    }

}
