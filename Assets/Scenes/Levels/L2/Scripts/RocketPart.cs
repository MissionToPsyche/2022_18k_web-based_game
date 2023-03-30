using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPart : MonoBehaviour
{
    Rigidbody2D rocketPartRigidBody;
    RocketPart rocketPartScript;
    public bool isFinishedBuilding = false;
    public bool isPartOfTheRocket = false;
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
    public bool canBePlaced;
    public GameObject poof;
    private SpriteRenderer spriteRenderer;
    public GameObject engineFire;
    private Animator _engineFireAnimator;
    public GameObject groundExplosion;
    private Animator _groundExplosionAnimator;
    private bool _isOnGround;
    void Start()
    {
        isPartOfTheRocket = false;
        isPartOfLeftBooster = false;
        isPartOfRightBooster = false;
        canBePlaced = true;
        rocketPartRigidBody = gameObject.GetComponent<Rigidbody2D>();
        rocketScript = gameObject.GetComponentInParent<Rocket>();
        rocketPartScript = gameObject.GetComponent<RocketPart>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        poof.SetActive(false);
        if (gameObject.tag == "Engine")
        {
            engineFire.SetActive(true);
            _engineFireAnimator = engineFire.GetComponent<Animator>();
            groundExplosion.SetActive(true);
            _groundExplosionAnimator = groundExplosion.GetComponent<Animator>();
        }
        Init();
    }
    private void Init()
    {
        // the rocket parts are in a building state. With this rigidbody configuration, they are able to be dragged properly
        rocketPartRigidBody.bodyType = RigidbodyType2D.Kinematic;
        rocketPartRigidBody.useFullKinematicContacts = true;
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
        rocketPartScript.isPartOfTheRocket = false;
        rocketPartScript.isDetached = true;

        RemoveRocketPartProperty(rocketPartScript);
        Rigidbody2D rocketPartRb2D = rocketPart.gameObject.GetComponent<Rigidbody2D>();
        rocketPart.SetParent(null); // Detach from parent
        rocketPartRb2D.bodyType = RigidbodyType2D.Dynamic;
        rocketPartRb2D.constraints = RigidbodyConstraints2D.None;
        rocketPartRb2D.gravityScale = 1f;
        rocketPartRb2D.velocity = new Vector2(0, _fallSpeed);
    }
    public void RemoveRocketPartProperty(RocketPart rocketPartScript)
    {
        // Remove fuel, thrust, and mass
        if (rocketPartScript.fuel > 0)
        {
            rocketScript.numberOfFuelTanks--;
            float fuelToSubtract = rocketPartScript.fuel - rocketScript.fuelDrainagePerTank;
            if (fuelToSubtract < 0)
            {
                fuelToSubtract = 0;
            }
            SendMessageUpwards("OnReduceFuel", fuelToSubtract, SendMessageOptions.RequireReceiver);
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
        if (isFinishedBuilding)
        {
            if (collision.collider.tag == "Ground")
            {
                if (isPartOfTheRocket)
                {
                    rocketScript.isOnGround = true;
                    _isOnGround = true;
                    // If crash too hard, the rocket part gets crashed 
                    if (rocketScript.GetSpeed() < _crashThreshold)
                    {
                        RocketPartCrashed();
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
    void RocketPartCrashed()
    {
        RemoveRocketPartProperty(rocketPartScript);
        spriteRenderer.enabled = false;
        poof.SetActive(true);
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (isFinishedBuilding)
        {
            if (collision.collider.tag == "Ground")
            {
                if (isPartOfTheRocket)
                {
                    _isOnGround = false;
                    rocketScript.isOnGround = false;
                    rocketScript.ApplyGravity();
                }
            }
        }
        else
        {
            canBePlaced = true;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" || collision.collider.tag == "TrashCan")
        {
            canBePlaced = false;
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
    public void EngineOff()
    {
        _engineFireAnimator.SetBool("EngineOn", false);
    }
    public void EngineOn()
    {
        // Trigger ground explosion animation if the rocket is on ground
        if (_isOnGround)
        {
            _groundExplosionAnimator.SetTrigger("GroundExplosion");
        }
        // Trigger engine fire animation
        _engineFireAnimator.SetBool("EngineOn", true);

        // Turn on engine audio
        if (!rocketScript.hasLaunched)
        {
            rocketScript.hasLaunched = true;
            if (rocketScript.alreadyPlayingLaunchAudio == false)
            {
                SoundManager.instance.Play("Launch");
                rocketScript.alreadyPlayingLaunchAudio = true;
            }
        }
        else
        {
            if (rocketScript.alreadyPlayingEngineOnAudio == false)
            {
                SoundManager.instance.Play("EngineOn");
                rocketScript.alreadyPlayingEngineOnAudio = true;
            }
        }
    }
}
