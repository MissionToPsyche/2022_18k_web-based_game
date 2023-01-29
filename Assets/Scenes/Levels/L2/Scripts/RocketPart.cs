using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPart : MonoBehaviour
{
    Rigidbody2D rocketPartRigidBody;
    public bool isFinishedBuilding = false;
    public bool isPartOfTheRocket = false;
    private float _crashThreshold = 4f;
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
        rocketPartRigidBody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        rocketPartRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX;
        rocketPartRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    void Update()
    {
        // set maximum velocity
        // if (Mathf.Abs(rocketPartRigidBody.velocity.y) > (Mathf.Abs(_maxVelocity)))
        // {
        //     // if falling, set max fall velocity as negative
        //     if (rocketPartRigidBody.velocity.y < 0)
        //     {
        //         rocketPartRigidBody.velocity = new Vector2(rocketPartRigidBody.velocity.x, -_maxVelocity);
        //     }
        //     else
        //     {
        //         rocketPartRigidBody.velocity = new Vector2(rocketPartRigidBody.velocity.x, _maxVelocity);
        //     }
        // }
    }
    void OnMouseDown()
    {
        DetachPart();
    }
    void DetachPart()
    {
        // Detach part
        if (isFinishedBuilding)
        {
            if (gameObject.tag == "Separator")
            {
                transform.SetParent(null); // Detach from parent
                rocketPartRigidBody.bodyType = RigidbodyType2D.Dynamic;
                rocketPartRigidBody.gravityScale = 1f;
                rocketPartRigidBody.velocity = new Vector2(0, -20f);
                isPartOfTheRocket = false;
            }
        }
    }
    void OnFinishedBuilding()
    {
        gameObject.GetComponent<Draggable>().isDraggable = false;
        isFinishedBuilding = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (isPartOfTheRocket)
            {
                // If crash too hard, the rocket crashes and the player loses
                if (gameObject.GetComponentInParent<RocketMovement>().GetSpeed() > _crashThreshold)
                {
                    SendMessageUpwards("OnCrash", SendMessageOptions.RequireReceiver);
                }
                else
                {
                    SendMessageUpwards("OnHitGround", SendMessageOptions.RequireReceiver);
                }
            }
            else
            {
                // If detached part touches the ground, it disappers immediately
                gameObject.SetActive(false);
            }
        }
    }
}
