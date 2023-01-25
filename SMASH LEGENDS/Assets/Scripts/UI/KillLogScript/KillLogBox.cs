using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class KillLogBox : MonoBehaviour
    {
        public Queue<KillLogBase> KillLogQueue;

        public void KillLogAdd(KillLogBase log)
        {
            KillLogQueue.Enqueue(log);

            if(KillLogQueue.Count >= 4)
            {
                KillLogQueue.Dequeue();
            }
        }
    }
}
