using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPart : MonoBehaviour
{
    Rigidbody2D rocketPartRigidBody;
    public bool isFinishedBuilding = false;
    public bool isPartOfTheRocket = false;
    public GameObject rocketPartOnTop;
    public GameObject rocketPartOnBottom;
    public GameObject rocketPartOnLeft;
    public GameObject rocketPartOnRight;
    public GameObject snappingPointOnTop;
    public GameObject snappingPointOnBottom;
    public GameObject snappingPointOnLeft;
    public GameObject snappingPointOnRight;
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
    public void OnDragStart()
    {
        // If middle part, do not move
        // Detach all snapping points connected to this rocket part
        if (snappingPointOnTop)
        {
            snappingPointOnTop.GetComponent<SnappingPoint>().isAttached = false;
        }
        if (snappingPointOnBottom)
        {
            snappingPointOnBottom.GetComponent<SnappingPoint>().isAttached = false;
        }
        if (snappingPointOnLeft)
        {
            snappingPointOnLeft.GetComponent<SnappingPoint>().isAttached = false;
        }
        if (snappingPointOnRight)
        {
            snappingPointOnRight.GetComponent<SnappingPoint>().isAttached = false;
        }

        if (rocketPartOnTop)
        {
            RocketPart tmp = rocketPartOnTop.GetComponent<RocketPart>();
            tmp.snappingPointOnBottom.GetComponent<SnappingPoint>().isAttached = false;
            tmp.rocketPartOnBottom = null;
            rocketPartOnTop = null;
        }
        if (rocketPartOnBottom)
        {
            RocketPart tmp = rocketPartOnBottom.GetComponent<RocketPart>();
            tmp.snappingPointOnTop.GetComponent<SnappingPoint>().isAttached = false;
            tmp.rocketPartOnTop = null;
            rocketPartOnBottom = null;
        }
        if (rocketPartOnLeft)
        {
            RocketPart tmp = rocketPartOnLeft.GetComponent<RocketPart>();
            tmp.snappingPointOnRight.GetComponent<SnappingPoint>().isAttached = false;
            tmp.rocketPartOnRight = null;
            rocketPartOnLeft = null;
        }
        if (rocketPartOnRight)
        {
            RocketPart tmp = rocketPartOnRight.GetComponent<RocketPart>();
            tmp.snappingPointOnLeft.GetComponent<SnappingPoint>().isAttached = false;
            tmp.rocketPartOnLeft = null;
            rocketPartOnRight = null;
        }
    }
}
