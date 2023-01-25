using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazor : MonoBehaviour
{
    [SerializeField] ParticleSystem[] beam;

    private void OnEnable()
    {
        foreach(ParticleSystem test in beam)
        {
        }

        GetComponent<ParticleSystem>().Play();
    }
}
