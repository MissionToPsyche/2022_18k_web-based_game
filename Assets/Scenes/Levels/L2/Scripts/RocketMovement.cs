using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    private float _acceleration;
    private float _speed = 0f;
    private float _maxSpeed = 10f;
    private bool _enginesOn = false;
    private bool _isOnGround = true;
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
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
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
        _isOnGround = false;
        _acceleration = 0.1f;
        _enginesOn = true;
        foreach (Rigidbody2D rb in rocketPartRigidbodies)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
    public void EnginesOff()
    {
        _acceleration = -0.2f;
        _enginesOn = false;
        MakeRocketPartsDynamicWithoutGravity();
    }
    private void MakeRocketPartsDynamic()
    {
        foreach (Transform child in rocketParts)
        {
            Rigidbody2D childRigidbody = child.GetComponent<Rigidbody2D>();
            childRigidbody.bodyType = RigidbodyType2D.Dynamic;
            childRigidbody.simulated = true;
            childRigidbody.drag = 0.3f;
            childRigidbody.gravityScale = 1f;
        }
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
