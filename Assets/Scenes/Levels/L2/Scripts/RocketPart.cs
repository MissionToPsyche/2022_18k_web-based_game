using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPart : MonoBehaviour
{
    Rigidbody2D rocketPartRigidBody;
    private float _maxVelocity = 20f;
    void Start()
    {
        rocketPartRigidBody = gameObject.GetComponent<Rigidbody2D>();
        Init();
    }
    private void Init()
    {
        // the rocket parts are in a building state. With this rigidbody configuration, they are able to be dragged properly
        rocketPartRigidBody.bodyType = RigidbodyType2D.Static;
        rocketPartRigidBody.simulated = true;
    }
    void Update()
    {
        // set maximum velocity
        if (Mathf.Abs(rocketPartRigidBody.velocity.y) > (Mathf.Abs(_maxVelocity)))
        {
            // if falling, set max fall velocity as negative
            if (rocketPartRigidBody.velocity.y < 0)
            {
                rocketPartRigidBody.velocity = new Vector2(rocketPartRigidBody.velocity.x, -_maxVelocity);
            }
            else
            {
                rocketPartRigidBody.velocity = new Vector2(rocketPartRigidBody.velocity.x, _maxVelocity);
            }
        }
    }
}
