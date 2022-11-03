using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    Rigidbody2D rigBody;
    public float movementSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        rigBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){
        rigBody.velocity = new Vector2(0, movementSpeed);
    }
}
