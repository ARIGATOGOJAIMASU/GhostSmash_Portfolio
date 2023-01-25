using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookGroundMove : MonoBehaviour
{
    [SerializeField]
    Transform GroundPos;

    private void FixedUpdate()
    {
        if (Vector3.Distance(GroundPos.position ,transform.position) > 0.1f)
        {
            transform.position = GroundPos.position;
        }
    }
}
