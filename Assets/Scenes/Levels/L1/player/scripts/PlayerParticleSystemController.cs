using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleSystemController : MonoBehaviour
{
    public ParticleSystem dust;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateDust()
    {
        dust?.Play();
    }
}
