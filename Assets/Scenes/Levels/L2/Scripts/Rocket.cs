using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private float _acceleration;
    private float _speed = 0f;
    private float _maxSpeed = 10f;
    private float _torque = 0;
    private bool _enginesOn = false;
    private bool _isOnGround = true;
    public float totalThrust = 0f;
    public float totalLeftThrust = 0f;
    public float totalRightThrust = 0f;
    public float totalMass = 0f;
    public float totalLeftMass = 0f;
    public float totalRightMass = 0f;
    public float totalFuel = 0f;
    public float TWR = 0f;
    private List<Transform> rocketParts = new List<Transform>();
    private List<Rigidbody2D> rocketPartRigidbodies = new List<Rigidbody2D>();
    public RocketFollowThis rocketFollowThisScript;
    private Coroutine _accelerationCoroutine;
    public UIManager uiManager;
    void Start()
    {
        Init();
    }

    IEnumerator Accelerate()
    {
        while (true)
        {
            _speed += _acceleration;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Update()
    {
        if (!_isOnGround)
        {
            // Set max speed
            if (Mathf.Abs(_speed) >= (Mathf.Abs(_maxSpeed)))
            {
                // if falling, set max fall velocity as negative
                if (_speed < 0)
                {
                    _speed = -_maxSpeed;
                }
                else
                {
                    _speed = _maxSpeed;
                }
            }
            // Fly in the direction of the rocket
            if (_enginesOn)
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
            }
            // When falling, the rocket needs to fall straight down without care for rotation
            else
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
            }

            transform.Rotate(Vector3.back * _torque * Time.deltaTime);
        }
    }
    void Init()
    {
        _enginesOn = false;
    }
    public void BuildFinished()
    {
        _accelerationCoroutine = StartCoroutine(Accelerate());
        _isOnGround = false;
        GetReferenceToRocketParts();
        EnginesOff();
        foreach (Transform child in rocketParts)
        {
            child.SendMessage("OnFinishedBuilding");
        }
    }
    private void GetReferenceToRocketParts()
    {
        foreach (Transform child in transform)
        {
            rocketParts.Add(child);
            rocketPartRigidbodies.Add(child.GetComponent<Rigidbody2D>());
            if (child.tag == "Capsule")
            {
                rocketFollowThisScript.FindAndFollowCapsule(child);
            }
        }
    }
    public void EnginesOn()
    {
        GetReferenceToRocketParts();
        _isOnGround = false;
        _acceleration = 0.05f;
        _enginesOn = true;

        foreach (Rigidbody2D rb in rocketPartRigidbodies)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

    }
    public void EnginesOff()
    {
        _acceleration = 0.2f;
        _enginesOn = false;
        MakeRocketPartsDynamicWithoutGravity();
    }

    private void MakeRocketPartsDynamicWithoutGravity()
    {
        foreach (Transform child in rocketParts)
        {
            Rigidbody2D childRigidbody = child.GetComponent<Rigidbody2D>();
            childRigidbody.bodyType = RigidbodyType2D.Dynamic;
            childRigidbody.simulated = true;
            childRigidbody.drag = 0.3f;
            childRigidbody.gravityScale = 0f;   // Off gravity so it doesn't affect the movement
            childRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }
    void OnHitGround()
    {
        _isOnGround = true;
        _speed = 0;
        _acceleration = 0;
        if (!uiManager.engineControllerBtnActive)
        {
            uiManager.ActivateEngineControllerBtn();
        }
    }
    void OnCrash()
    {
        gameObject.SetActive(false);
    }
    public float GetSpeed()
    {
        return _speed;
    }
}
