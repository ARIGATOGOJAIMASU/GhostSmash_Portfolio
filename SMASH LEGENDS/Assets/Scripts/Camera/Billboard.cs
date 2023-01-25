using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class Billboard : MonoBehaviour
    {
        private Transform Camera;
        public bool Hang;

        private void Start()
        {
            Camera = GameObject.Find("CameraManager").transform;
        }

        private void LateUpdate()
        {
            Quaternion a = Quaternion.Euler(Camera.rotation.eulerAngles.x, Camera.rotation.eulerAngles.y, Camera.rotation.eulerAngles.z);
            transform.LookAt(transform.position + a * Vector3.forward,
                Camera.transform.rotation * Vector3.up);

            if (Hang)
            {
                transform.localPosition = new Vector3(0, 2.5f, 0);
            }
            else
            {
                transform.localPosition = new Vector3(0, 1.7f, 0);
            }
        }
    }
}