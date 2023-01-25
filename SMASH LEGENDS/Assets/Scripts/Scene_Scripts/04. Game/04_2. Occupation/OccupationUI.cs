using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Juhyung
{
    public class OccupationUI : MonoBehaviourPun
    {
        [SerializeField] private GameObject Fight_now;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(OccupationManager.Instance.RedTeamPlayer != 0 && OccupationManager.Instance.BlueTeamPlayer != 0)
            {
                Fight_now.SetActive(true);
            }
            else
            {
                Fight_now.SetActive(false);
            }
        }
    }
}