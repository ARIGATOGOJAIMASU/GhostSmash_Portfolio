using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Juhyung
{
    public class OccupationTrigger : MonoBehaviourPun
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Red"))
            {
                OccupationManager.Instance.RedTeamPlayer++;
            }

            if (other.tag.Equals("Blue"))
            {
                OccupationManager.Instance.BlueTeamPlayer++;
            }
            Debug.Log("레드: " + OccupationManager.Instance.RedTeamPlayer + "블루" + OccupationManager.Instance.BlueTeamPlayer);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Red"))
            {
                OccupationManager.Instance.RedTeamPlayer--;
            }

            if (other.tag.Equals("Blue"))
            {
                OccupationManager.Instance.BlueTeamPlayer--;
            }
            Debug.Log("레드: " + OccupationManager.Instance.RedTeamPlayer + "블루" + OccupationManager.Instance.BlueTeamPlayer);
        }
    }
}
