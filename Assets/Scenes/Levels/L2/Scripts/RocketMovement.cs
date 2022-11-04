using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    public float acceleration = 20f;

    void Start()
    {
    }

    void FixedUpdate(){
        transform.Translate(Vector2.up * (Time.deltaTime * acceleration));
    }
}
