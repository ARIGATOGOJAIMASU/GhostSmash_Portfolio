using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    [SerializeField]
    private float Rotate_speed = 6.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.Rotate(0.0f, -Input.GetAxis("Mouse X") * Rotate_speed, 0.0f, Space.World);
        }
    }
}