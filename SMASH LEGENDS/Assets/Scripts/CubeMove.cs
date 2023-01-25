using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, Time.deltaTime * 500.0f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.GetComponent<Rigidbody>().velocity = new Vector3(-Time.deltaTime * 500.0f, 0, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -Time.deltaTime * 500.0f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.GetComponent<Rigidbody>().velocity = new Vector3(Time.deltaTime * 500.0f, 0, 0);
        }
    }
}
