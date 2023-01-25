using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_RayCast : MonoBehaviour
{
    // Start is called before the first frame update
    Ray RightRay;
    Ray LeftRay;
    RaycastHit righthit;
    RaycastHit lefthit;
    Vector3 rightpos;
    Vector3 leftpos;
    [SerializeField]
    float raylength;

    [SerializeField]
    float angle;
    float radangle { get { return angle * Mathf.PI / 180; } }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //leftpos = transform.forward - (transform.right * Mathf.Tan(radangle)).normalized;
        //rightpos = transform.forward + (transform.right * Mathf.Tan(radangle)).normalized;
        //RightRay = new Ray(transform.position, rightpos);
        //LeftRay = new Ray(transform.position, leftpos);
        //Physics.Raycast(LeftRay, out lefthit, raylength);
        //Physics.Raycast(RightRay, out righthit, raylength);

        Physics.SphereCast(LeftRay,raylength, out lefthit);

        Debug.Log(lefthit.collider.gameObject.name);
        Debug.Log(righthit.collider.gameObject.name);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, this.transform.position + rightpos * raylength);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, this.transform.position + leftpos * raylength);

        Gizmos.DrawCube(righthit.point, new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.DrawCube(lefthit.point, new Vector3(0.1f, 0.1f, 0.1f));
    }
}
