using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class CameraTarget : MonoBehaviour
    {
        public Transform TagetObj;
        public bool BattleMode;
        public float Distance = 5.5f;

        // Update is called once per frame
        void Update()
        {
            if (TagetObj && BattleMode == false)
            {
                transform.position = TagetObj.position;
            }

            if ((Distance < Vector3.Distance(transform.position, TagetObj.position)))
            {
                BattleMode = false;
                transform.position = TagetObj.position;
                CameraManager.Instance().CameraBack();
            }
        }
    }
}
