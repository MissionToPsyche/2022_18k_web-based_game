using InterWorld.Shared.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public RocketPartType type;
    public RocketPartTeir teir = RocketPartTeir.none;
    private RocketInformation rocketInformation;
    public ParticleSystem particleSystem;
    private TextMesh text;
    // Start is called before the first frame update
    void Start()
    {
        rocketInformation = RocketInformation.instance;
        text = transform.parent.GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void OnTriggerEnter2D(Collider2D collision)
    {
        bool isCollisionWithPlayer = collision.gameObject.CompareTag("Player");
        if (!isCollisionWithPlayer)
        {
            return;
        }

        // Collided with player
        short rocketPart = (short)((short)teir | (short)type);
        rocketInformation.Collect(rocketPart);

        particleSystem.Play();
        Destroy(gameObject);
        Destroy(text);
        Destroy(particleSystem.transform.parent.gameObject, particleSystem.main.duration);
    }
}
