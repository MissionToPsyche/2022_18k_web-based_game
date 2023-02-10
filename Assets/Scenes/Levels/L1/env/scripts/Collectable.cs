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

    // Start is called before the first frame update
    void Start()
    {
        rocketInformation = RocketInformation.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        bool isCollisionWithPlayer = collision.gameObject.CompareTag("Player");
        if (!isCollisionWithPlayer)
        {
            return;
        }

        // Collided with player
        short rocketPart = (short)((short)teir | (short)type);
        rocketInformation.Collect(rocketPart);
        Destroy(gameObject);
    }
}
