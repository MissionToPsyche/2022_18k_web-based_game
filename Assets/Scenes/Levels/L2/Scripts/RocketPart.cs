using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPart : MonoBehaviour
{
    Rigidbody2D rocketPartRigidBody;
    void Start()
    {
        rocketPartRigidBody = gameObject.GetComponent<Rigidbody2D>();
        // the rocket parts are in a building state. They are able to be dragged properly
        rocketPartRigidBody.bodyType = RigidbodyType2D.Static;
        rocketPartRigidBody.simulated = true;
    }
    void OnCollisionEnter2D(Collision2D target)
    {
        // when the rocket part hits the ground, tell the rocket that it hit the ground
        if (target.gameObject.tag.Equals("Ground") == true)
        {
            this.transform.parent.gameObject.GetComponent<RocketMovement>().RocketHitTheGround();
        }
    }

}
