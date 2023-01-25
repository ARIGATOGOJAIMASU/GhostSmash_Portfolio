using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Wooseok
{
    [ExecuteInEditMode]
    public class Stick : MonoBehaviour
    {
        // Start is called before the first frame update


        public SectorColumnCollider PivotSectorCol;
        public GameObject StickCol;

        void Start()
        {
            if(StickCol.GetComponent<Skill_Colider>())
            {
                StickCol.GetComponent<Skill_Colider>().mainskill = this.PivotSectorCol.ParentSkill;
            }
            if(StickCol.GetComponent<SectorColumnCollider>())
            {
                StickCol.GetComponent<SectorColumnCollider>().ParentSkill = this.PivotSectorCol.ParentSkill;
            }

            Skill_Colider[] Scols = StickCol.GetComponentsInChildren<Skill_Colider>();

            foreach (var scol in Scols)
            {
                scol.mainskill = PivotSectorCol.ParentSkill;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            PivotSectorCol.ParentSkill.SkillEffectOnEnter(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            PivotSectorCol.ParentSkill.SkillEffectOnEnter(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            PivotSectorCol.ParentSkill.SkillEffectOnEnter(other.gameObject);
        }
    }
}