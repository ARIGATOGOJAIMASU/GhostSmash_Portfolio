using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Junpyo
{

    public class KillLogBase : MonoBehaviour
    {
        public GameObject RightLog;
        public GameObject LeftLog;
        private float RemoveTime = 5.0f;

        private void Start()
        {
            StartCoroutine(LogRemove());
        }

        IEnumerator LogRemove()
        {
            //일정시간 이
            yield return new WaitForSeconds(RemoveTime);

            Vector3 PredictionPos = transform.position - (transform.right * 500);
            transform.DOMove(PredictionPos, 1.0f);
            yield return new WaitForSeconds(1.0f);
            Destroy(this.gameObject);          
        }
    }
}
