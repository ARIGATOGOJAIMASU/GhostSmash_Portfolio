using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{

    public class KillLog : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(DestroyKillLog());
        }

        IEnumerator DestroyKillLog()
        {
            yield return new WaitForSeconds(1.0f);

            Destroy(this);
        }
    }
}
