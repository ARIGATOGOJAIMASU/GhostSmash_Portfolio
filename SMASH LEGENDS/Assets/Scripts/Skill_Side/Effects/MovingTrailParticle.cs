using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrailParticle : MonoBehaviour
{
    [SerializeField]
    GameObject TrackingObj;

    // Start is called before the first frame update
    private void Awake()
    {
        this.transform.parent = null;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        transform.position = TrackingObj.transform.position;
    }
}
