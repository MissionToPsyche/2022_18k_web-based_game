using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSounds : MonoBehaviour
{


    public AudioSource audios;
    public AudioClip engine;
    public GameObject ship;
    void Start()
    {
        audios = GetComponent<AudioSource>();
        ship = GameObject.Find("Ship");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            audios.pitch = 1.5f;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            audios.pitch = 2.5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            audios.pitch = 1.5f;
        }
        else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.LeftShift))
        {
            audios.pitch = .5f;
        }

        
        




    }
}
