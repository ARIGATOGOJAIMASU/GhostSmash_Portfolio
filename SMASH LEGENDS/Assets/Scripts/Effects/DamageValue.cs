using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Junpyo
{
    public class DamageValue : Wooseok.SelfDestroy
    {
        [SerializeField] TextMesh Value_Text;
        private Transform Camera;
        public bool arpa;
        private float Timer;

        private void Start()
        {
            Camera = GameObject.Find("CameraManager").transform;
        }


        public void SetDamage_Value(int value)
        {
            Value_Text.text = value.ToString();
        }

        private void Update()
        {

            Timer += Time.deltaTime;

            if(Timer < 0.4f)
            {
                if (Timer < 0.2f)
                {
                    transform.localScale += new Vector3(Time.deltaTime * 0.5f, Time.deltaTime * 0.5f, Time.deltaTime * 0.5f);
                }
                else if ((Timer > 0.3f))
                {
                    transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
                }

                transform.transform.position += (Vector3.up * Time.deltaTime * 2f);
            }
            else
            {
                if (Value_Text.color.a > 0)
                {
                    Value_Text.color = new Color(0, 0, 0, Value_Text.color.a - Time.deltaTime * 5.0f);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }

            Quaternion a = Quaternion.Euler(Camera.rotation.eulerAngles.x, Camera.rotation.eulerAngles.y, Camera.rotation.eulerAngles.z);
            transform.LookAt(transform.position + a * Vector3.forward,
                Camera.transform.rotation * Vector3.up);
        }
    }
}
