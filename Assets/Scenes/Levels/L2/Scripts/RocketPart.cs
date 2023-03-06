using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPart : MonoBehaviour
{
    Rigidbody2D rocketPartRigidBody;
    RocketPart rocketPartScript;
    public bool isFinishedBuilding = false;
    public bool isPartOfTheRocket = true;
    public bool isPartOfLeftBooster = false;
    public bool isPartOfRightBooster = false;
    public bool isFirstRocketPart = false;
    public GameObject rocketPartOnTop;
    public GameObject rocketPartOnBottom;
    public GameObject rocketPartOnLeft;
    public GameObject rocketPartOnRight;
    public GameObject snappingPointOnTop;
    public GameObject snappingPointOnBottom;
    public GameObject snappingPointOnLeft;
    public GameObject snappingPointOnRight;
    public GameObject attachedSeparator;
    public GameObject attachedSideSeparator;
    Rocket rocketScript;
    private bool isDetached = false;
    private float _fallSpeed = -15f;
    private float _maxVelocity = -20f;
    private float _crashThreshold = -4f;
    public float mass = 0f;
    public float fuel = 0f;
    public float fuelConsumptionRate = 0f;
    public float thrust = 0f;
    public rocketPart rocketPartBtnReference;
    void Start()
    {
        isPartOfTheRocket = true;
        isPartOfLeftBooster = false;
        isPartOfRightBooster = false;
        rocketPartRigidBody = gameObject.GetComponent<Rigidbody2D>();
        rocketScript = gameObject.GetComponentInParent<Rocket>();
        rocketPartScript = gameObject.GetComponent<RocketPart>();
        Init();
    }
    private void Init()
    {
        // the rocket parts are in a building state. With this rigidbody configuration, they are able to be dragged properly
        rocketPartRigidBody.bodyType = RigidbodyType2D.Static;
        rocketPartRigidBody.simulated = true;
        rocketPartRigidBody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
    }
    void Update()
    {
        // set maximum velocity
        if (isDetached)
        {
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
    void OnMouseDown()
    {
        DetachAllParts();
    }
    void DetachAllParts()
    {
        // Detach part
        if (isFinishedBuilding)
        {
            if (gameObject.tag == "Separator" || gameObject.tag == "SideSeparator")
            {
                GameObject rocketObject = gameObject.transform.parent.gameObject;
                List<Transform> children = new List<Transform>();
                foreach (Transform child in rocketObject.transform)
                {
                    children.Add(child);
                }

                // Remove all parts attached to the separator
                foreach (Transform child in children)
                {
                    RocketPart childScript = child.gameObject.GetComponent<RocketPart>();
                    if (childScript.attachedSeparator == this.gameObject || childScript.attachedSideSeparator == this.gameObject)
                    {
                        DetachPart(child, childScript);
                    }
                }

                // Detach the separator itself
                DetachPart(this.transform, this.GetComponent<RocketPart>());

                // Recalculate mass distribution of the rocket
                rocketObject.GetComponent<Rocket>().CalculateMassDistribution();
            }
        }
    }
    void DetachPart(Transform rocketPart, RocketPart rocketPartScript)
    {
        RemoveRocketPartProperty(rocketPartScript);
        Rigidbody2D rocketPartRb2D = rocketPart.gameObject.GetComponent<Rigidbody2D>();
        rocketPart.SetParent(null); // Detach from parent
        rocketPartRb2D.bodyType = RigidbodyType2D.Dynamic;
        rocketPartRb2D.gravityScale = 1f;
        rocketPartRb2D.velocity = new Vector2(0, _fallSpeed);
        rocketPartScript.isPartOfTheRocket = false;
        rocketPartScript.isDetached = true;
        rocketPartRb2D.constraints = RigidbodyConstraints2D.None;
    }
    public void RemoveRocketPartProperty(RocketPart rocketPartScript)
    {
        // Remove fuel, thrust, and mass
        if (rocketPartScript.fuel > 0)
        {
            SendMessageUpwards("OnReduceFuel", rocketPartScript.fuel, SendMessageOptions.RequireReceiver);
        }
        // If engine, remove thrust and fuel consumption rate
        if (rocketPartScript.thrust > 0)
        {
            SendMessageUpwards("OnReduceTotalThrust", rocketPartScript.thrust, SendMessageOptions.RequireReceiver);
            if (rocketPartScript.isPartOfLeftBooster)
            {
                SendMessageUpwards("OnReduceTotalLeftThrust", rocketPartScript.thrust, SendMessageOptions.RequireReceiver);
            }
            else if (rocketPartScript.isPartOfRightBooster)
            {
                SendMessageUpwards("OnReduceTotalRightThrust", rocketPartScript.thrust, SendMessageOptions.RequireReceiver);
            }
            SendMessageUpwards("OnReduceTotalFuelConsumptionRate", rocketPartScript.fuelConsumptionRate, SendMessageOptions.RequireReceiver);
        }

        // Remove mass
        if (rocketPartScript.mass > 0)
        {
            if (rocketPartScript.isPartOfLeftBooster)
            {
                SendMessageUpwards("OnReduceTotalLeftMass", rocketPartScript.mass, SendMessageOptions.RequireReceiver);
            }
            else if (rocketPartScript.isPartOfRightBooster)
            {
                SendMessageUpwards("OnReduceTotalRightMass", rocketPartScript.mass, SendMessageOptions.RequireReceiver);
            }
            SendMessageUpwards("OnReduceTotalMass", rocketPartScript.mass, SendMessageOptions.RequireReceiver);
        }
    }
    void OnFinishedBuilding()
    {
        // If not part of the rocket, detach the part
        if (!isPartOfTheRocket)
        {
            gameObject.transform.SetParent(null);
            rocketPartRigidBody.bodyType = RigidbodyType2D.Dynamic;
            rocketPartRigidBody.gravityScale = 1f;
        }
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
                rocketScript.isOnGround = true;

                if (rocketScript.GetSpeed() < _crashThreshold)
                {
                    SendMessageUpwards("OnCrash", SendMessageOptions.RequireReceiver);
                }
                else
                {
                    SendMessageUpwards("OnHitGround", SendMessageOptions.RequireReceiver);
                }

                // If not engine and no crash, crash it
                if (gameObject.tag != "Engine")
                {
                    SendMessageUpwards("OnCrash", SendMessageOptions.RequireReceiver);
                }
            }
            else
            {
                // If detached part touches the ground, it disappers immediately
                gameObject.SetActive(false);
            }
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (isPartOfTheRocket)
            {
                rocketScript.isOnGround = false;
                rocketScript.ApplyGravity();
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
        if (attachedSeparator)
        {
            attachedSeparator = null;
        }
        if (attachedSideSeparator)
        {
            attachedSideSeparator = null;
        }
    }
}
