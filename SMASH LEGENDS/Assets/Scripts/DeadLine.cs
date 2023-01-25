using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class DeadLine : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Red") || other.CompareTag("Blue"))
            {
                other.GetComponent<PlayerController_FSM>().Fall();
            }
        }
    }
}
