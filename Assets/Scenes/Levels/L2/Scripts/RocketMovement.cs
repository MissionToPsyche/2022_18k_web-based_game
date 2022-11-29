using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    [SerializeField]
    private float _velocity = 10f;
    private float _acceleration = 0.1f;
    private float _fallAcceleration = 0.5f;
    private float _maxVelocity = 20f;
    private IEnumerator _accelerationCoroutine;
    private IEnumerator _fallCoroutine;
    private bool _buildFinished = false;
    private bool _enginesOn = false;
    private bool _onTheGround = true;
    void Start()
    {
        _accelerationCoroutine = AccelerateRocket();
        _fallCoroutine = RocketFall();
        Init();
    }

    void Update()
    {
        // if engines are on, fly
        if (_enginesOn)
        {
            transform.Translate(Vector2.up * (Time.deltaTime * _velocity));
        }
        // if engines are off and on the air, make the rocket fall
        else if (!_enginesOn && !_onTheGround)
        {
            transform.Translate(Vector2.up * (Time.deltaTime * _velocity));
        }

        if (!_onTheGround)
        {
            // set maximum velocity
            if (Mathf.Abs(_velocity) > (Mathf.Abs(_maxVelocity)))
            {
                // if falling, set max fall velocity as negative
                if (_velocity < 0)
                {
                    _velocity = -_maxVelocity;
                }
                else
                {
                    _velocity = _maxVelocity;
                }
            }
        }
    }
    void Init()
    {
        _enginesOn = false;
    }
    public void BuildFinished()
    {
        _buildFinished = true;
        MakeRocketPartsDynamic();
    }
    public void EnginesOn()
    {
        _enginesOn = true;
        _onTheGround = false;
        StartCoroutine(_accelerationCoroutine);
        StopCoroutine(_fallCoroutine);
        MakeRocketPartsNotSimulated();
    }
    public void EnginesOff()
    {
        _enginesOn = false;
        StopCoroutine(_accelerationCoroutine);
        StartCoroutine(_fallCoroutine);
        MakeRocketPartsGravityZero();
    }

    // every second, add the acceleration to the velocity
    IEnumerator AccelerateRocket()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            _velocity += _acceleration;
        }
    }
    IEnumerator RocketFall()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            _velocity -= _fallAcceleration;
        }
    }
    private void MakeRocketPartsDynamic()
    {
        foreach (Transform child in transform)
        {
            Rigidbody2D childRigidbody = child.GetComponent<Rigidbody2D>();
            childRigidbody.bodyType = RigidbodyType2D.Dynamic;
            childRigidbody.simulated = true;
            childRigidbody.drag = 0.3f;
            childRigidbody.gravityScale = 1f;
        }
    }
    private void MakeRocketPartsNotSimulated()
    {
        foreach (Transform child in transform)
        {
            Rigidbody2D childRigidbody = child.GetComponent<Rigidbody2D>();
            childRigidbody.simulated = false;
        }
    }
    private void MakeRocketPartsGravityZero()
    {
        foreach (Transform child in transform)
        {
            Rigidbody2D childRigidbody = child.GetComponent<Rigidbody2D>();
            childRigidbody.simulated = true;
            childRigidbody.gravityScale = 0f;
        }
    }
    private void MakeRocketPartsGravityOne()
    {
        foreach (Transform child in transform)
        {
            Rigidbody2D childRigidbody = child.GetComponent<Rigidbody2D>();
            childRigidbody.simulated = true;
            childRigidbody.gravityScale = 1f;
        }
    }
    public void RocketHitTheGround()
    {
        _onTheGround = true;
        _velocity = 0;
        StopAccelerations();
        MakeRocketPartsGravityOne();
    }
    private void StopAccelerations()
    {
        StopCoroutine(_accelerationCoroutine);
        StopCoroutine(_fallCoroutine);
    }
}
