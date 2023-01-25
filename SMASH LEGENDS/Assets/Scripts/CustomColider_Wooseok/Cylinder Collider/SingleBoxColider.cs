using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{
    public class SingleBoxCollider : MonoBehaviour
    {
        BoxCollider Stick;
        public float Angle;

        public float lifetime;

        private float curtime;

        // Start is called before the first frame update
        void Start()
        {
            this.curtime = 0;
            Stick = this.gameObject.GetComponent<BoxCollider>();
        }
    
        // Update is called once per frame
        void Update()
        {
            if(curtime < lifetime)
            {
                float dt = Time.deltaTime;
                this.transform.Rotate(new Vector3((Angle / lifetime) * Time.deltaTime, 0, 0));
                this.curtime += dt;
            }
            else
            {
                Destroy(this.gameObject);
            }

        }
    }

}
