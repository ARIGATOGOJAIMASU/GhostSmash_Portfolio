using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    List<ParticleSystem> particles;

    private void OnEnable()
    {
        ParticleManager_PlayAll();
    }
    void ParticleManager_PlayAll()
    {
        foreach(ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }

    void ParticleManager_StopAll()
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
