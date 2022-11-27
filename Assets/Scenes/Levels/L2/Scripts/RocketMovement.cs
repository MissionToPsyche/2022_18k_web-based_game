using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    public float _velocity = 10f;
    public float _acceleration = 1f;
    private IEnumerator _accelerationCoroutine;
    private bool _enginesOn = false;
    private Rigidbody2D _rigidBody;
    void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        _accelerationCoroutine = AccelerateRocket();
        Init();
    }

    void FixedUpdate()
    {
        if (_enginesOn)
        {
            transform.Translate(Vector2.up * (Time.deltaTime * _velocity));
        }
    }
    void Init()
    {
        _enginesOn = false;
        _rigidBody.simulated = false;
        _rigidBody.drag = 0.3f;     // prevents the falling of the rocket from being too fast.
    }
    public void EnginesOn()
    {
        _enginesOn = true;
        _rigidBody.simulated = false;
        StartCoroutine(_accelerationCoroutine);
    }
    public void EnginesOff()
    {
        _enginesOn = false;
        _rigidBody.simulated = true;
        StopCoroutine(_accelerationCoroutine);
    }
    // every second, add the acceleration to the velocity
    IEnumerator AccelerateRocket()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            _velocity += _acceleration;
        }
    }
}
